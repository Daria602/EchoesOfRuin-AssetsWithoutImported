using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Serializable]
    public class CharacterInitiative
    {
        public int characterId;
        public int characterInitiative;

        public CharacterInitiative(int characterId, int characterInitiative)
        {
            this.characterId = characterId;
            this.characterInitiative = characterInitiative;
        }
    }
    public List<CharacterInitiative> characterInitiativeList = new List<CharacterInitiative>();

    public int[] characterIdKeys;
    public GameObject[] characterObjectValues;
    public Dictionary<int, GameObject> characters = new Dictionary<int, GameObject>();
    public bool isCombatGoing = false;
    public int turnIndex = 0;
    public int round = 1;

    public GameObject currentCharacter;

    public delegate void EnemyDeathEventHandler(NPCHealth character);




    private static CombatManager instance;
    //public GameObject player;
    //public int currentCharacterTurn = -1;

    public static CombatManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many CombatManager instances. It's a singleton class");
        }
        instance = this;


    }

    private void Start()
    {
        if (characterIdKeys.Length != characterObjectValues.Length)
        {
            Debug.LogError("Keys and values of character id's dictionary have different size");
        }
        else
        {
            for (int i = 0; i < characterIdKeys.Length; i++)
            {
                characters.Add(characterIdKeys[i], characterObjectValues[i]);
            }
        }
    }
    private void Update()
    {
        if (isCombatGoing)
        {
            if (characterInitiativeList[turnIndex].characterId == 0)
            {
                currentCharacter.GetComponent<PlayerCombat>().DoSomething();
            }
            else
            {
                currentCharacter.GetComponent<NPCCombat>().DoSomething();
            }
            


            if (currentCharacter.GetComponent<CombatController>().endedTurn)
            {
                if (turnIndex + 1 == characterInitiativeList.Count)
                {
                    turnIndex = 0;
                    round++;
                }
                else
                {
                    turnIndex++;
                }

                currentCharacter = characters[characterInitiativeList[turnIndex].characterId];
                currentCharacter.GetComponent<CombatController>().endedTurn = false;


            }
        }
    }

    public void StartCombat(List<int> characterIds)
    {
        // Clear the list from previous fight
        characterInitiativeList.Clear();
        // Iterate through the list of characters that participate in the fight
        for (int i = 0; i < characterIds.Count; i++)
        {
            // For each character in the list, set the animation trigger EnterBattle
            characters[characterIds[i]].GetComponent<Animator>().SetTrigger("EnterBattle");
            // If the character is not a player, look at the player
            if (characterIds[i] != Constants.PLAYER_ID)
            {
                characters[characterIds[i]].transform.LookAt(characters[Constants.PLAYER_ID].transform);
            }
            // Set their combat boolean value to true and add them to a list
            characters[characterIds[i]].GetComponent<CombatController>().IsInCombat = true;
            characterInitiativeList.Add(new CharacterInitiative(characterIds[i], CalculateInitiative(characterIds[i])));

        }
        // Sort the list in descending order
        characterInitiativeList.Sort((a, b) => b.characterInitiative.CompareTo(a.characterInitiative));
        // Set the current character to take turn
        currentCharacter = characters[characterInitiativeList[0].characterId];
        currentCharacter.GetComponent<CombatController>().endedTurn = false;
    }

    public int CalculateInitiative(int id)
    {
        return characters[id].GetComponent<Stats>().GetInitiative();

    }
}

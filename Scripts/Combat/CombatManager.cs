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
    private bool m_isCombatGoing = false;
    public bool isCombatGoing { get => m_isCombatGoing; set => CombatValueChanged(value); }
    public int turnIndex = 0;
    public int round = 1;

    private GameObject currentCharacter;

    public delegate void EnemyDeathEventHandler(NPCHealth character);


    // action points
    public GameObject actionPointsUI;
    public GameObject enemyTurnUI;
    public GameObject endTurnButton;

    private static CombatManager instance;
    public GameObject player;

    public GameObject portraitsGrid;
    public GameObject portraitPrefab;

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

    public void CombatValueChanged(bool value)
    {
        if (value != m_isCombatGoing)
        {
            m_isCombatGoing = value;
            if (!value)
            {
                ResetAftermath();
            }
        }
    }

    public void ResetAftermath()
    {
        player.GetComponent<CombatController>().IsInCombat = false;
        player.GetComponent<Animator>().SetTrigger("CombatOver");
        player.GetComponent<CombatController>().DecreaseCooldown(true);
        //player.GetComponent<CombatController>().RemoveWeapon();
        CombatOver();
        //ToggleSkillsVisibility(CombatController.MAX_ACTION_POINTS);
        List<Skill> skills = currentCharacter.GetComponent<CombatController>().skills;
        for (int i = 0; i < skills.Count; i++)
        {
            SkillPanelController.GetInstance().SetButtonInactive(true, i, skills[i].cooldown);

        }

    }




    private void Update()
    {
        if (isCombatGoing)
        {
            if (characterInitiativeList.Count <= 1)
            {
                isCombatGoing = false;
                
                // TODO: end combat
                
            }
            else
            {
                if (characterInitiativeList[turnIndex].characterId == 0)
                {
                    UpdateActionPointsUI(currentCharacter.GetComponent<PlayerCombat>().actionPointsLeft);
                    ToggleSkillsVisibility(currentCharacter.GetComponent<CombatController>().actionPointsLeft);
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
                        for (int i = 0; i < characterInitiativeList.Count; i++)
                        {
                            characters[characterInitiativeList[i].characterId].GetComponent<CombatController>().DecreaseCooldown(false);
                        }
                    }
                    else
                    {
                        turnIndex++;
                    }
                    ShowPortraits();

                    currentCharacter = characters[characterInitiativeList[turnIndex].characterId];
                    currentCharacter.GetComponent<CombatController>().endedTurn = false;
                    if (characterInitiativeList[turnIndex].characterId == Constants.PLAYER_ID)
                    {
                        // enable action point UI
                        EnableActionPointsUI();
                        UpdateActionPointsUI(CombatController.MAX_ACTION_POINTS);
                        List<Skill> skills = currentCharacter.GetComponent<CombatController>().skills;
                        for (int i = 0; i < skills.Count; i++)
                        {
                            SkillPanelController.GetInstance().UpdateCooldown(i, skills[i].cooldown);
                        }
                    }
                    else
                    {
                        // say that it's enemies turn
                        EnableEnemyTurnUI();
                    }
                    currentCharacter.GetComponent<CombatController>().actionPointsLeft = CombatController.MAX_ACTION_POINTS;


                }
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
                characters[characterIds[i]].GetComponent<OutlineController>().IsEnabled = true;
                characters[characterIds[i]].GetComponent<OutlineController>().ChangeColor(0);
            }
            // Set their combat boolean value to true and add them to a list
            characters[characterIds[i]].GetComponent<CombatController>().IsInCombat = true;
            characters[characterIds[i]].GetComponent<Health>().OnHealthUpdateCallback += ShowPortraits;
            characterInitiativeList.Add(new CharacterInitiative(characterIds[i], CalculateInitiative(characterIds[i])));

        }
        // Sort the list in descending order
        characterInitiativeList.Sort((a, b) => b.characterInitiative.CompareTo(a.characterInitiative));
        // Set the current character to take turn
        currentCharacter = characters[characterInitiativeList[0].characterId];
        currentCharacter.GetComponent<CombatController>().endedTurn = false;
        if (characterInitiativeList[0].characterId == Constants.PLAYER_ID)
        {
            // enable action point UI
            EnableActionPointsUI();
            UpdateActionPointsUI(3);
        }
        else
        {
            // say that it's enemies turn
            EnableEnemyTurnUI();
        }

        ShowPortraits();


    }
    private void ClearPortraits()
    {
        Portrait[] portraits = portraitsGrid.GetComponentsInChildren<Portrait>();
        if (portraits != null && portraits.Length > 0)
        {
            foreach (Portrait portrait in portraits)
            {
                portrait.SelfDestroy();
            }
        }
    }

    public void ShowPortraits()
    {
        // clear the previous ones
        ClearPortraits();

        // Set new ones
        for (int i = 0; i < characterInitiativeList.Count; i++)
        {
            int currentTurnIndex;
            if (i + turnIndex >= characterInitiativeList.Count)
            {
                currentTurnIndex = characterInitiativeList.Count - (i + turnIndex);
            }
            else
            {
                currentTurnIndex = i + turnIndex;
            }
            int characterId = characterInitiativeList[currentTurnIndex].characterId;
            Sprite characterPortrait = characters[characterId].GetComponent<CharacterAppearance>().image;
            int currentHealth = characters[characterId].GetComponent<Health>().currentHealth;
            int maxHealth = characters[characterId].GetComponent<Health>().CurrentMaxHealth;
            string characterName = characters[characterId].name == "PlayerInGame" ? "You" : characters[characterId].name;
            GameObject go = Instantiate(portraitPrefab, portraitsGrid.GetComponent<RectTransform>());
            Portrait portrait = go.GetComponent<Portrait>();
            portrait.SetImage(characterPortrait);
            portrait.SetHealthMinMax(0, maxHealth);
            portrait.SetHealthValue(currentHealth);
            portrait.SetName(characterName);
            if (characterId == 0)
            {
                portrait.SetAsAlly();
            }
            else
            {
                portrait.SetAsEnemy();
            }
        }
    }



    public int CalculateInitiative(int id)
    {
        return characters[id].GetComponent<Stats>().GetInitiative();

    }

    public void EnableActionPointsUI()
    {
        actionPointsUI.SetActive(true);
        endTurnButton.SetActive(true);
        enemyTurnUI.SetActive(false);
    }

    public void EnableEnemyTurnUI()
    {
        actionPointsUI.SetActive(false);
        endTurnButton.SetActive(false);
        enemyTurnUI.SetActive(true);
    }

    public void UpdateActionPointsUI(int actionPointsLeft)
    {
        ActionPoint[] ap = actionPointsUI.GetComponentsInChildren<ActionPoint>();
        for (int i = 0; i < ap.Length; i++)
        {
            ap[i].ToggleActive(i < actionPointsLeft);
        }
    }

    public void ToggleSkillsVisibility(int actionPointsLeft)
    {
        List<Skill> skills = currentCharacter.GetComponent<CombatController>().skills;
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].cost > actionPointsLeft)
            {
                SkillPanelController.GetInstance().SetButtonInactive(false, i, skills[i].cooldown);
            }
            
        }
    }

    public void SignalDeath(int id)
    {
        for (int i = 0; i < characterInitiativeList.Count; i++)
        {
            if (characterInitiativeList[i].characterId == id)
            {
                characterInitiativeList.RemoveAt(i);
            }
        }
    }

    public void CombatOver()
    {
        actionPointsUI.SetActive(false);
        endTurnButton.SetActive(false);
        enemyTurnUI.SetActive(false);
        ClearPortraits();
    }
}

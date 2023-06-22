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
                    //Debug.Log(characters[characterInitiativeList[turnIndex].characterId].GetComponent<PlayerCombat>().skills.Count);   
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
                            Constants.GetInstance().characters[characterInitiativeList[i].characterId].GetComponent<CombatController>().DecreaseCooldown(false);
                        }
                    }
                    else
                    {
                        turnIndex++;
                    }
                    //ShowPortraits();

                    currentCharacter = Constants.GetInstance().characters[characterInitiativeList[turnIndex].characterId];
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

    public void AddParticipants(List<int> characterIds)
    {
        for (int i = 0; i < characterIds.Count; i++)
        {
            // For each character in the list, set the animation trigger EnterBattle
            Constants.GetInstance().characters[characterIds[i]].GetComponent<Animator>().SetTrigger("EnterBattle");
            // If the character is not a player, look at the player
            if (characterIds[i] != Constants.PLAYER_ID)
            {
                Constants.GetInstance().characters[characterIds[i]].transform.LookAt(Constants.GetInstance().characters[Constants.PLAYER_ID].transform);
                Constants.GetInstance().characters[characterIds[i]].GetComponent<OutlineController>().IsEnabled = true;
                Constants.GetInstance().characters[characterIds[i]].GetComponent<OutlineController>().ChangeColor(0);
            }
            // Set their combat boolean value to true and add them to a list
            Constants.GetInstance().characters[characterIds[i]].GetComponent<CombatController>().IsInCombat = true;
            Constants.GetInstance().characters[characterIds[i]].GetComponent<Health>().OnHealthUpdateCallback += ShowPortraits;
            Constants.GetInstance().characters[characterIds[i]].GetComponent<Health>().OnDeathCallback += () => Debug.Log("this got triggered");
            characterInitiativeList.Add(new CharacterInitiative(characterIds[i], CalculateInitiative(characterIds[i])));

        }
    }

    public void EnableUI()
    {
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
    }

    public void StartCombat(List<int> characterIds)
    {
        // Clear the list from previous fight
        characterInitiativeList.Clear();
        // Iterate through the list of characters that participate in the fight
        AddParticipants(characterIds);
        // Sort the list in descending order based on initiative
        characterInitiativeList.Sort((a, b) => b.characterInitiative.CompareTo(a.characterInitiative));
        // Set the current character to take turn
        currentCharacter = Constants.GetInstance().characters[characterInitiativeList[0].characterId];
        currentCharacter.GetComponent<CombatController>().endedTurn = false;
        // Show combat UI
        EnableUI();
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

        if (m_isCombatGoing)
        {
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
                Sprite characterPortrait = Constants.GetInstance().characters[characterId].GetComponent<CharacterAppearance>().image;
                int currentHealth = Constants.GetInstance().characters[characterId].GetComponent<Health>().currentHealth;
                int maxHealth = Constants.GetInstance().characters[characterId].GetComponent<Health>().CurrentMaxHealth;
                string characterName = Constants.GetInstance().characters[characterId].name == "PlayerInGame" ? "You" : Constants.GetInstance().characters[characterId].name;
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
        
    }



    public int CalculateInitiative(int id)
    {
        return Constants.GetInstance().characters[id].GetComponent<Stats>().GetInitiative();

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
        turnIndex--;
        if (id == Constants.PLAYER_ID)
        {
            isCombatGoing = false;
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

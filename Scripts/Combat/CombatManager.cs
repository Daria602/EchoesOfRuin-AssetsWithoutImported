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

    //public delegate void EnemyDeathEventHandler(NPCHealth character);


    // action points
    //public GameObject actionPointsUI;
    //public GameObject enemyTurnUI;
    //public GameObject endTurnButton;

    private static CombatManager instance;
    public GameObject player;

    

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
        //CombatOver();
        //ToggleSkillsVisibility(CombatController.MAX_ACTION_POINTS);
        List<Skill> skills = currentCharacter.GetComponent<CombatController>().skills;
        for (int i = 0; i < skills.Count; i++)
        {
            // SkillPanelController.GetInstance().SetButtonInactive(true, i, skills[i].cooldown);

        }

    }




    private void Update()
    {
        if (isCombatGoing)
        {
            //if (characterInitiativeList.Count <= 1)
            //{
            //    isCombatGoing = false;
                
            //    // TODO: end combat
                
            //}
            //else
            //{
                
            if (currentCharacter.GetComponent<CombatController>().endedTurn)
            {

                if (turnIndex + 1 == characterInitiativeList.Count)
                {
                    turnIndex = 0;
                    round++;
                    for (int i = 0; i < characterInitiativeList.Count; i++)
                    {
                        Constants.GetInstance().characters[characterInitiativeList[i].characterId].GetComponent<CombatController>().DecreaseCooldown(false);
                        Constants.GetInstance().characters[characterInitiativeList[i].characterId].GetComponent<CombatController>().actionPointsLeft = 5;
                        Debug.Log("Got to decrease cooldowns of " + Constants.GetInstance().characters[characterInitiativeList[i].characterId].name);
                        foreach (Skill skill in Constants.GetInstance().characters[characterInitiativeList[i].characterId].GetComponent<CombatController>().skills)
                        {
                            Debug.Log(skill.skillName + " of cooldown " + skill.cooldown);
                        }
                    }
                }
                else
                {
                    turnIndex++;
                }
                //ShowPortraits();

                currentCharacter = Constants.GetInstance().characters[characterInitiativeList[turnIndex].characterId];
                currentCharacter.GetComponent<CombatController>().endedTurn = false;
                EnableUI();
                //if (characterInitiativeList[turnIndex].characterId == Constants.PLAYER_ID)
                //{
                //    // enable action point UI
                //    //EnableActionPointsUI();
                //    //UpdateActionPointsUI(CombatController.MAX_ACTION_POINTS);
                //    List<Skill> skills = currentCharacter.GetComponent<CombatController>().skills;
                //    for (int i = 0; i < skills.Count; i++)
                //    {
                //        SkillPanelController.GetInstance().UpdateCooldown(i, skills[i].cooldown);
                //    }
                //}
                //else
                //{
                //    // say that it's enemies turn
                //    //EnableEnemyTurnUI();
                //}
                //currentCharacter.GetComponent<CombatController>().actionPointsLeft = CombatController.MAX_ACTION_POINTS;


            }
            else
            {
                if (characterInitiativeList[turnIndex].characterId == 0)
                {
                    //Debug.Log("Player is doing the turn");
                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        // currentCharacter.GetComponent<CombatController>().endedTurn = true;
                        List<Skill> debugSkills = currentCharacter.GetComponent<CombatController>().skills;
                        foreach (Skill skill in debugSkills)
                        {
                            Debug.Log(skill.cooldown);
                        }

                    }
                    
                    int actionPointsLeft = currentCharacter.GetComponent<CombatController>().actionPointsLeft;
                    CombatUI.GetInstance().UpdateActionPointsUI(actionPointsLeft);
                    CombatUI.GetInstance().UpdateCooldownsOnSkills(currentCharacter.GetComponent<CombatController>().skills);


                    bool targetDied = currentCharacter.GetComponent<PlayerCombat>().PlayersTurn();
                    if (targetDied)
                    {
                        GameObject deadParticipant = currentCharacter.GetComponent<PlayerCombat>().interactable.gameObject;
                        RecalculateBattle(deadParticipant);
                    }
                }
                else
                {
                    bool playerDied = currentCharacter.GetComponent<NPCCombat>().NPCTurn();
                    if (playerDied)
                    {
                        Debug.Log("player dies");
                    }
                    
                }
                


            }
            UpdatePortraits();
        }
    }

    public void ShowPortraits()
    {
        List<int> participantsIds = new List<int>();
        for (int i = turnIndex; i < characterInitiativeList.Count; i++)
        {
            int characterId = characterInitiativeList[i].characterId;
            participantsIds.Add(characterId);
        }
        if (turnIndex > 0)
        {
            for (int i = 0; i < turnIndex; i++)
            {
                int characterId = characterInitiativeList[i].characterId;
                participantsIds.Add(characterId);
            }
        }
        CombatUI.GetInstance().CreatePortraits(participantsIds);
    }

    public void UpdatePortraits()
    {
        CombatUI.GetInstance().ClearPortraits();
        Debug.Log("Cleared portraits");
        //Debug.Break();
        ShowPortraits();
    }

    // TODO: update health + portraits

    public void PlayerEndsTurn()
    {
        if (currentCharacter.GetComponent<CharacterController>().characterId == Constants.PLAYER_ID)
        {
            currentCharacter.GetComponent<CombatController>().endedTurn = true;
        }
        else
        {
            Debug.LogWarning("Something went wrong. A character other than player tried to end the turn");
        }
        
    }

    private void RecalculateBattle(GameObject deadParticipant)
    {
        // get id's of both of them
        // check in initiative list who is the the first from them
        // do the logic
        //int currentCharacterId = currentCharacter.GetComponent<CharacterController>().characterId;
        int deadParticipantId = deadParticipant.GetComponent<CharacterController>().characterId;
        int currentCharacterPosition = turnIndex;
        int deadParticipantPosition = -1;
        for (int i = 0; i < characterInitiativeList.Count; i++)
        {
            if (characterInitiativeList[i].characterId == deadParticipantId)
            {
                deadParticipantPosition = i;
            }
        }

        if (deadParticipantPosition < currentCharacterPosition)
        {
            turnIndex--;
        }
        Debug.Log(characterInitiativeList.Count);
        characterInitiativeList.RemoveAt(deadParticipantPosition);
        Debug.Log(characterInitiativeList.Count);
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
        isCombatGoing = true;
        // Show combat UI
        EnableUI();
        ShowPortraits();
    }
    public void AddParticipants(List<int> characterIds)
    {
        for (int i = 0; i < characterIds.Count; i++)
        {
            // For each character in the list, set the animation trigger EnterBattle
            Debug.Log(Constants.GetInstance().characters[characterIds[i]].name);
            Constants.GetInstance().characters[characterIds[i]].GetComponent<Animator>().SetTrigger("EnterBattle");
            // If the character is not a player, look at the player
            if (characterIds[i] != Constants.PLAYER_ID)
            {
                Constants.GetInstance().characters[characterIds[i]].transform.LookAt(Constants.GetInstance().characters[Constants.PLAYER_ID].transform);
                //Constants.GetInstance().characters[characterIds[i]].GetComponent<OutlineController>().IsEnabled = true;
                //Constants.GetInstance().characters[characterIds[i]].GetComponent<OutlineController>().ChangeColor(0);
            }
            // Set their combat boolean value to true and add them to a list
            Constants.GetInstance().characters[characterIds[i]].GetComponent<CombatController>().IsInCombat = true;
            //Constants.GetInstance().characters[characterIds[i]].GetComponent<Health>().OnHealthUpdateCallback += ShowPortraits;
            //Constants.GetInstance().characters[characterIds[i]].GetComponent<Health>().OnDeathCallback += () => Debug.Log("this got triggered");
            characterInitiativeList.Add(new CharacterInitiative(characterIds[i], CalculateInitiative(characterIds[i])));

        }
    }
    public int CalculateInitiative(int id)
    {
        return Constants.GetInstance().characters[id].GetComponent<Stats>().GetInitiative();

    }

    public void EnableUI()
    {
        CombatUI.GetInstance().EnableUI(currentCharacter.GetComponent<CharacterController>().characterId);
    }


    //public void ShowPortraits()
    //{
    //    // clear the previous ones
    //    //ClearPortraits();

    //    if (m_isCombatGoing)
    //    {
    //        // Set new ones
    //        for (int i = 0; i < characterInitiativeList.Count; i++)
    //        {
    //            int currentTurnIndex;
    //            if (i + turnIndex >= characterInitiativeList.Count)
    //            {
    //                currentTurnIndex = characterInitiativeList.Count - (i + turnIndex);
    //            }
    //            else
    //            {
    //                currentTurnIndex = i + turnIndex;
    //            }
    //            int characterId = characterInitiativeList[currentTurnIndex].characterId;
    //            Sprite characterPortrait = Constants.GetInstance().characters[characterId].GetComponent<CharacterAppearance>().image;
    //            int currentHealth = Constants.GetInstance().characters[characterId].GetComponent<Health>().currentHealth;
    //            int maxHealth = Constants.GetInstance().characters[characterId].GetComponent<Health>().CurrentMaxHealth;
    //            string characterName = Constants.GetInstance().characters[characterId].name == "PlayerInGame" ? "You" : Constants.GetInstance().characters[characterId].name;
    //            GameObject go = Instantiate(portraitPrefab, portraitsGrid.GetComponent<RectTransform>());
    //            Portrait portrait = go.GetComponent<Portrait>();
    //            portrait.SetImage(characterPortrait);
    //            portrait.SetHealthMinMax(0, maxHealth);
    //            portrait.SetHealthValue(currentHealth);
    //            portrait.SetName(characterName);
    //            if (characterId == 0)
    //            {
    //                portrait.SetAsAlly();
    //            }
    //            else
    //            {
    //                portrait.SetAsEnemy();
    //            }
    //        }
    //    }

    //}











    public void ToggleSkillsVisibility(int actionPointsLeft)
    {
        List<Skill> skills = currentCharacter.GetComponent<CombatController>().skills;
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].cost > actionPointsLeft)
            {
                //SkillPanelController.GetInstance().SetButtonInactive(false, i, skills[i].cooldown);
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

    
}

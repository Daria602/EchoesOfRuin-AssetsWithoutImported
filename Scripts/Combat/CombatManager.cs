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
    public bool IsCombatGoing { get => m_isCombatGoing; set => m_isCombatGoing = value; }
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

    [Serializable]
    public class CharacterBuff
    {
        public int characterId;
        public int turnsCountdown;
        public Buff buff;
        public CharacterBuff(
            int characterId, 
            int turnsCountdown, 
            Constants.Attributes[] attributesBuffed,
            Constants.Abilities[] abilitiesBuffed)
        {
            this.characterId = characterId;
            this.turnsCountdown = turnsCountdown;
            this.buff = new Buff(attributesBuffed, abilitiesBuffed);
        }
        public bool DecreaseCountdown()
        {
            turnsCountdown--;
            if (turnsCountdown == 0)
            {
                RemoveBuff();
                return true;
            }
            return false;
        }

        private void RemoveBuff()
        {
            foreach (Constants.Attributes attribute in buff.attributesBuffed)
            {
                Constants.GetInstance().characters[characterId].GetComponent<Stats>().attributes.TempModifyAttribute(attribute, false);
            }
            foreach (Constants.Abilities ability in buff.abilitiesBuffed)
            {
                Constants.GetInstance().characters[characterId].GetComponent<Stats>().abilities.TempModifyAbility(ability, false);
            }
        }
    }
    [Serializable]
    public class Buff
    {
        public Constants.Attributes[] attributesBuffed;
        public Constants.Abilities[] abilitiesBuffed;
        public Buff(Constants.Attributes[] attributesBuffed, Constants.Abilities[] abilitiesBuffed)
        {
            this.attributesBuffed = attributesBuffed;
            this.abilitiesBuffed = abilitiesBuffed;
        }
    }
    public List<CharacterBuff> characterBuffs = new List<CharacterBuff>();

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
                //ResetAftermath();
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
        if (IsCombatGoing)
        {
            if (characterInitiativeList.Count > 1)
            {
                if (currentCharacter.GetComponent<CombatController>().endedTurn)
                {

                    if (turnIndex + 1 == characterInitiativeList.Count)
                    {
                        turnIndex = 0;
                        round++;
                        DecreaseSkillCooldown();
                        DecreaseBuffCountdown();
                    }
                    else
                    {
                        turnIndex++;
                    }

                    SetNewCharacter();
                    EnableUI();
                }
                else
                {
                    if (characterInitiativeList[turnIndex].characterId == Constants.PLAYER_ID)
                    {
                        PlayerTurn();
                    }
                    else
                    {
                        EnemyTurn();

                    }

                }
                UpdatePortraits();
            }
            else
            {
                IsCombatGoing = false;
                EndCombat();
            }
        }
    }

    private void EndCombat()
    {
        // reset all the buffs
        ResetBuffs();
        // set player out of combat: movement, animator
        SetPlayerOutOfCombat();
        // reset cooldowns to 0
        player.GetComponent<CombatController>().DecreaseCooldown(true);
        // make action points max
        player.GetComponent<CombatController>().actionPointsLeft = Constants.MAX_ACTION_POINTS;
        // set ui out of combat
        DisableUI();
        // refresh character initiative list
        characterInitiativeList.Clear();
        currentCharacter = null;
        turnIndex = 0;
        round = 1;
        SkillPanelController.GetInstance().MakeSkillsUnclickable();

    }

    private void ResetBuffs()
    {
        while (characterBuffs.Count > 0)
        {
            DecreaseBuffCountdown();
        }
    }

    private void DisableUI()
    {
        CombatUI.GetInstance().DisableUI();
    }

    private void SetPlayerOutOfCombat()
    {
        player.GetComponent<CombatController>().IsInCombat = false;
        player.GetComponent<Animator>().SetTrigger("CombatOver");
        
    }

    private void DecreaseSkillCooldown()
    {
        for (int i = 0; i < characterInitiativeList.Count; i++)
        {
            Constants.GetInstance().characters[characterInitiativeList[i].characterId].GetComponent<CombatController>().DecreaseCooldown(false);
            Constants.GetInstance().characters[characterInitiativeList[i].characterId].GetComponent<CombatController>().actionPointsLeft = 5;
            // Debug.Log("Got to decrease cooldowns of " + Constants.GetInstance().characters[characterInitiativeList[i].characterId].name);
            //foreach (Skill skill in Constants.GetInstance().characters[characterInitiativeList[i].characterId].GetComponent<CombatController>().skills)
            //{
            //    Debug.Log(skill.skillName + " of cooldown " + skill.cooldown);
            //}
        }
    }

    private void DecreaseBuffCountdown()
    {
        int index = 0;
        while (index < characterBuffs.Count)
        {
            if (characterBuffs[index].DecreaseCountdown())
            {
                StatsUI.GetInstance().SetAbilityValues();
                characterBuffs.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
    }

    private void SetNewCharacter()
    {
        currentCharacter = Constants.GetInstance().characters[characterInitiativeList[turnIndex].characterId];
        currentCharacter.GetComponent<CombatController>().actionsTakenThisTurn = 0;
        currentCharacter.GetComponent<CombatController>().endedTurn = false;
    }

    private void PlayerTurn()
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

    private void EnemyTurn()
    {
        bool playerDied = currentCharacter.GetComponent<NPCCombat>().NPCTurn();
        if (playerDied)
        {
            IsCombatGoing = false;
            GameOver();
        }
    }

    private void GameOver()
    {
        CombatUI.GetInstance().ShowPlayerDeath();
        PlayerDies();
        //Debug.LogError("Game over should be here");
    }

    private void PlayerDies()
    {
        // reset all the buffs
        ResetBuffs();
        // set enemies out of combat: movement, animator
        SetEnemiesOutOfCombat();
        // reset cooldowns to 0
        //player.GetComponent<CombatController>().DecreaseCooldown(true);
        // make action points max
        //player.GetComponent<CombatController>().actionPointsLeft = Constants.MAX_ACTION_POINTS;
        // set ui out of combat
        DisableUI();
        characterInitiativeList.Clear();
        currentCharacter = null;
        turnIndex = 0;
        round = 1;
        UIManager.GetInstance().RemoveUI();
        player.SetActive(false);
        // TODO: remove skill panel
        SkillPanelController.GetInstance().DisableSkillPanel();

    }

    private void SetEnemiesOutOfCombat()
    {
        foreach (CharacterInitiative characterInitiative in characterInitiativeList)
        {
            if (characterInitiative.characterId != Constants.PLAYER_ID)
            {
                GameObject ch = Constants.GetInstance().characters[characterInitiative.characterId];
                ch.GetComponent<CombatController>().IsInCombat = false;
            }
            
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
        ShowPortraits();
    }

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
        Constants.GetInstance().charactersDead.Add(deadParticipantId);
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
        characterInitiativeList.RemoveAt(deadParticipantPosition);
        deadParticipant.GetComponent<Animator>().SetTrigger("Death");
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
        IsCombatGoing = true;
        // Show combat UI
        EnableUI();
        ShowPortraits();
        // SkillPanelController.GetInstance().MakeSkillsClickable();
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
            }
            // Set their combat boolean value to true and add them to a list
            Constants.GetInstance().characters[characterIds[i]].GetComponent<CombatController>().IsInCombat = true;
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

    public void ApplyBuff(Skill skill)
    {
        foreach (var attribute in skill.attributes)
        {
            currentCharacter.GetComponent<Stats>().attributes.TempModifyAttribute(attribute, true);
        }
        foreach (var ability in skill.abilities)
        {
            currentCharacter.GetComponent<Stats>().abilities.TempModifyAbility(ability, true);
        }

        int characterId = currentCharacter.GetComponent<CharacterController>().characterId;
        int turnsCountdown = skill.buffsForTurns;
        Constants.Attributes[] attributes = skill.attributes;
        Constants.Abilities[] abilities = skill.abilities;
        CharacterBuff cb = new CharacterBuff(characterId, turnsCountdown, attributes, abilities);
        characterBuffs.Add(cb);
        StatsUI.GetInstance().SetAbilityValues(); 
    }









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
            IsCombatGoing = false;
        }
    }

    
}

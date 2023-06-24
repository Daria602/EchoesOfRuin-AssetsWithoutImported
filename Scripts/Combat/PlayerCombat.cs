using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : CombatController
{
    bool isPreparingToAffect = false;
    public Interactable interactable;
    Vector3 pointInScene;

    public bool donePerforming = false;

    private enum PlayerAttackStates
    {
        SelectingSkill,
        SelectingTarget,
        Attacking,
        Done
    }
    private PlayerAttackStates state = PlayerAttackStates.SelectingSkill;

    public bool PlayersTurn()
    {
        switch (state)
        {
            case PlayerAttackStates.SelectingSkill:
                // wait for click on skill
                // ButtonIndexClicked
                //Debug.Log("Selecting a skill");
                break;
            case PlayerAttackStates.SelectingTarget:
                // wait to select a target
                //Debug.Log("Selecting a target");
                SelectingTarget();
                break;
            case PlayerAttackStates.Attacking:
                // do the animation of attacking and deal damage
                //Debug.Log("Attacking");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    SetDoneCasting();
                }
                break;
            case PlayerAttackStates.Done:
                //Debug.Log("Done");
                state = PlayerAttackStates.SelectingSkill;
                break;
                // done
                //break;
        }
        if (enemyDied)
        {
            enemyDied = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ButtonIndexClicked(int buttonIndex)
    {
        actionIndex = buttonIndex;
        GetComponent<Animator>().SetBool("isMagicAoE", true);
        state = PlayerAttackStates.SelectingTarget;
    }

    public void SelectingTarget()
    {
        if (InputManager.GetInstance().LeftMouseClicked())
        {
            Constants.ClickType clickType = InputManager.GetInstance().GetClickType(out pointInScene, out interactable);
            if (clickType == Constants.ClickType.Interact)
            {
                GetComponent<Animator>().SetTrigger("CastMagicAoE");
                state = PlayerAttackStates.Attacking;
            }

        }
        else if (InputManager.GetInstance().RightMouseClicked())
        {
            // cancel the attack
            // state = PlayerAttackStates.SelectingSkill;
        }
        
    }

    public void Attacking()
    {
        // set the animation 
    }

    private bool enemyDied = false;
    public void SetDoneCasting()
    {
        GetComponent<Animator>().SetBool("isMagicAoE", false);
        // deal damage to the enemy
        state = PlayerAttackStates.Done;
        enemyDied = DealDamageToEnemy();
        skills[actionIndex].cooldown = skills[actionIndex].maxCooldown;
        actionPointsLeft -= skills[actionIndex].cost;
    }

    public bool DealDamageToEnemy()
    {
        int damageAfflicted = GetDamageDealt();
        // returns true if character died
        if (interactable.gameObject.GetComponent<NPCHealth>().GetDamaged(damageAfflicted))
        {
            // return true for death of an enemy
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetDamageDealt()
    {
        return 5;
        //return skills[actionIndex].baseDamageMax;
    }

    
    //public void SetDoneCasting()
    //{
    //    donePerforming = true;
    //}

    //private void Update()
    //{
    //    if (IsInCombat)
    //    {
    //        //Debug.Log("Player is in combat");

    //        if (isPreparingToAffect)
    //        {
    //            // do the animation and circle around
    //            WaitForClick();
    //            Debug.Log("Waiting for a click");
    //        }
    //        else if (isPerformingAction)
    //        {
    //            //Debug.Log(skills[actionIndex].name);
    //            Debug.Log("Performing the action");
    //            //doAction();
    //            if (donePerforming)
    //            {
    //                interactable.gameObject.GetComponent<NPCHealth>().GetDamaged(CalculateAffectedDamage());
    //                gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].prepareAnimationBoolName, false);
    //                skills[actionIndex].RemoveEffect();
    //                donePerforming = false;
    //                isPerformingAction = false;
    //            }

    //        }

    //    }
    //}

    public bool AddNewSkill(int skillId)
    {
        for (int i = 0; i < skillsIds.Count; i++)
        {
            if (skillsIds[i] == skillId)
            {
                return false;
            }
        }
        skillsIds.Add(skillId);
        AddSkills();
        return true;
    }
    

    private void CancelPrepare()
    {
        if (skills[actionIndex].hasAnimTrigger)
        {
            //Debug.Log("Got here");
            gameObject.GetComponent<Animator>().SetTrigger(skills[actionIndex].castTrigger);
            
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, true);
        } 
        
    }

    private void CancelAction()
    {
        isPreparingToAffect = false;
        isPerformingAction = false;
        finishedPerforming = true;
        if (skills[actionIndex].prepareAnimationBoolName != "")
        {
            gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].prepareAnimationBoolName, false);
        }
        
        //gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, false);
        skills[actionIndex].RemoveEffect();
    }

    private void PerformAction(int skillIndex)
    {
        isPerformingAction = false;
        isPreparingToAffect = true;
        finishedPerforming = false;
        actionIndex = skillIndex;
        // Sets the visual effect around the character
        skills[skillIndex].SetEffect(gameObject);
        //if (skills[skillIndex].prepareAnimationBoolName != "")
        //{
        //    GetComponent<Animator>().SetBool(skills[skillIndex].prepareAnimationBoolName, true);
        //}
        
        //if (skills[skillIndex].name == "Move")
        //{
        //    //isPerformingAction = false;
        //    //isPreparingToAffect = true;
        //}

        Debug.Log("Performing skill " + skills[skillIndex].name);
        //Debug.Log("Animation is set to " + GetComponent<Animator>().GetBool(skills[skillIndex].prepareAnimationBoolName));
    }

    //public void ButtonIndexClicked(int buttonIndex)
    //{
    //    // TODO: create something to check what actions are displayed
    //    // For example, character can have 11 skills
    //    // But the bar can display only 10
    //    // If the first set of 10 is displayed and the character clicks on button_0
    //    // actionIndex = 0, 
    //    // else actionIndex should recalculate
    //    if (CombatManager.GetInstance().isCombatGoing)
    //    {
    //        //int skillIndex = 0;
    //        if (buttonIndex < skills.Count)
    //        {
    //            PerformAction(buttonIndex);
    //        }
    //        else
    //        {
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        return;
    //    }
        
        
    //}

    

    public void EndTurn()
    {
        
        Debug.Log("Got here");
        endedTurn = true;
        
    }

    public void LoadGameData(CharacterData characterData)
    {
        this.IsInCombat = characterData.IsInCombat;
        this.characterId = characterData.characterId;
        this.skillsIds = characterData.skillsIds;
        this.hasWeapon = characterData.hasWeapon;
        this.weaponId = characterData.weaponId;
        //this.rightHand = characterData.rightHand;
        this.isPerformingAction = characterData.isPerformingAction;
        this.finishedPerforming = characterData.finishedPerforming;
        this.actionIndex = characterData.actionIndex;
        this.charactersTurn = characterData.charactersTurn;

        //Most likely we don't need to call the below methods from here since they're in the Start() method of CombatController, but to be tested

        //AddSkills();  
        //EquipWeapon();
}
    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.IsInCombat = this.IsInCombat;
        characterData.characterId = this.characterId;
        characterData.skillsIds = this.skillsIds;
        characterData.hasWeapon = this.hasWeapon;
        characterData.weaponId = this.weaponId;
        characterData.isPerformingAction = this.isPerformingAction;
        characterData.finishedPerforming = this.finishedPerforming;
        characterData.actionIndex = this.actionIndex;
        characterData.charactersTurn = this.charactersTurn;
    }
}

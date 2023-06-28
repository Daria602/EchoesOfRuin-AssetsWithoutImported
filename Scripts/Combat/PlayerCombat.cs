using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : CombatController
{
    //bool isPreparingToAffect = false;
    public Interactable interactable;
    Vector3 pointInScene;

    public bool donePerforming = false;

    private enum PlayerAttackStates
    {
        SelectingSkill,
        SelectingTarget,
        Attacking,
        Moving,
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
                break;
            case PlayerAttackStates.SelectingTarget:
                // wait to select a target
                SelectingTarget();
                break;
            case PlayerAttackStates.Attacking:
                // do the animation of attacking and deal damage
                // animation event calls SetDoneCasting or SetDoneBuff
                break;
            case PlayerAttackStates.Moving:
                CheckIfArrived();
                break;
            case PlayerAttackStates.Done:
                // done
                state = PlayerAttackStates.SelectingSkill;
                break;
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
        state = PlayerAttackStates.SelectingTarget;
    }

    public void SelectingTarget()
    {
        DisplayDistance();
        if (InputManager.GetInstance().LeftMouseClicked())
        {
            Constants.ClickType clickType = InputManager.GetInstance().GetClickType(out pointInScene, out interactable);
            if (clickType == Constants.ClickType.Interact)
            {
                if (IsInDistance())
                {
                    transform.LookAt(interactable.transform);
                    if (skills[actionIndex].skillName == "Basic Attack")
                    {
                        SetBasicAttackAnimation();

                    }
                    else
                    {
                        GetComponent<Animator>().SetTrigger("CastMagicAoE");
                    }

                    state = PlayerAttackStates.Attacking;
                }
                else
                {
                    CancelAttack();
                }
                
            }
            else if (clickType == Constants.ClickType.Move)
            {
                MoveAction();
                
            }
            else if (clickType == Constants.ClickType.Self)
            {
                Debug.Log("Self hit");
                if (skills[actionIndex].spellType == Constants.SpellType.Buff)
                {
                    Debug.Log(skills[actionIndex].skillName);
                    GetComponent<Animator>().SetTrigger("CastBuff");
                    state = PlayerAttackStates.Attacking;


                }
                //CancelAttack();
            }
            else
            {
                CancelAttack();
            }

        }
        else if (InputManager.GetInstance().RightMouseClicked())
        {
            CancelAttack();
        }
        
    }

    private void CheckIfArrived()
    {
        if (GetComponent<PlayerMovement>().HasArrived())
        {
            GetComponent<PlayerMovement>().IsAllowedToMove = false;
            HideDistance();
            skills[actionIndex].cooldown = skills[actionIndex].maxCooldown;

            state = PlayerAttackStates.Done;
        }
    }
    private void MoveAction()
    {
        if (skills[actionIndex].skillName == "Move")
        {
            int moveCost = MoveCost(pointInScene);
            if (moveCost <= actionPointsLeft)
            {
                if (GetComponent<PlayerMovement>().CanGetToDestination(pointInScene))
                {
                    GetComponent<PlayerMovement>().IsAllowedToMove = true;
                    GetComponent<PlayerMovement>().SetCombatStopDist();
                    GetComponent<Animator>().SetBool("isRunning", true);
                    GetComponent<PlayerMovement>().MovePlayer(pointInScene);
                    actionPointsLeft -= moveCost;
                    state = PlayerAttackStates.Moving;
                }
                else
                {
                    CancelAttack();
                }
            }
            else
            {
                CancelAttack();
            }
        }
        else
        {
            CancelAttack();
        }
    }

    private int MoveCost(Vector3 pointInScene)
    {
        float distance = Vector3.Distance(transform.position, pointInScene);
        int cost = (int)(distance / Constants.DISTANCE_COST_UNIT);
        Debug.Log("Cost: " + cost + "; float is " + distance / Constants.DISTANCE_COST_UNIT);
        return cost;
    }
    private void DisplayDistance()
    {
        bool isMoveSkill = skills[actionIndex].skillName == "Move";
        CombatUI.GetInstance().ShowDistance(skills[actionIndex].maxDistance, isMoveSkill);
    }

    private void HideDistance()
    {
        CombatUI.GetInstance().DisableDistance();
    }

    private void CancelAttack()
    {
        HideDistance();
        state = PlayerAttackStates.SelectingSkill;
    }
    private bool IsInDistance()
    {
        float distance = Vector3.Distance(transform.position, interactable.transform.position);
        float maxAllowedDistance = skills[actionIndex].maxDistance;
        if ( distance <= maxAllowedDistance)
        {
            return true;
        }
        return false;
    }

    private void SetBasicAttackAnimation()
    {
        if (hasWeapon)
        {
            Weapon currentWeapon = GetWeapon();
            //Debug.Log("Weapon is null: " + currentWeapon == null);
            if (currentWeapon != null)
            {
                switch (currentWeapon.weaponType)
                {
                    case Constants.WeaponTypes.Axe:
                        GetComponent<Animator>().SetTrigger("BasicAxe");
                        Debug.Log("Basic with Axe");
                        break;
                    case Constants.WeaponTypes.Bow:
                        transform.Rotate(0, 90, 0);
                        GetComponent<Animator>().SetTrigger("BasicBow");
                        Debug.Log("Basic with Bow");
                        break;
                    case Constants.WeaponTypes.Wand:
                        GetComponent<Animator>().SetTrigger("BasicWand");
                        Debug.Log("Basic with Wand");
                        break;
                }
            }

        }
    }

    public void Attacking()
    {
        // set the animation 
    }

    private bool enemyDied = false;
    public void SetDoneCasting()
    {
        HideDistance();
        if (GetWeapon() != null)
        {
            if (GetWeapon().weaponType == Constants.WeaponTypes.Bow)
            {
                transform.Rotate(0, -90, 0);
            }
        }
        
        // deal damage to the enemy
        state = PlayerAttackStates.Done;
        enemyDied = DealDamageToEnemy();
        skills[actionIndex].cooldown = skills[actionIndex].maxCooldown;
        actionPointsLeft -= skills[actionIndex].cost;
    }

    public void SetDoneBuff()
    {
        state = PlayerAttackStates.Done;
        enemyDied = false;
        CombatManager.GetInstance().ApplyBuff(skills[actionIndex]);
        skills[actionIndex].cooldown = skills[actionIndex].maxCooldown;
        actionPointsLeft -= skills[actionIndex].cost;
    }

    public bool DealDamageToEnemy()
    {
        int damageAfflicted = GetDamageDealt(skills[actionIndex]);
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

    public void EndTurn()
    {
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

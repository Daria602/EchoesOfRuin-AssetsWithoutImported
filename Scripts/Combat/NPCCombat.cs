using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombat : CombatController
{
    private Skill selectedSkill = null;
    public GameObject enemySaysPrefab;
    private GameObject enemySaysInstance;
    private bool startedSaying = false;
    private bool playerDied = false;



    private enum NPCAttackState
    {
        SelectingSkill,
        Attacking,
        Moving,
        Done

    }
    private NPCAttackState state = NPCAttackState.SelectingSkill;
    public bool NPCTurn()
    {
        //Debug.Log("State: " + state.ToString() + "; selectedSkill is null: " + (selectedSkill == null).ToString());
        switch (state)
        {
            case NPCAttackState.SelectingSkill:
                selectedSkill = ChooseSkill();
                if (selectedSkill == null)
                {
                    state = NPCAttackState.Done;
                }
                else
                {
                    actionsTakenThisTurn++;
                    state = NPCAttackState.Attacking;
                    Attack();
                }
                break;
            case NPCAttackState.Attacking:
                break;
            case NPCAttackState.Moving:
                CheckIfArrived();
                break;
            case NPCAttackState.Done:
                if (selectedSkill == null)
                {
                    // say argh
                    if (!startedSaying)
                    {
                        StartCoroutine(WaitBeforeFinishingTurn());
                        startedSaying = true;
                    }
                }
                else
                {
                    state = NPCAttackState.SelectingSkill;
                }
                
                break;
        }
        if (playerDied)
        {
            playerDied = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator WaitBeforeFinishingTurn()
    {
        if (enemySaysInstance == null)
        {
            enemySaysInstance = Instantiate(enemySaysPrefab, transform);
        }
        yield return new WaitForSeconds(3f);
        if (enemySaysInstance != null)
        {
            Destroy(enemySaysInstance);
        }
        endedTurn = true;
        startedSaying = false;
        state = NPCAttackState.SelectingSkill;
    }

    private void CheckIfArrived()
    {
        if (GetComponent<NPCMovement>().HasArrived())
        {
            GetComponent<NPCMovement>().IsAllowedToMove = false;
            GetComponent<NPCMovement>().ResetSpeed();
            state = NPCAttackState.Done;
        }
    }

    public void Attack()
    {
        if (selectedSkill.skillName == "Move")
        {
            MoveTowardsPlayer();
            
        }
        else if (selectedSkill.spellType == Constants.SpellType.Buff)
        {
            GetComponent<Animator>().SetTrigger("CastBuff");
            actionPointsLeft -= selectedSkill.cost;
            if (skills[actionIndex].hasVisuals)
            {
                skills[actionIndex].SetEffect(gameObject);
            }
        }
        else
        {
            if (selectedSkill.skillName == "Basic Attack")
            {
                SetBasicAttackAnimation();

            }
            else
            {
                if (selectedSkill.hasAnimTrigger)
                {
                    GetComponent<Animator>().SetTrigger(selectedSkill.castTrigger);
                }
                else
                {
                    GetComponent<Animator>().SetTrigger("CastMagicAoE");
                }

            }
            //GetComponent<Animator>().SetTrigger("CastMagicAoE");
            actionPointsLeft -= selectedSkill.cost;
            if (selectedSkill.hasVisuals)
            {
                selectedSkill.SetEffect(gameObject);
            }
        }
        selectedSkill.cooldown = selectedSkill.maxCooldown;
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
                        //transform.Rotate(0, 90, 0);
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
    private void MoveTowardsPlayer()
    {
        Vector3 playerPosition = FindObjectOfType<PlayerController>().transform.position;
        Vector3? possiblePointToMoveTo = CalculatePointTomoveTo(playerPosition);
        
        if (possiblePointToMoveTo.HasValue)
        {
            Vector3 pointToMoveTo = possiblePointToMoveTo.Value;
            if (GetComponent<NPCMovement>().CanGetToDestination(pointToMoveTo))
            {
                GetComponent<NPCMovement>().IsAllowedToMove = true;
                GetComponent<NPCMovement>().SetCombatStopDist();
                GetComponent<NPCMovement>().SetCombatSpeed();
                GetComponent<Animator>().SetBool("isRunning", true);
                GetComponent<NPCMovement>().MoveNPC(pointToMoveTo);
                actionPointsLeft -= (int)(Vector3.Distance(playerPosition, pointToMoveTo) / Constants.DISTANCE_COST_UNIT);
                state = NPCAttackState.Moving;
            }
            else
            {
                state = NPCAttackState.Done;
                Debug.Log("NPCCombat, can't get to destination: " + pointToMoveTo);
            }
        }
        else
        {
            state = NPCAttackState.Done;
        }
        
    }

    private Vector3? CalculatePointTomoveTo(Vector3 playerPosition)
    {
        float maxDistance = -1;
        for (int i = 1; i < skills.Count; i++)
        {
            if (CooldownDone(skills[i]) && skills[i].maxDistance > maxDistance)
            {
                maxDistance = skills[i].maxDistance;
            }

        }
        // found some actions that could be taken this turn
        // but is too far from the player
        if (maxDistance != -1)
        {
            float distanceToPlayer = Vector3.Distance(playerPosition, transform.position);
            if (distanceToPlayer > maxDistance)
            {
                float distToMove = distanceToPlayer - maxDistance;
                int cost = (int)(distToMove / Constants.DISTANCE_COST_UNIT);
                while (cost > actionPointsLeft)
                {
                    distToMove -= Constants.DISTANCE_COST_UNIT;
                    cost = (int)(distToMove / Constants.DISTANCE_COST_UNIT);
                }
                while (distanceToPlayer - distToMove < 2f)
                {
                    distToMove -= 0.05f;
                }
                float t = distToMove / distanceToPlayer;
                return Vector3.Lerp(transform.position, playerPosition, t);

            }
            

        }
        return null;
    }

    private bool CooldownDone(Skill skill)
    {
        if (skill.cooldown == 0)
        {
            return true;
        }
        else return false;
    }

    
    public void SetDoneCasting()
    {
        // deal damage to the enemy
        state = NPCAttackState.Done;
        if (selectedSkill.hasVisuals)
        {
            selectedSkill.RemoveEffect();
        }
        playerDied = DealDamageToPlayer();
    }
    public void SetDoneBuff()
    {
        // deal damage to the enemy
        playerDied = false;
        CombatManager.GetInstance().ApplyBuff(selectedSkill);
        state = NPCAttackState.Done;
        if (selectedSkill.hasVisuals)
        {
            selectedSkill.RemoveEffect();
        }

        // skills[actionIndex].cooldown = skills[actionIndex].maxCooldown;
        // actionPointsLeft -= skills[actionIndex].cost;
    }

    public bool DealDamageToPlayer()
    {
        int damageAfflicted = GetDamageDealt(selectedSkill);
        // returns true if character died
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player.GetDamaged(damageAfflicted))
        {
            // return true for death of a player
            return true;
        }
        else
        {
            return false;
        }
    }

    private Skill ChooseSkill()
    {
        if (actionPointsLeft == 0)
        {
            Debug.Log("Registered that there were no action points left");
            return null;
        }
        List<Skill> possibleSkills = new List<Skill>();
        Vector3 playerPosition = FindObjectOfType<PlayerCombat>().transform.position;
        int skillIndex = 0;
        foreach (var skill in skills)
        {
            if (skillIndex > 0)
            {
                AddSkillToPossible(ref possibleSkills, skill, playerPosition);
            }
            skillIndex++;
        }
        //Debug.Log("Actions taken: " + actionsTakenThisTurn + "; possibleSkills.Count: " + possibleSkills.Count 
        //    + "; AP left: " + actionPointsLeft);
        // skills where found
        if (possibleSkills.Count > 0)
        {
            int selectedSkillIndex = Random.Range(0, possibleSkills.Count - 1);
            Skill chosenSkill = possibleSkills[selectedSkillIndex];
            possibleSkills.Clear();
            return chosenSkill;
        }
        if (possibleSkills.Count == 0 && actionsTakenThisTurn == 0)
        {
            return skills[0]; // return move skill
        }
        possibleSkills.Clear();
        return null;

    }

    public void AddSkillToPossible(ref List<Skill> possibleSkills, Skill skill, Vector3 playerPosition)
    {
        //Debug.Log("Name: " + skill.skillName +
        //    "; cooldown: " + skill.cooldown +
        //    "; cost: " + skill.cost +
        //    "; AP left: " + actionPointsLeft +
        //    "; can afford cost: " + (skill.cost <= actionPointsLeft).ToString() +
        //    "; player is in distance: " + (Vector3.Distance(transform.position, playerPosition) <= skill.maxDistance).ToString() +
        //    "; distance to player: " + Vector3.Distance(transform.position, playerPosition));
        if (skill.spellType != Constants.SpellType.Special)
        {
            // skill is not in cooldown 
            if (skill.cooldown == 0)
            {
                // check if the target is too far
                if ((int)Vector3.Distance(transform.position, playerPosition) <= (int)skill.maxDistance)
                {
                    if (skill.cost <= actionPointsLeft)
                    {
                        possibleSkills.Add(skill);
                    }

                }
            }
        }
    }

}

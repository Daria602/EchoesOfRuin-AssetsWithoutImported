using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombat : CombatController
{
    public Skill moveSkill;
    public Skill basicAttackSkill;


    private Skill selectedSkill = null;

    public CombatManager combatManager;
    public bool isChoosingAttack = true;
    public bool donePerforming = false;

    private enum NPCAttackState
    {
        SelectingSkill,
        Attacking,
        Done

    }
    private NPCAttackState state = NPCAttackState.SelectingSkill;
    public bool NPCTurn()
    {
        Debug.Log(state);
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
                    state = NPCAttackState.Attacking;
                    Attack();
                }
                break;
            case NPCAttackState.Attacking:
                break;
            case NPCAttackState.Done:
                if (selectedSkill == null)
                {
                    Debug.Log("no more skills to do");
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

    public void Attack()
    {
        GetComponent<Animator>().SetTrigger("CastMagicAoE");
        selectedSkill.cooldown = selectedSkill.maxCooldown;
    }

    private bool playerDied = false;
    public void SetDoneCasting()
    {
        // deal damage to the enemy
        state = NPCAttackState.Done;
        playerDied = DealDamageToPlayer();
    }

    public bool DealDamageToPlayer()
    {
        int damageAfflicted = GetDamageDealt();
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

    public int GetDamageDealt()
    {
        return 1;
        //return skills[actionIndex].baseDamageMax;
    }

    private Skill ChooseSkill()
    {
        if (actionPointsLeft == 0)
        {
            return null;
        }
        List<Skill> possibleSkills = new List<Skill>();
        Vector3 playerPosition = FindObjectOfType<PlayerCombat>().transform.position;
        foreach (var skill in skills)
        {
            AddSkillToPossible(ref possibleSkills, skill, playerPosition);
        }

        // skills where found
        if (possibleSkills.Count > 0)
        {
            int selectedSkillIndex = Random.Range(0, possibleSkills.Count - 1);
            Skill chosenSkill = possibleSkills[selectedSkillIndex];
            possibleSkills.Clear();
            return chosenSkill;
            //foreach (var skill in skills)
            //{
            //    if (selectedSkill.name == skill.name)
            //    {
            //        skill.cooldown = skill.maxCooldown;
            //    }
            //}
        }
        possibleSkills.Clear();
        return null;

    }

    public void AddSkillToPossible(ref List<Skill> possibleSkills, Skill skill, Vector3 playerPosition)
    {
        if (skill.spellType != Constants.SpellType.Special)
        {
            // skill is not in cooldown 
            if (skill.cooldown == 0)
            {
                // check if the target is too far
                if (Vector3.Distance(transform.position, playerPosition) < skill.maxDistance)
                {
                    if (skill.cost <= actionPointsLeft)
                    {
                        possibleSkills.Add(skill);
                    }

                }
            }
        }
    }

    public void DoSomething()
    {
        if(isChoosingAttack)
        {
            Debug.Log("Was true");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            endedTurn = true;
        }
        if (isChoosingAttack)
        {
            Debug.Log("Choosing attack for " + gameObject.name);
            // do the animation and circle around
            ChooseSkill();
            if (selectedSkill != null)
            {
                isChoosingAttack = false;
                isPerformingAction = true;
                PerformAction(selectedSkill);
            }
            else
            {
                isChoosingAttack = true;
                endedTurn = true;
            }
            
            
        }
        else if (isPerformingAction)
        {
            //Debug.Log(skills[actionIndex].name);
            //Debug.Log("Performing the action");
            //doAction();
            if (donePerforming)
            {
                FindObjectOfType<PlayerHealth>().GetDamaged(CalculateAffectedDamage());
                //gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].prepareAnimationBoolName, false);
                //skills[actionIndex].RemoveEffect();
                skills[actionIndex].cooldown = skills[actionIndex].maxCooldown;
                selectedSkill.RemoveEffect();
                donePerforming = false;
                isPerformingAction = false;
                selectedSkill = null;
                isChoosingAttack = true;
                actionPointsLeft -= skills[actionIndex].cost;
            }

        }
    }

    private void PerformAction(Skill skill)
    {
        // Sets the visual effect around the character
        skill.SetEffect(gameObject);
        if (skill.hasAnimTrigger)
        {
            //Debug.Log("Got here");
            gameObject.GetComponent<Animator>().SetTrigger(skill.castTrigger);

        }
    }

    public bool finishedTurn = false;
   

    //private void ChooseSkill()
    //{
    //    if (actionPointsLeft == 0)
    //    {
    //        return;
    //    }
    //    List<Skill> possibleSkills = new List<Skill>();
    //    Vector3 playerPosition = FindObjectOfType<PlayerCombat>().transform.position;
    //    foreach (var skill in skills)
    //    {
            
    //        if (skill.spellType != Constants.SpellType.Special && skill.spellType != Constants.SpellType.Buff)
    //        {
    //            // skill is not in cooldown 
    //            if (skill.cooldown == 0)
    //            {   
    //                // check if the target is too far
    //                if (Vector3.Distance(transform.position, playerPosition) < skill.maxDistance)
    //                {
    //                    if (skill.cost <= actionPointsLeft)
    //                    {
    //                        possibleSkills.Add(skill);
    //                    }
                        
    //                }
    //            }
    //        }
    //        // add buff separately, because it is self cast and doesn't require distance
    //        else if (skill.spellType == Constants.SpellType.Buff)
    //        {
                
    //            if (skill.cooldown == 0)
    //            {
    //                // skill is still in cooldown
    //                possibleSkills.Add(skill);
    //            }
    //        }
    //    }

    //    // skills where found
    //    if (possibleSkills.Count > 0)
    //    {
    //        int selectedSkillIndex = Random.Range(0, possibleSkills.Count - 1);
    //        selectedSkill = possibleSkills[selectedSkillIndex];
    //        foreach (var skill in skills)
    //        {
    //            if (selectedSkill.name == skill.name)
    //            {
    //                skill.cooldown = skill.maxCooldown;
    //            }
    //        }
    //    }
    //    possibleSkills.Clear();
    //}

    private bool HasAvailableSkills()
    {
        return true;
    }

    //public void SetDoneCasting()
    //{
    //    donePerforming = true;
    //}
}

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
            Debug.Log("Performing the action");
            //doAction();
            if (donePerforming)
            {
                FindObjectOfType<PlayerHealth>().GetDamaged(CalculateAffectedDamage());
                //gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].prepareAnimationBoolName, false);
                //skills[actionIndex].RemoveEffect();
                selectedSkill.RemoveEffect();
                donePerforming = false;
                isPerformingAction = false;
                selectedSkill = null;
                isChoosingAttack = true;
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
   

    private void ChooseSkill()
    {
        List<Skill> possibleSkills = new List<Skill>();
        Vector3 playerPosition = FindObjectOfType<PlayerCombat>().transform.position;
        foreach (var skill in skills)
        {
            
            if (skill.spellType != Constants.SpellType.Special && skill.spellType != Constants.SpellType.Buff)
            {
                // skill is not in cooldown 
                if (skill.cooldown == 0)
                {   
                    // check if the target is too far
                    if (Vector3.Distance(transform.position, playerPosition) < skill.maxDistance)
                    {
                        possibleSkills.Add(skill);
                    }
                }
            }
            // add buff separately, because it is self cast and doesn't require distance
            else if (skill.spellType == Constants.SpellType.Buff)
            {
                
                if (skill.cooldown == 0)
                {
                    // skill is still in cooldown
                    possibleSkills.Add(skill);
                }
            }
        }

        // skills where found
        if (possibleSkills.Count > 0)
        {
            int selectedSkillIndex = Random.Range(0, possibleSkills.Count - 1);
            selectedSkill = possibleSkills[selectedSkillIndex];
            foreach (var skill in skills)
            {
                if (selectedSkill.name == skill.name)
                {
                    skill.cooldown = skill.maxCooldown;
                }
            }
        }
        possibleSkills.Clear();
    }

    private bool HasAvailableSkills()
    {
        return true;
    }

    public void SetDoneCasting()
    {
        donePerforming = true;
    }
}

using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{

    public int characterId;
    private bool m_isInCombat = false;
    public List<int> skillsIds;
    public bool hasWeapon = true;
    public int weaponId;
    public Transform rightHand;

    public bool isPerformingAction = false;
    public bool finishedPerforming = true;
    public int actionIndex = 0;
    public bool charactersTurn = false;
    //[HideInInspector()]
    public List<Skill> skills = new List<Skill>();
    private Weapon weapon = null;
    private GameObject weaponObject = null;

    

    protected delegate void DoAction();
    protected DoAction doAction;

    public bool endedTurn = true;
    public int actionPointsLeft = 5;
    public const int MAX_ACTION_POINTS = 5;

    public bool IsInCombat 
    { 
        get => m_isInCombat; 
        set => CombatStateIsChanged(value); 
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        Debug.Log("Combat controller thing");
        AddSkills();
        EquipWeapon();
    }

    protected void AddSkills()
    {

        skills.Clear();
        for (int i = 0; i < skillsIds.Count; i++)
        {
            Skill skillInstantiated = Instantiate(Constants.GetInstance().skillMap[skillsIds[i]]);
            skills.Add(skillInstantiated);
        }
    }

    protected void EquipWeapon()
    {
        if (hasWeapon)
        {
            weaponObject = Instantiate(Constants.GetInstance().weaponMap[weaponId].weaponPrefab, rightHand);
            weapon = Instantiate(Constants.GetInstance().weaponMap[weaponId], rightHand);
        }
    }

    


    protected void Move()
    {
        // Move gets a special treatment
        if (gameObject.GetComponent<BaseMovement>().IsAllowedToMove)
        {
            if (gameObject.GetComponent<BaseMovement>().HasArrived())
            {
                gameObject.GetComponent<BaseMovement>().IsAllowedToMove = false;
                isPerformingAction = false;
                gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, false);

            }

        }
        
    }

    protected void Attack()
    {
        if (finishedPerforming)
        {
            finishedPerforming = false;
            // Deal damage here
            Debug.Log("Got into Attack");
            isPerformingAction = false;
            gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, false);
            skills[actionIndex].RemoveEffect();

        }
    }

    private void CombatStateIsChanged(bool value)
    {
        if (value)
        {
            AddWeapon();
        }
        else
        {
            RemoveWeapon();
        }
        m_isInCombat = value;
        ResetMovement();
    }

    private void ResetMovement()
    {
        gameObject.GetComponent<BaseMovement>().IsAllowedToMove = !m_isInCombat;

    }

    protected int CalculateAffectedDamage()
    {
        float baseDamage = 0f;
        if (skills[actionIndex].requiresWeapon)
        {
            // TODO: calculate base on weapon
        }
        else
        {
            baseDamage = Random.Range((int)skills[actionIndex].baseDamageMin, (int)skills[actionIndex].baseDamageMax);
        }

        // TODO: apply stats here
        baseDamage = ApplyAttributes(baseDamage);
        return (int)baseDamage;
    }

    public void RemoveWeapon()
    {
        if (hasWeapon)
        {
            Destroy(weapon);
        }
        
    }

    public void AddWeapon()
    {
        if (hasWeapon)
        {
            weapon = Instantiate(Constants.GetInstance().weaponMap[weaponId], rightHand);
        }
    }

    private float ApplyAttributes(float baseDamage)
    {
        List<float> additions = new List<float>();
        for (int i = 0; i < skills[actionIndex].affectedByAttributes.Length; i++)
        {
            switch (skills[actionIndex].affectedByAttributes[i])
            {
                case Constants.AffectedByAttributes.Strength:
                    break;
                case Constants.AffectedByAttributes.Agility:
                    break;
                case Constants.AffectedByAttributes.Intelligence:
                    additions.Add(
                        baseDamage * gameObject.GetComponent<Stats>().intelligence * 0.05f
                        );
                    break;
                case Constants.AffectedByAttributes.Constitution:
                    break;
                case Constants.AffectedByAttributes.Wits:
                    break;
            }
        }

        foreach (float addition in additions)
        {
            baseDamage += addition;
        }

        return Mathf.Round(baseDamage);
    }

    //private void ApplyAbilities(out float damage)
    //{

    //}

    public void DecreaseCooldown(bool resetCooldowns)
    {
        if (resetCooldowns)
        {
            foreach (Skill skill in skills)
            {
                skill.cooldown = 0;
            }
        }
        else
        {
            foreach (Skill skill in skills)
            {
                skill.cooldown = (skill.cooldown - 1 < 0) ? 0 : skill.cooldown - 1;
            }
        }
        
    }


}

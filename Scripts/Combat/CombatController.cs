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
    public int actionsTakenThisTurn = 0;

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
        //Debug.Log("Combat controller thing");
        AddSkills();
        EquipWeapon();
    }

    public Weapon GetWeapon()
    {
        if (weapon != null)
        {
            return weapon;
        }
        return null;
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
                //gameObject.GetComponent<Animator>().SetBool(skills[actionIndex].affectAnimationBoolName, false);

            }

        }
        
    }

    public int GetDamageDealt(Skill skill)
    {
        bool isCrit = IsCriticalStrike();
        int minBaseDamage = 0;
        int maxBaseDamage = 0;
        if (skill.requiresWeapon)
        {
            if (weapon.weaponType == skill.weaponTypeRequired)
            {
                minBaseDamage = weapon.minDamage;
                maxBaseDamage = weapon.maxDamage;
            }
            else
            {
                Debug.LogError("Weapon is required for this skill, and the character doesn't have that weapon equipped");
            }
        }
        else
        {
            minBaseDamage = skill.baseDamageMin;
            maxBaseDamage = skill.baseDamageMax;
        }

        int chaosPoints = GetComponent<Stats>().abilities.chaos;
        minBaseDamage = (minBaseDamage - chaosPoints < 0) ? 0 : minBaseDamage - chaosPoints;
        maxBaseDamage = maxBaseDamage + chaosPoints;

        int attributeMultiplier;
        if (skill.skillName == "Basic Attack")
        {
            if (weapon.weaponType == Constants.WeaponTypes.Axe)
            {
                attributeMultiplier = GetAttributeValue(Constants.Attributes.Strength);
            }
            else if (weapon.weaponType == Constants.WeaponTypes.Bow)
            {
                attributeMultiplier = GetAttributeValue(Constants.Attributes.Agility);
            }
            else if (weapon.weaponType == Constants.WeaponTypes.Wand)
            {
                attributeMultiplier = GetAttributeValue(Constants.Attributes.Intelligence);
            }
            else
            {
                attributeMultiplier = 0;
            }
        }
        else
        {
            attributeMultiplier = GetAttributeValue(skill.affectedByAttribute);
        }

        float additionToTheDamage = attributeMultiplier * Constants.ATTRIBUTE_MULTIPLIER;
        minBaseDamage += (int)(minBaseDamage * additionToTheDamage);
        maxBaseDamage += (int)(maxBaseDamage * additionToTheDamage);

        int finalDamage = Random.Range(minBaseDamage, maxBaseDamage + 1);

        if (isCrit)
        {
            finalDamage *= 2;
        }
        return finalDamage;
    }

    private int GetAttributeValue(Constants.Attributes attribute)
    {
        Stats stats = GetComponent<Stats>();
        switch (attribute)
        {
            case Constants.Attributes.Strength:
                return stats.attributes.strength;
            case Constants.Attributes.Agility:
                return stats.attributes.agility;
            case Constants.Attributes.Intelligence:
                return stats.attributes.intelligence;
            case Constants.Attributes.Constitution:
                return stats.attributes.constitution;
            case Constants.Attributes.Wits:
                return stats.attributes.wits;
        }
        return -1000;
    }



    private bool IsCriticalStrike()
    {
        // crit chance starts from 10% and with each point in Shadow will increase with 5%
        float baseCritStrikeStat = GetComponent<Stats>().critStrike;
        int shadowPoints = GetComponent<Stats>().abilities.shadow;
        float shadowAddition = shadowPoints * Constants.SHADOW_MULTIPLIER; // 0.05
        float finalCritChance = baseCritStrikeStat + shadowAddition;
        // 30% = 0.3
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber <= finalCritChance)
        {
            return true;
        }
        return false;
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
        //baseDamage = ApplyAttributes(baseDamage);
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

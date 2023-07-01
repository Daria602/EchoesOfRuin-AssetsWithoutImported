using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Create New Skill")]
public class Skill : ScriptableObject
{
    [Header("Unique identifier")]
    public int skillId;
    [Header("Details")]
    public string skillName;
    public Sprite skillIcon;
    public string description;
    [Header("Special effects")]
    public bool hasVisuals = false;
    public GameObject visualEffectPrefab = null;
    private GameObject visualEffect = null;
    [Header("Animations")]
    public bool hasAnimTrigger = true;
    public string castTrigger;
    [Header("Max Cooldown, Damage and Max Distance")]
    public int cooldown = 0;
    public int maxCooldown = 4;
    public int baseDamageMin;
    public int baseDamageMax;
    public float maxDistance = 10;
    [Header("Weapon realted")]
    public bool requiresWeapon = false;
    public Constants.WeaponTypes weaponTypeRequired;

    public Constants.SpellType spellType;
    public Constants.Attributes affectedByAttribute;
    //public Constants.SkillBelongsTo skillBelongsTo;
    [Header("Action points required")]
    public int cost;
    [Header("Buff related")]
    public int buffsForTurns;
    public Constants.Attributes[] attributes;
    public Constants.Abilities[] abilities;

    


    //public Constants.Spell spell;
    
    // TODO: afflict enemies with debuffs


    //public Constants.Attributes[] affectedByAttributes;
    //public Constants.AffectedByAbilities[] affectedByAbilities;

    private void Awake()
    {
        //switch (spellType)
        //{
        //    case Constants.SpellType.Special:
        //        cost = 0;
        //        break;
        //    case Constants.SpellType.Buff:
        //        cost = 1;
        //        break;
        //    case Constants.SpellType.IncreasedDamage:
        //        cost = 2;
        //        break;
        //    case Constants.SpellType.AoE:
        //        cost = 3;
        //        break;
        //}
    }
    public void SetEffect(GameObject character)
    {
        if (hasVisuals)
        {
            if (visualEffect == null)
            {
                visualEffect = Instantiate(visualEffectPrefab, character.transform);
            }
        }
    }

    public void RemoveEffect()
    {
        if (visualEffect != null)
        {
            Destroy(visualEffect);
            
        }
    }

    public string GetTooltip()
    {
        /*
        * Name +
        * Base damage +
        * Range +
        * Affected by +
        * Description +
        */
        string skillCost = this.cost.ToString();
        string tooltipString;
        string name = skillName;
        string description;
        if (skillName == "Basic Attack")
        {
            description = "Attack with your ";
            Weapon weapon = FindObjectOfType<PlayerCombat>().GetWeapon();
            if (weapon == null)
            {
                description += "hands";
            }
            else
            {
                switch (weapon.weaponType)
                {
                    case Constants.WeaponTypes.Axe:
                        description += "axe";
                        break;
                    case Constants.WeaponTypes.Bow:
                        description += "bow";
                        break;
                    case Constants.WeaponTypes.Wand:
                        description += "wand";
                        break;
                }
            }
        }
        else
        {
            description = this.description;
        }
        
        int[] minMaxDamage = GetDamageCalculated();
        string affectedBy = affectedByAttribute.ToString();
        string range = maxDistance.ToString();
        tooltipString =
            "~" + name + " $" + skillCost + "AP\n" +
            "$Damage: " + minMaxDamage[0] + " - " + minMaxDamage[1] + "`\n" +
            "*Range: " + range + "u`\n" +
           ( name != "Move" ? "@This skill is affected by " + affectedBy + ".`\n" : "") +
            description;
        return tooltipString;
    }

    private int[] GetDamageCalculated()
    {
        int[] minMaxDamage = { 0, 0 };
        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        Weapon weapon = player.GetComponent<PlayerCombat>().GetWeapon();
        if (requiresWeapon && weapon != null)
        {
           if (weapon.weaponType == weaponTypeRequired)
           {
                minMaxDamage[0] = weapon.minDamage;
                minMaxDamage[1] = weapon.maxDamage;
           }
           else
           {
                minMaxDamage[0] = -1;
                minMaxDamage[1] = -1;
                return minMaxDamage;
           }
        }
        else
        {
            minMaxDamage[0] = baseDamageMin;
            minMaxDamage[1] = baseDamageMax;
        }
        // Apply chaos points
        int chaosPoints = player.GetComponent<Stats>().abilities.chaos;
        minMaxDamage[0] = (minMaxDamage[0] - chaosPoints < 0) ? 0 : minMaxDamage[0] - chaosPoints;
        minMaxDamage[1] = minMaxDamage[1] + chaosPoints;

        // Apply attributes
        int attributeMultiplier;
        if (skillName == "Basic Attack")
        {
            if (weapon == null)
            {
                attributeMultiplier = 0;
            }
            else
            {
                if (weapon.weaponType == Constants.WeaponTypes.Axe)
                {
                    attributeMultiplier = player.GetComponent<PlayerCombat>().GetAttributeValue(Constants.Attributes.Strength);
                }
                else if (weapon.weaponType == Constants.WeaponTypes.Bow)
                {
                    attributeMultiplier = player.GetComponent<PlayerCombat>().GetAttributeValue(Constants.Attributes.Agility);
                }
                else if (weapon.weaponType == Constants.WeaponTypes.Wand)
                {
                    attributeMultiplier = player.GetComponent<PlayerCombat>().GetAttributeValue(Constants.Attributes.Intelligence);
                }
                else
                {
                    attributeMultiplier = 0;
                }
            }
            
        }
        else
        {
            attributeMultiplier = player.GetComponent<PlayerCombat>().GetAttributeValue(affectedByAttribute);
        }
        float additionToTheDamage = attributeMultiplier * Constants.ATTRIBUTE_MULTIPLIER;
        minMaxDamage[0] += Mathf.RoundToInt(minMaxDamage[0] * additionToTheDamage);
        minMaxDamage[1] += Mathf.RoundToInt(minMaxDamage[1] * additionToTheDamage);

        return minMaxDamage;
    }

}

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

    public void Something()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Create New Skill")]
public class Skill : ScriptableObject
{
    public int skillId;
    public string skillName;
    public Sprite skillIcon;
    public Constants.SkillBelongsTo skillBelongsTo;
    
    public bool hasVisuals = false;
    public GameObject visualEffectPrefab = null;
    private GameObject visualEffect = null;
    public string prepareAnimationBoolName = "";
    public string affectAnimationBoolName = "";
    public bool hasAnimTrigger = true;
    public string castTrigger;
    public float maxDistance = 10;
    public string description;
    public Constants.SpellType spellType;

    public bool isInCooldown = false;

    public int cooldown = 0;
    public int maxCooldown = 4;

    public int cost;

    public int baseDamageMin;
    public int baseDamageMax;


    public Constants.Spell spell;
    public bool requiresWeapon = false;
    public Constants.WeaponTypes weaponTypeRequired;
    // TODO: afflict enemies with debuffs


    public Constants.Attributes[] affectedByAttributes;
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

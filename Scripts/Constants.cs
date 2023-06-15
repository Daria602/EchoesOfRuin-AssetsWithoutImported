using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour 
{
    private static Constants instance;
    public const int PLAYER_ID = 0;

    public const float DISTANCE_COST_UNIT = 1.5f;

    public int[] skillIds;
    public Skill[] allSkills;
    public Dictionary<int, Skill> skillMap = new Dictionary<int, Skill>();


    public enum SpellType
    {
        Special,
        Buff,
        IncreasedDamage,
        AoE
    }

    public enum Spell
    {
        Magic,
        Melee,
        Bow
        
    }

    public enum WeaponTypes
    {
        NONE, Axe, Bow, Wand
    }

    public enum AffectedByAttributes
    {
        Strength,
        Agility,
        Intelligence,
        Constitution,
        Wits
    }

    public enum AffectedByAbilities
    {
        Melee,
        Ranged,
        Fire,
        Air,
        Earth,
        Water
    }

    public static Constants GetInstance()
    {
        return instance;
    }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many CombatManager instances. It's a singleton class");
        }
        instance = this;

        if (skillIds.Length != allSkills.Length)
        {
            Debug.LogError("skill id's should have the same number of elements as all skills array");
        }
        else
        {
            for (int i = 0; i < skillIds.Length; i++)
            {
                skillMap.Add(skillIds[i], allSkills[i]);
            }
        }
    }
}

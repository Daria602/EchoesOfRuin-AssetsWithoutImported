using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour, ILoadingData 
{
    private static Constants instance;
    public const int PLAYER_ID = 0;
    public const float DISTANCE_COST_UNIT = 1.5f;

    public int[] skillIds;
    public Skill[] allSkills;
    public Dictionary<int, Skill> skillMap = new Dictionary<int, Skill>();

    public int[] weaponIds;
    public Weapon[] allWeapons;
    public Dictionary<int, Weapon> weaponMap = new Dictionary<int, Weapon>();

    public int[] itemsStillPresent;
    public List<GameObject> itemsGOStillPresent;

    public int[] questIds;
    public Quest[] quests;
    public Dictionary<int, Quest> questMap = new Dictionary<int, Quest>();

    public enum SkillBelongsTo
    {
        Axe,
        Bow,
        Fire,
        Water,
        Earth,
        Air,
        Other
    }
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

    public enum QuestType
    {
        Kill,
        Explore,
        Talk
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

        if (weaponIds.Length != allWeapons.Length)
        {
            Debug.LogError("weapon id's should have the same number of elements as all weapons array");
        }
        else
        {
            for (int i = 0; i < weaponIds.Length; i++)
            {
                weaponMap.Add(weaponIds[i], allWeapons[i]);
            }
        }

        if (questIds.Length != quests.Length)
        {
            Debug.LogError("weapon id's should have the same number of elements as all weapons array");
        }
        else
        {
            for (int i = 0; i < questIds.Length; i++)
            {
                questMap.Add(questIds[i], quests[i]);
            }
        }
    }

    public void LoadGameData(CharacterData characterData)
    {
        if (characterData.itemsStillPresent == null)
        {
            return;
        }
        else
        {
            itemsStillPresent = characterData.itemsStillPresent;
        }
    }

    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.itemsStillPresent = itemsStillPresent;
        Debug.Log(characterData.itemsStillPresent.Length);
    }
}

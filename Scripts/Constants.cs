using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour, ILoadingData 
{
    private static Constants instance;
    public static Constants GetInstance()
    {
        return instance;
    }
    public const int PLAYER_ID = 0;
    public const float DISTANCE_COST_UNIT = 1.5f;
    public const float ATTRIBUTE_MULTIPLIER = 0.5f;
    public const float SHADOW_MULTIPLIER = 0.05f;
    public const int MAX_ACTION_POINTS = 5;

    public int[] characterIdKeys;
    public GameObject[] characterObjectValues;
    public Dictionary<int, GameObject> characters = new Dictionary<int, GameObject>();
    public List<int> charactersDead = new List<int>();

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

    //public string[] combatTextVariants =
    //{

    //};

    private string[] combatText =
    {
        "You will pay for this!",
        "Argh!!!",
        "I will end you!",
        "Your luck's run out.",
        "No escape from defeat.",
        "Feeble attempts won't save you.",
        "Can't touch me, pathetic.",
        "Is that your best? Disappointing.",
        "Outmatched and outplayed.",
        "Your failure is meaningless.",
        "Prepare for the end.",
        "Just an annoyance in my path.",
        "Surrender while you can."
    };

    public string GetRandomCombatText()
    {
        int randomIndex = Random.Range(0, combatText.Length);
        return combatText[randomIndex];
    }

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
        Attack
    }

    public enum Spell
    {
        Magic,
        Axe,
        Bow
        
    }

    public enum WeaponTypes
    {
        NONE, Axe, Bow, Wand
    }

    public enum Attributes
    {
        Strength,
        Agility,
        Intelligence,
        Constitution,
        Wits
    }
    public enum Abilities
    {
        Shadow,
        Chaos,
        Luck,
        Charisma
    }

    //public enum AffectedByAbilities
    //{
    //    Melee,
    //    Ranged,
    //    Fire,
    //    Air,
    //    Earth,
    //    Water
    //}

    public enum QuestType
    {
        Kill,
        Explore,
        Talk
    }
    public enum ClickType
    {
        NONE,
        UI,
        Move,
        Interact,
        Self
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

        if (characterIdKeys.Length != characterObjectValues.Length)
        {
            Debug.LogError("Keys and values of character id's dictionary have different size");
        }
        else
        {
            for (int i = 0; i < characterIdKeys.Length; i++)
            {
                characters.Add(characterIdKeys[i], characterObjectValues[i]);
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

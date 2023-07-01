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
    public const int HEALTH_PER_CONSTITUTION = 10;

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
    public GameObject goldPrefab;

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

    static Vector3[] RogerMovement()
    {
        return new Vector3[] {
        new Vector3(164.3f, 1.2f, 338.9f),
        new Vector3(170.2f, 1.6f, 337.5f),
        new Vector3(167.3f, 1.5f, 342.4f),
        new Vector3(163.7f, 1.5f, 342.4f)
        };
    }

    static Vector3[] LenaMovement()
    {
        return new Vector3[] {
            new Vector3(137.5f, 1.3f, 341.1f),
            new Vector3(140.3f, 1.2f, 338.2f),
            new Vector3(142.5f, 1.1f, 332.6f),
            new Vector3(144.5f, 1.0f, 325.5f),
            new Vector3(141.0f, 1.3f, 320.7f),
            new Vector3(143.9f, 1.1f, 328.6f),
            new Vector3(141.7f, 1.4f, 334.8f),
            new Vector3(137.0f, 1.3f, 341.2f)
        };
    }
    static Vector3[] SebastianMovement()
    {
        return new Vector3[]
        {
            new Vector3(155.6f, 0.6f, 361.9f),
            new Vector3(161.6f, 0.9f, 364.9f),
            new Vector3(160.1f, 0.7f, 359.3f)
        };
    }

    static Vector3[] AmeliaMovement()
    {
        return new Vector3[]
        {
            new Vector3(179.0f, 0.6f, 365.4f),
            new Vector3(169.7f, 1.4f, 362.7f),
            new Vector3(167.8f, 0.8f, 356.0f),
            new Vector3(177.2f, 0.5f, 359.9f)
        };
    }

    static Vector3[] NinaMovement()
    {
        return new Vector3[]
        {
            new Vector3(181.0f, 1.3f, 300.1f),
            new Vector3(174.2f, 1.4f, 301.4f),
            new Vector3(172.5f, 1.3f, 308.1f),
            new Vector3(179.2f, 1.0f, 311.5f),
            new Vector3(182.4f, 1.1f, 303.1f)
        };
    }

    static Vector3[] GrimsonMovement()
    {
        return new Vector3[]
        {
            new Vector3(179.7f, 1.1f, 338.7f),
            new Vector3(171.9f, 1.3f, 346.9f),
            new Vector3(159.9f, 0.9f, 351.1f),
            new Vector3(178.7f, 1.0f, 336.3f)
        };
    }

    static Vector3[] JoshuaMovement()
    {
        return new Vector3[]
        {
            new Vector3(110.1f, 3.4f, 326.0f),
            new Vector3(121.2f, 1.9f, 325.0f),
            new Vector3(115.6f, 3.1f, 320.8f),
            new Vector3(117.6f, 2.4f, 329.8f)
        };
    }
    static Vector3[] RavendirMovement()
    {
        return new Vector3[]
        {
            new Vector3(55.4f, 2.6f, 317.0f),
            new Vector3(56.5f, 3.1f, 319.2f),
            new Vector3(53.2f, 2.1f, 315.0f)
        };
    }

    static Vector3[] DravenwoodMovement()
    {
        return new Vector3[]
        {
            new Vector3(33.5f, 1.6f, 251.1f),
            new Vector3(39.0f, 1.4f, 251.7f),
            new Vector3(36.1f, 1.4f, 255.3f)
        };
    }
    static Vector3[] ValorienMovement()
    {
        return new Vector3[]
        {
            new Vector3(24.8f, 1.3f, 246.1f),
            new Vector3(28.7f, 1.4f, 250.5f),
            new Vector3(22.6f, 1.2f, 250.9f)
        };
    }

    static Vector3[] SeraphinaMovement()
    {
        return new Vector3[]
        {
            new Vector3(25.4f, 1.3f, 272.0f),
            new Vector3(27.4f, 1.3f, 267.2f),
            new Vector3(29.9f, 1.3f, 268.9f)
        };
    }

    static Vector3[] LysandraMovement()
    {
        return new Vector3[]
        {
            new Vector3(199.6f, 2.4f, 199.5f),
            new Vector3(204.7f, 2.6f, 194.3f),
            new Vector3(207.2f, 2.6f, 198.9f)
        };
    }

    static Vector3[] AstridMovement()
    {
        return new Vector3[]
        {
            new Vector3(206.8f, 3.4f, 180.9f),
            new Vector3(211.7f, 3.4f, 181.6f),
            new Vector3(210.7f, 3.5f, 186.4f)
        };
    }

    static Vector3[] TharlicMovement()
    {
        return new Vector3[]
        {
            new Vector3(205.1f, 2.9f, 189.6f),
            new Vector3(209.5f, 3.4f, 192.2f)
        };
    }

    static Vector3[] EldricMovement()
    {
        return new Vector3[]
        {
            new Vector3(298.9f, 2.3f, 125.5f),
            new Vector3(291.3f, 2.4f, 123.2f),
            new Vector3(292.4f, 2.0f, 127.0f)
        };
    }

    static Vector3[] AlistarMovement()
    {
        return new Vector3[]
        {
            new Vector3(299.4f, 1.5f, 105.2f),
            new Vector3(308.4f, 2.3f, 101.6f),
            new Vector3(308.1f, 2.6f, 107.7f)
        };
    }

    static Vector3[] MagnusMovement()
    {
        return new Vector3[]
        {
            new Vector3(324.6f, 3.2f, 95.3f),
            new Vector3(321.8f, 3.6f, 99.4f),
            new Vector3(320.2f, 2.3f, 90.4f)
        };
    }

    static Vector3[] GarrickMovement()
    {
        return new Vector3[]
        {
            new Vector3(321.8f, 0.6f, 74.5f),
            new Vector3(313.9f, 0.7f, 77.4f),
            new Vector3(321.5f, 1.1f, 79.8f)
        };
    }

    static Vector3[] AureliaMovement()
    {
        return new Vector3[]
        {
            new Vector3(395.1f, 6.4f, 228.1f),
            new Vector3(391.6f, 6.4f, 229.0f),
            new Vector3(391.7f, 6.8f, 224.7f)
        };
    }

    static Vector3[] FinneganMovement()
    {
        return new Vector3[]
        {
            new Vector3(390.5f, 6.9f, 216.2f),
            new Vector3(388.3f, 6.6f, 221.4f),
            new Vector3(386.1f, 6.3f, 218.5f)
        };
    }

    static Vector3[] LuciusMovement()
    {
        return new Vector3[]
        {
            new Vector3(384.0f, 6.6f, 227.4f),
            new Vector3(381.6f, 6.7f, 228.3f),
            new Vector3(381.5f, 6.3f, 224.9f)
        };
    }



    public static Vector3[] GetNPCMovement(int characterId)
    {
        switch (characterId)
        {
            case 0:
                // Case 0
                return null;
            case 1:
                // Case 1
                return RogerMovement();
            case 2:
                // Case 2
                return LenaMovement();
            case 3:
                // Case 3
                return SebastianMovement();
            case 4:
                // Case 4
                return AmeliaMovement();
            case 5:
                // Case 5
                return NinaMovement();
            case 6:
                // Case 6
                return GrimsonMovement();
            case 7:
                // Case 7
                return JoshuaMovement();
            case 8:
                // Case 8
                return RavendirMovement();
            case 9:
                // Case 9
                return DravenwoodMovement();
            case 10:
                // Case 10
                return ValorienMovement();
            case 11:
                // Case 11
                return SeraphinaMovement();
            case 12:
                // Case 9
                return LysandraMovement();
            case 13:
                // Case 10
                return AstridMovement();
            case 14:
                // Case 11
                return TharlicMovement();
            case 15:
                // Case 11
                return EldricMovement();
            case 16:
                // Case 9
                return AlistarMovement();
            case 17:
                // Case 10
                return MagnusMovement();
            case 18:
                // Case 11
                return GarrickMovement();
            case 19:
                // Case 9
                return AureliaMovement();
            case 20:
                // Case 10
                return FinneganMovement();
            case 21:
                // Case 11
                return LuciusMovement();
            default:
                // Default case
                //Console.WriteLine("Invalid characterId");
                break;
        }
        return new Vector3[] { Vector3.zero };
    }



}

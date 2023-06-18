using UnityEngine;

[System.Serializable]
public class CharacterData
{
    // Appearance related
    public bool isMale;
    public bool darkSkin;
    public int currentHeadIndex;
    public int currentHairIndex;
    public int currentHairColor;

    // Attributes
    public int availableAttributePoints;
    public int strength;
    public int agility;
    public int intelligence;
    public int constitution;
    public int wits;

    // Abilities
    public int availableAbilityPoints;
    public int shadow;
    public int chaos;
    public int luck;
    public int charisma;
    // Character position
    public Vector3 characterPosition;
    // Character combat state
    public int characterId;
    public bool m_isInCombat;
    public int[] skillsIds;
    public bool hasWeapon;
    public int weaponId;
    //public Transform rightHand;
    public bool isPerformingAction;
    public bool finishedPerforming;
    public int actionIndex;
    public bool charactersTurn;
    public bool IsInCombat;

    // Character health

    public int currentHealth;
    public int healthModifier;
    public int maxHealth;
    public int m_currentMaxHealth;

    // Creating initial values
    public CharacterData()
    {
        this.isMale = true;
        this.darkSkin = false;
        this.currentHeadIndex = 0;
        this.currentHairIndex = 0;
        this.currentHairColor = 0;

        // Attributes
        this.availableAttributePoints = 5;
        this.strength = 10;
        this.agility = 10;
        this.intelligence = 10;
        this.constitution = 10;
        this.wits = 10;

        // Abilities
        this.availableAbilityPoints = 3;
        this.shadow = 0;
        this.chaos = 0;
        this.luck = 0;
        this.charisma = 0;

        // Character position
        this.characterPosition = Vector3.zero;

        // Character combat state
        this.IsInCombat = false;
        this.characterId = 0;
        this.m_isInCombat = false;
        this.skillsIds = null;
        this.hasWeapon = false;
        this.weaponId = 0;
        //this.rightHand = Vector3.zero;
        this.isPerformingAction = false;
        this.finishedPerforming = true;
        this.actionIndex = 0;
        this.charactersTurn = false;

        this.currentHealth = 0;
        this.healthModifier = 0;
        this.maxHealth = 10;
        this.m_currentMaxHealth = 10;
    }

}

using System.Collections.Generic;
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
    public List<int> skillsIds;
    public bool hasWeapon;
    public int weaponId;
    //public Transform rightHand;
    public bool isPerformingAction;
    public bool finishedPerforming;
    public int actionIndex;
    public bool charactersTurn;
    public bool IsInCombat;
    public int characterLevel;

    // Character health

    public int currentHealth;
    //public int healthModifier;
    //public int maxHealth;
    //public int m_currentMaxHealth;

    // Items present in the world visually
    public int[] itemsStillPresent;

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
        this.strength = 0; 
        this.agility = 0;
        this.intelligence = 0;
        this.constitution = 10;
        this.wits = 0;

        // Abilities
        this.availableAbilityPoints = 5;
        this.shadow = 0;
        this.chaos = 0;
        this.luck = 0;
        this.charisma = 0;

        // Character position  Vector3(183.199997,2.16000009,305.399994)
        // this.characterPosition = Vector3.zero;
        this.characterPosition = new Vector3(183.199997f, 2.16000009f, 305.399994f);
        // Character combat state
        this.IsInCombat = false;
        this.characterId = 0;
        this.m_isInCombat = false;
        this.skillsIds = new List<int>() { 12, 13, 0, 2, 4, 7, 10, 15, 19 }; // { 12, 13, 6, 5 };
        this.hasWeapon = false; 
        this.weaponId = -1; // TODO: change here to 0
        //this.rightHand = Vector3.zero;
        this.isPerformingAction = false;
        this.finishedPerforming = true;
        this.actionIndex = 0;
        this.charactersTurn = false;

        this.currentHealth = -1;
        //this.healthModifier = 0;
        //this.maxHealth = 10;
       // this.m_currentMaxHealth = 10;

        //Character level & xp
        this.characterLevel = 1;

        // Items still present
        this.itemsStillPresent = null;
    }

}

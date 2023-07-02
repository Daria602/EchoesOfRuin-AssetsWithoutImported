using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // For combat
    private int initiative = 1;

    [System.Serializable]
    public class Attributes
    {
        public int strength;
        int tempStrength;
        public int agility;
        int tempAgility;
        public int intelligence;
        int tempIntelligence;
        public int constitution;
        int tempConstitution;
        public int wits;
        int tempWits;
        public void SetAttributes(int strength, int agility, int intelligence, int constitution, int wits)
        {
            this.strength = strength;
            tempStrength = strength;
            this.agility = agility;
            tempAgility = agility;
            this.intelligence = intelligence;
            tempIntelligence = intelligence;
            this.constitution = constitution;
            tempConstitution = constitution;
            this.wits = wits;
            tempWits = wits;
        }
        public void TempModifyAttribute(Constants.Attributes attribute, bool increase)
        {
            int amount = increase ? 1 : -1;
            switch (attribute)
            {
                case Constants.Attributes.Strength:
                    if (this.strength + amount >= 0)
                    {
                        this.strength += amount;
                    }
                    break;
                case Constants.Attributes.Agility:
                    if (this.agility + amount >= 0)
                    {
                        this.agility += amount;
                    }
                    break;
                case Constants.Attributes.Intelligence:
                    if (this.intelligence + amount >= 0)
                    {
                        this.intelligence += amount;
                    }
                    break;
                case Constants.Attributes.Constitution:
                    if (this.constitution + amount >= 0)
                    {
                        this.constitution += amount;
                        FindObjectOfType<PlayerHealth>().UpdateCurrentHealth(increase);
                        FindObjectOfType<PlayerHealth>().UpdateHealthUI();
                    }
                    break;
                case Constants.Attributes.Wits:
                    if (this.wits + amount >= 0)
                    {
                        this.wits += amount;
                    }
                    break;
            }
        }

        public void ResetTempIncrease(Constants.Attributes attribute)
        {
            switch (attribute)
            {
                case Constants.Attributes.Strength:
                    this.strength = tempStrength;
                    break;
                case Constants.Attributes.Agility:
                    this.agility = tempAgility;
                    break;
                case Constants.Attributes.Intelligence:
                    this.intelligence = tempIntelligence;
                    break;
                case Constants.Attributes.Constitution:
                    this.constitution = tempConstitution;
                    break;
                case Constants.Attributes.Wits:
                    this.wits = tempWits;
                    break;
            }
        }

        public bool PermModifyAttribute(Constants.Attributes attribute, bool increase)
        {
            int amount = increase ? 1 : -1;
            bool wasModified = false;
            switch (attribute)
            {
                case Constants.Attributes.Strength:
                    if (this.strength + amount >= 0)
                    {
                        this.strength += amount;
                        tempStrength = this.strength;
                        wasModified = true;
                    }
                    break;
                case Constants.Attributes.Agility:
                    if (this.agility + amount >= 0)
                    {
                        this.agility += amount;
                        tempAgility = this.agility;
                        wasModified = true;
                    }
                    break;
                case Constants.Attributes.Intelligence:
                    if (this.intelligence + amount >= 0)
                    {
                        this.intelligence += amount;
                        tempIntelligence = this.intelligence;
                        wasModified = true;
                    }
                    break;
                case Constants.Attributes.Constitution:
                    if (this.constitution + amount >= 0)
                    {
                        this.constitution += amount;
                        tempConstitution = this.constitution;
                        
                        wasModified = true;
                    }
                    break;
                case Constants.Attributes.Wits:
                    if (this.wits + amount >= 0)
                    {
                        this.wits += amount;
                        tempWits = this.wits;
                        wasModified = true;
                    }
                    break;
            }
            return wasModified;
        }
    }
    public Attributes attributes = new Attributes();

    [System.Serializable]
    public class Abilities
    {
        public int shadow;
        int tempShadow;
        public int chaos;
        int tempChaos;
        public int luck;
        int tempLuck;
        public int charisma;
        int tempCharisma;
        public void SetAbilities(int shadow, int chaos, int luck, int charisma)
        {
            this.shadow = shadow;
            tempShadow = shadow;
            this.chaos = chaos;
            tempChaos = chaos;
            this.luck = luck;
            tempLuck = luck;
            this.charisma = charisma;
            tempCharisma = charisma;
        }
        public void TempModifyAbility(Constants.Abilities ability, bool increase)
        {
            int amount = increase ? 1 : -1;
            switch (ability)
            {
                case Constants.Abilities.Shadow:
                    if (this.shadow + amount >= 0)
                    {
                        this.shadow += amount;
                    }
                    break;
                case Constants.Abilities.Chaos:
                    if (this.chaos + amount >= 0)
                    {
                        this.chaos += amount;
                    }
                    break;
                case Constants.Abilities.Luck:
                    if (this.luck + amount >= 0)
                    {
                        this.luck += amount;
                    }
                    break;
                case Constants.Abilities.Charisma:
                    if (this.charisma + amount >= 0)
                    {
                        this.charisma += amount;
                    }
                    break;
            }
        }
        public bool PermModifyAttribute(Constants.Abilities ability, bool increase)
        {
            int amount = increase ? 1 : -1;
            bool wasModified = false;
            switch (ability)
            {
                case Constants.Abilities.Shadow:
                    if (this.shadow + amount >= 0)
                    {
                        this.shadow += amount;
                        tempShadow = this.shadow;
                        wasModified = true;
                    }
                    break;
                case Constants.Abilities.Chaos:
                    if (this.chaos + amount >= 0)
                    {
                        this.chaos += amount;
                        tempChaos = this.chaos;
                        wasModified = true;
                    }
                    break;
                case Constants.Abilities.Luck:
                    if (this.luck + amount >= 0)
                    {
                        this.luck += amount;
                        tempLuck = this.luck;
                        wasModified = true;
                    }
                    break;
                case Constants.Abilities.Charisma:
                    if (this.charisma + amount >= 0)
                    {
                        this.charisma += amount;
                        tempCharisma = this.charisma;
                        wasModified = true;
                    }
                    break;
            }
            return wasModified;
        }

    }
    public Abilities abilities = new Abilities();

    private void Start()
    {
        //attributes.SetAttributes(strength, agility, intelligence, constitution, wits);
    }
    // Attributes
    public enum AttributeStats
    {
        Strength,
        Agility,
        Intelligence,
        Constitution,
        Wits
    }
    public int availableAttributePoints = 5;
    //public int strength = 0;
    //public int agility = 0;
    //public int intelligence = 0;
    //public int constitution = 0;
    //public int wits = 0;

    private const int MIN_ATTRIBUTE_VALUE = 0;

    public int characterLevel = 1;
    public float critStrike = 0.3f;
    public int grantedXP = 1000;
    // Combat abilities
    public enum AbilityStats
    {
        Shadow,
        Chaos,
        Luck,
        Charisma
    }
    public int availableAbilityPoints = 5;
    //public int shadow = 0;
    //public int chaos = 0;
    //public int luck = 0;
    //public int charisma = 0;

    private const int MIN_ABILITY_VALUE = 0;

    public bool IncreaseAttributeStat(int statType)
    {
        if (availableAttributePoints > 0)
        {
            if (attributes.PermModifyAttribute((Constants.Attributes)statType, true))
            {
                availableAttributePoints--;
                if (statType == (int)Constants.Attributes.Constitution)
                {
                    FindObjectOfType<PlayerHealth>().UpdateCurrentHealth(true);
                    FindObjectOfType<PlayerHealth>().UpdateHealthUI();
                }
                return true;
            }
          
        }
        return false;
    }

    public bool SimplyIncreaseAttributeStat(int statType)
    {
        if (availableAttributePoints > 0)
        {
            if (attributes.PermModifyAttribute((Constants.Attributes)statType, true))
            {
                availableAttributePoints--;
                return true;
            }

        }
        return false;
    }

    public bool DecreaseAttributeStat(int statType)
    {
        if (attributes.PermModifyAttribute((Constants.Attributes)statType, false))
        {
            availableAttributePoints++;
            if (statType == (int)Constants.Attributes.Constitution)
            {
                FindObjectOfType<PlayerHealth>().UpdateCurrentHealth(false);
                FindObjectOfType<PlayerHealth>().UpdateHealthUI();
            }
            
            return true;
        }
        return false;
    }
    public bool SimplyDecreaseAttributeStat(int statType)
    {
        if (attributes.PermModifyAttribute((Constants.Attributes)statType, false))
        {
            availableAttributePoints++;
            return true;
        }
        return false;
    }

    public bool IncreaseAbilityStat(int statType)
    {
        if (availableAbilityPoints > 0)
        {
            if (abilities.PermModifyAttribute((Constants.Abilities)statType, true))
            {
                availableAbilityPoints--;
                return true;
            }
        }
        return false;
    }

    public bool DecreaseAbilityStat(int statType)
    {
        if (abilities.PermModifyAttribute((Constants.Abilities)statType, false))
        {
            availableAbilityPoints++;
            return true;
        }
        return false;
    }

    // Special function for future modifications to this, maybe there is a ceiling to how much player can increase a stat
    private bool Increase(ref int stat, ref int availablePoints)
    {
        if (availablePoints > 0)
        {
            stat = stat + 1;
            availablePoints--;
            return true;
        }
        return false;
        
    }

    private bool Decrease(ref int stat,ref int availablePoints, int minValue)
    {
        if (stat - 1 >= minValue)
        {
            stat--;
            availablePoints++;
            return true;
        }
        return false;
    }

    public void LoadGameData(CharacterData characterData)
    {
        this.availableAttributePoints = characterData.availableAttributePoints;
        this.attributes.SetAttributes
        (
            characterData.strength,
            characterData.agility,
            characterData.intelligence,
            characterData.constitution,
            characterData.wits
        );

        this.availableAbilityPoints = characterData.availableAbilityPoints;
        this.abilities.SetAbilities
        (
            characterData.shadow,
            characterData.chaos,
            characterData.luck,
            characterData.charisma
        );
        this.characterLevel = characterData.characterLevel;
    }
    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.availableAttributePoints = this.availableAttributePoints;
        characterData.strength = this.attributes.strength;
        characterData.agility = this.attributes.agility;
        characterData.intelligence = this.attributes.intelligence;
        characterData.constitution = this.attributes.constitution;
        characterData.wits = this.attributes.wits;

        characterData.availableAbilityPoints = this.availableAbilityPoints;
        characterData.shadow = this.abilities.shadow;
        characterData.chaos = this.abilities.chaos;
        characterData.luck= this.abilities.luck;
        characterData.charisma = this.abilities.charisma;
        characterData.characterLevel = this.characterLevel;
    }

    public int GetInitiative()
    {
        return initiative + attributes.wits;
    }
}

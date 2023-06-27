using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // For combat
    public int initiative = 1;

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
    public float criticalStrike = 0.3f;
    public int grantedXP = 1000;
    // Combat abilities
    public enum AbilityStats
    {
        Shadow,
        Chaos,
        Luck,
        Charisma
    }
    public int availableAbilityPoints = 3;
    public int shadow = 0;
    public int chaos = 0;
    public int luck = 0;
    public int charisma = 0;

    private const int MIN_ABILITY_VALUE = 0;

    public bool IncreaseAttributeStat(int statType)
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
            return true;
        }
        return false;
    }

    public bool IncreaseAbilityStat(int statType)
    {
        bool result = false;
        switch (statType)
        {
            case (int)AbilityStats.Shadow:
                result = Increase(ref shadow, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.Chaos:
                result = Increase(ref chaos, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.Luck:
                result = Increase(ref luck, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.Charisma:
                result = Increase(ref charisma, ref availableAbilityPoints);
                break;
        }
        return result;
    }

    public bool DecreaseAbilityStat(int statType)
    {
        bool result = false;
        switch (statType)
        {
            case (int)AbilityStats.Shadow:
                result = Decrease(ref shadow, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.Chaos:
                result = Decrease(ref chaos, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.Luck:
                result = Decrease(ref luck, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.Charisma:
                result = Decrease(ref charisma, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
        }
        return result;
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
        this.shadow = characterData.shadow;
        this.chaos = characterData.chaos;
        this.luck = characterData.luck;
        this.charisma = characterData.charisma;
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
        characterData.shadow = this.shadow;
        characterData.chaos = this.chaos;
        characterData.luck= this.luck;
        characterData.charisma = this.charisma;
        characterData.characterLevel = this.characterLevel;
    }

    public int GetInitiative()
    {
        return initiative + attributes.wits;
    }

    public void ApplyBuff(Constants.Attributes[] attributes, Constants.Abilities[] abilities)
    {

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // For combat
    public int initiative = 1;

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
    public int strength = 0;
    public int agility = 0;
    public int intelligence = 0;
    public int constitution = 0;
    public int wits = 0;

    private const int MIN_ATTRIBUTE_VALUE = 0;

    public int characterLevel = 1;
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
        bool result = false;
        switch (statType)
        {
            case (int)AttributeStats.Strength:
                result = Increase(ref strength, ref availableAttributePoints);
                break;
            case (int)AttributeStats.Agility:
                result = Increase(ref agility, ref availableAttributePoints);
                break;
            case (int)AttributeStats.Intelligence:
                result = Increase(ref intelligence, ref availableAttributePoints);
                break;
            case (int)AttributeStats.Constitution:
                result = Increase(ref constitution, ref availableAttributePoints);
                break;
            case (int)AttributeStats.Wits:
                result = Increase(ref wits, ref availableAttributePoints);
                break;
        }
        return result;
    }

    public bool DecreaseAttributeStat(int statType)
    {
        bool result = false;
        switch (statType)
        {
            case (int)AttributeStats.Strength:
                result = Decrease(ref strength, ref availableAttributePoints, MIN_ATTRIBUTE_VALUE);
                break;
            case (int)AttributeStats.Agility:
                result = Decrease(ref agility, ref availableAttributePoints, MIN_ATTRIBUTE_VALUE);
                break;
            case (int)AttributeStats.Intelligence:
                result = Decrease(ref intelligence, ref availableAttributePoints, MIN_ATTRIBUTE_VALUE);
                break;
            case (int)AttributeStats.Constitution:
                result = Decrease(ref constitution, ref availableAttributePoints, MIN_ATTRIBUTE_VALUE);
                break;
            case (int)AttributeStats.Wits:
                result = Decrease(ref wits, ref availableAttributePoints, MIN_ATTRIBUTE_VALUE);
                break;
        }
        return result;
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
        this.strength = characterData.strength;
        this.agility = characterData.agility;
        this.intelligence = characterData.intelligence;
        this.constitution = characterData.constitution;
        this.wits = characterData.wits;
        this.characterLevel = characterData.characterLevel;

        this.availableAbilityPoints = characterData.availableAbilityPoints;
        this.shadow = characterData.shadow;
        this.chaos = characterData.chaos;
        this.luck = characterData.luck;
        this.charisma = characterData.charisma;
    }
    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.availableAttributePoints = this.availableAttributePoints;
        characterData.strength = this.strength;
        characterData.agility = this.agility;
        characterData.intelligence = this.intelligence;
        characterData.constitution = this.constitution;
        characterData.wits = this.wits;

        characterData.availableAbilityPoints = this.availableAbilityPoints;
        characterData.shadow = this.shadow;
        characterData.chaos = this.chaos;
        characterData.luck= this.luck;
        characterData.charisma = this.charisma;
        characterData.characterLevel = this.characterLevel;
    }

    public int GetInitiative()
    {
        return initiative + wits;
    }



}

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

    // Combat abilities
    public enum AbilityStats
    {
        OneHanded,
        TwoHanded,
        DualWielding,
        Ranged,
        FireAffinity,
        AirAffinity,
        WaterAffinity,
        EarthAffinity
    }
    public int availableAbilityPoints = 3;
    public int oneHanded = 0;
    public int twoHanded = 0;
    public int dualWielding = 0;
    public int ranged = 0;
    public int fireAffinity = 0;
    public int airAffinity = 0;
    public int waterAffinity = 0;
    public int earthAffinity = 0;

    private const int MIN_ABILITY_VALUE = 0;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            case (int)AbilityStats.OneHanded:
                result = Increase(ref oneHanded, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.TwoHanded:
                result = Increase(ref twoHanded, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.DualWielding:
                result = Increase(ref dualWielding, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.Ranged:
                result = Increase(ref ranged, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.FireAffinity:
                result = Increase(ref fireAffinity, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.AirAffinity:
                result = Increase(ref airAffinity, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.WaterAffinity:
                result = Increase(ref waterAffinity, ref availableAbilityPoints);
                break;
            case (int)AbilityStats.EarthAffinity:
                result = Increase(ref earthAffinity, ref availableAbilityPoints);
                break;
        }
        return result;
    }

    public bool DecreaseAbilityStat(int statType)
    {
        bool result = false;
        switch (statType)
        {
            case (int)AbilityStats.OneHanded:
                result = Decrease(ref oneHanded, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.TwoHanded:
                result = Decrease(ref twoHanded, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.DualWielding:
                result = Decrease(ref dualWielding, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.Ranged:
                result = Decrease(ref ranged, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.FireAffinity:
                result = Decrease(ref fireAffinity, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.AirAffinity:
                result = Decrease(ref airAffinity, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.WaterAffinity:
                result = Decrease(ref waterAffinity, ref availableAbilityPoints, MIN_ABILITY_VALUE);
                break;
            case (int)AbilityStats.EarthAffinity:
                result = Decrease(ref earthAffinity, ref availableAbilityPoints, MIN_ABILITY_VALUE);
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

        this.availableAbilityPoints = characterData.availableAbilityPoints;
        this.oneHanded = characterData.oneHanded;
        this.twoHanded = characterData.twoHanded;
        this.dualWielding = characterData.dualWielding;
        this.ranged = characterData.ranged;
        this.fireAffinity = characterData.fireAffinity;
        this.airAffinity = characterData.airAffinity;
        this.waterAffinity = characterData.waterAffinity;
        this.earthAffinity = characterData.earthAffinity;
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
        characterData.oneHanded = this.oneHanded;
        characterData.twoHanded = this.twoHanded;
        characterData.dualWielding= this.dualWielding;
        characterData.ranged = this.ranged;
        characterData.fireAffinity = this.fireAffinity;
        characterData.airAffinity = this.airAffinity;
        characterData.waterAffinity = this.waterAffinity;
        characterData.earthAffinity = this.earthAffinity;
    }


    public int GetInitiative()
    {
        return initiative + wits;
    }



}

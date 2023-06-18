using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInGame : MonoBehaviour, ILoadingData
{
    // Appearance
    public bool male = true;
    public GameObject femaleCharacter;
    public GameObject maleCharacter;

    // Attributes
    public enum AttributeStats
    {
        Strength,
        Agility,
        Intelligence,
        Constitution,
        Wits
    }

    public TMP_Text availablePointsAttribute;
    public TMP_Text strength;
    public TMP_Text agility;
    public TMP_Text intelligence;
    public TMP_Text constitution;
    public TMP_Text wits;

    // Abilities
    public enum AbilityStats
    {
        Shadow,
        Chaos,
        Luck,
        Charisma
    }
    public TMP_Text availablePointsAbility;
    // TODO: change this to melee
    public TMP_Text shadow;
    public TMP_Text chaos;
    public TMP_Text luck;
    public TMP_Text charisma;


    // Attributes
    public void IncreaseStat(int statType)
    {
        if (male)
        {
            if (maleCharacter.GetComponent<PlayerStats>().IncreaseAttributeStat(statType))
            {
                UpdateAttributesUI(statType, true);
            }
        }
        else
        {
            if (femaleCharacter.GetComponent<PlayerStats>().IncreaseAttributeStat(statType))
            {
                UpdateAttributesUI(statType, true);
            }
        }
    }

    public void DecreaseStat(int statType)
    {
        if (male)
        {
            if (maleCharacter.GetComponent<PlayerStats>().DecreaseAttributeStat(statType))
            {
                UpdateAttributesUI(statType, false);
            }
        }
        else
        {
            if (femaleCharacter.GetComponent<PlayerStats>().DecreaseAttributeStat(statType))
            {
                UpdateAttributesUI(statType, false);
            }

        }
    }

    private void UpdateAttributesUI(int statType, bool increase)
    {
        int amount = increase ? 1 : -1;
        int currentNumber;
        switch (statType)
        {
            case (int)AttributeStats.Strength:
                currentNumber = int.Parse(strength.text) + amount;
                strength.text = currentNumber.ToString();
                break;
            case (int)AttributeStats.Agility:
                currentNumber = int.Parse(agility.text) + amount;
                agility.text = currentNumber.ToString();
                break;
            case (int)AttributeStats.Intelligence:
                currentNumber = int.Parse(intelligence.text) + amount;
                intelligence.text = currentNumber.ToString();
                break;
            case (int)AttributeStats.Constitution:
                currentNumber = int.Parse(constitution.text) + amount;
                constitution.text = currentNumber.ToString();
                break;
            case (int)AttributeStats.Wits:
                currentNumber = int.Parse(wits.text) + amount;
                wits.text = currentNumber.ToString();
                break;
        }
        int pointsLeft = int.Parse(availablePointsAttribute.text) - amount;
        availablePointsAttribute.text = pointsLeft.ToString();
    }

    public void IncreaseAbility(int statType)
    {
        if (male)
        {
            if (maleCharacter.GetComponent<PlayerStats>().IncreaseAbilityStat(statType))
            {
                UpdateAbilitiesUI(statType, true);
            }
        }
        else
        {
            if (femaleCharacter.GetComponent<PlayerStats>().IncreaseAbilityStat(statType))
            {
                UpdateAbilitiesUI(statType, true);
            }
        }
    }

    public void DecreaseAbility(int statType)
    {
        if (male)
        {
            if (maleCharacter.GetComponent<PlayerStats>().DecreaseAbilityStat(statType))
            {
                UpdateAbilitiesUI(statType, false);
            }
        }
        else
        {
            if (femaleCharacter.GetComponent<PlayerStats>().DecreaseAbilityStat(statType))
            {
                UpdateAbilitiesUI(statType, false);
            }

        }
    }

    private void UpdateAbilitiesUI(int statType, bool increase)
    {
        int amount = increase ? 1 : -1;
        int currentNumber;
        switch (statType)
        {
            case (int)AbilityStats.Shadow:
                currentNumber = int.Parse(shadow.text) + amount;
                shadow.text = currentNumber.ToString();
                break;
            case (int)AbilityStats.Chaos:
                currentNumber = int.Parse(chaos.text) + amount;
                chaos.text = currentNumber.ToString();
                break;
            case (int)AbilityStats.Luck:
                currentNumber = int.Parse(luck.text) + amount;
                luck.text = currentNumber.ToString();
                break;
            case (int)AbilityStats.Charisma:
                currentNumber = int.Parse(charisma.text) + amount;
                charisma.text = currentNumber.ToString();
                break;
        }
        int pointsLeft = int.Parse(availablePointsAbility.text) - amount;
        availablePointsAbility.text = pointsLeft.ToString();
    }

    public void SetCorrectValues()
    {
        if (male)
        {
            SetAbilityValues(ref maleCharacter);
        }
        else
        {
            SetAbilityValues(ref femaleCharacter);
        }
    }

    public void SetAbilityValues(ref GameObject player)
    {
        strength.text = player.GetComponent<PlayerStats>().strength.ToString();
        agility.text = player.GetComponent<PlayerStats>().agility.ToString();
        intelligence.text = player.GetComponent<PlayerStats>().intelligence.ToString();
        constitution.text = player.GetComponent<PlayerStats>().constitution.ToString();
        wits.text = player.GetComponent<PlayerStats>().wits.ToString();

        shadow.text = player.GetComponent<PlayerStats>().shadow.ToString();
        chaos.text = player.GetComponent<PlayerStats>().chaos.ToString();
        luck.text = player.GetComponent<PlayerStats>().luck.ToString();
        charisma.text = player.GetComponent<PlayerStats>().charisma.ToString();

        availablePointsAbility.text = player.GetComponent<PlayerStats>().availableAbilityPoints.ToString();
        availablePointsAttribute.text = player.GetComponent<PlayerStats>().availableAttributePoints.ToString();
    }

    public void LoadGameData(CharacterData characterData)
    {
        try
        {
            this.male = characterData.isMale;
            if (male)
            {
                maleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
                maleCharacter.GetComponent<Stats>().LoadGameData(characterData);
                SetAbilityValues(ref maleCharacter);
           }
           else
            {
                femaleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
                femaleCharacter.GetComponent<Stats>().LoadGameData(characterData);
                SetAbilityValues(ref femaleCharacter);
            }
        } catch (NullReferenceException ex)
        {
           
        }

    }

    public void SaveGameData(ref CharacterData characterData)
    {
        Debug.Log("Is char data null: " + characterData == null);
        characterData.isMale = this.male;
        if (male)
        {
            maleCharacter.GetComponent<CharacterCreation>().SaveGameData(ref characterData);
            maleCharacter.GetComponent<Stats>().SaveGameData(ref characterData);
        }
        else
        {
            femaleCharacter.GetComponent<CharacterCreation>().SaveGameData(ref characterData);
            femaleCharacter.GetComponent<Stats>().SaveGameData(ref characterData);
        }

    }
}

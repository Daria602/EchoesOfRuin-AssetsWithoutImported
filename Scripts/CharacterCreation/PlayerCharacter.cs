using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, ILoadingData
{
    // Appearance
    public bool male = true;
    public GameObject femaleCharacter;
    public GameObject maleCharacter;
    public TMP_Text textShowingGender;
    public TMP_Text headType;
    public TMP_Text hairType;
    public TMP_Text hairColor;
    public TMP_Text skinColor;

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
    public TMP_Text shadow;
    public TMP_Text chaos;
    public TMP_Text luck;
    public TMP_Text charisma;

    public bool isInChreationScene;



    private void Start()
    {
        UpdateAppearanceUI();
    }


    // Appearance
    public void Next(int groupType) // group types are described in CharacterCreation script
    {
        if (male)
        {
            maleCharacter.GetComponent<CharacterCreation>().Next(groupType);
        } 
        else
        {
            femaleCharacter.GetComponent<CharacterCreation>().Next(groupType);
        }
        UpdateAppearanceUI();
    }

    public void Previous(int groupType)
    {
        if (male)
        {
            maleCharacter.GetComponent<CharacterCreation>().Previous(groupType);
        }
        else
        {
            femaleCharacter.GetComponent<CharacterCreation>().Previous(groupType);
        }
        UpdateAppearanceUI();
    }

    public void ToggleSkinColor()
    {
        if (male)
        {
            maleCharacter.GetComponent<CharacterCreation>().ToggleSkinColor();
        }
        else
        {
            femaleCharacter.GetComponent<CharacterCreation>().ToggleSkinColor();
        }
    }

    public void ToggleGender()
    {
        male = !male;
        femaleCharacter.SetActive(!male);
        maleCharacter.SetActive(male);
        UpdateAppearanceUI();
        SetCorrectValues();

        textShowingGender.text = male ? "Male" : "Female";
    }

    private void UpdateAppearanceUI()
    {
        string[] typeNames;
        if (male)
        {
            typeNames = maleCharacter
                .GetComponent<CharacterCreation>()
                .GetAllCurrentTypeNames();
            
        }
        else
        {
            typeNames = femaleCharacter
                .GetComponent<CharacterCreation>()
                .GetAllCurrentTypeNames();
        }
        if (typeNames != null && typeNames.Length > 0)
        {
            ApplyAppearanceUIData(typeNames);
        }
    }

    private void ApplyAppearanceUIData(string[] typeNames)
    {
        headType.text = typeNames[0];
        hairType.text = typeNames[1];
        hairColor.text = typeNames[2];
        skinColor.text = typeNames[3];
    }
    public void ResetCharacter()
    {
        if (male)
        {
            maleCharacter.GetComponent<CharacterCreation>()
                .ResetCharacter();
        }
        else
        {
            femaleCharacter.GetComponent<CharacterCreation>()
                .ResetCharacter();
        }
        UpdateAppearanceUI();
    }


    // Attributes
    public void IncreaseStat(int statType)
    {
        if (male)
        {
            if (maleCharacter.GetComponent<Stats>().IncreaseAttributeStat(statType))
            {
                UpdateAttributesUI(statType, true);
            }
        }
        else
        {
            if (femaleCharacter.GetComponent<Stats>().IncreaseAttributeStat(statType))
            {
                UpdateAttributesUI(statType, true);
            }
        }
    }

    public void DecreaseStat(int statType)
    {
        if (male)
        {
            if (maleCharacter.GetComponent<Stats>().DecreaseAttributeStat(statType))
            {
                UpdateAttributesUI(statType, false);
            }
        }
        else
        {
            if (femaleCharacter.GetComponent<Stats>().DecreaseAttributeStat(statType))
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
            if (maleCharacter.GetComponent<Stats>().IncreaseAbilityStat(statType))
            {
                UpdateAbilitiesUI(statType, true);
            }
        }
        else
        {
            if (femaleCharacter.GetComponent<Stats>().IncreaseAbilityStat(statType))
            {
                UpdateAbilitiesUI(statType, true);
            }
        }
    }

    public void DecreaseAbility(int statType)
    {
        if (male)
        {
            if (maleCharacter.GetComponent<Stats>().DecreaseAbilityStat(statType))
            {
                UpdateAbilitiesUI(statType, false);
            }
        }
        else
        {
            if (femaleCharacter.GetComponent<Stats>().DecreaseAbilityStat(statType))
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
        strength.text = player.GetComponent<Stats>().strength.ToString();
        agility.text = player.GetComponent<Stats>().agility.ToString();
        intelligence.text = player.GetComponent<Stats>().intelligence.ToString();
        constitution.text = player.GetComponent<Stats>().constitution.ToString();
        wits.text = player.GetComponent<Stats>().wits.ToString();

        shadow.text = player.GetComponent<Stats>().shadow.ToString();
        chaos.text = player.GetComponent<Stats>().chaos.ToString();
        luck.text = player.GetComponent<Stats>().luck.ToString();
        charisma.text = player.GetComponent<Stats>().charisma.ToString();

        availablePointsAbility.text = player.GetComponent<Stats>().availableAbilityPoints.ToString();
        availablePointsAttribute.text = player.GetComponent<Stats>().availableAttributePoints.ToString();
    }

    public void LoadGameData(CharacterData characterData)
    {
        if (isInChreationScene)
        {
            this.male = true;
            CharacterData cd = new CharacterData();
            maleCharacter.GetComponent<CharacterCreation>().LoadGameData(cd);
            maleCharacter.GetComponent<Stats>().LoadGameData(cd);
            femaleCharacter.GetComponent<CharacterCreation>().LoadGameData(cd);
            femaleCharacter.GetComponent<Stats>().LoadGameData(cd);
        }
        else
        {
            this.male = characterData.isMale;
            if (male)
            {
                maleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
            }
            else
            {
                femaleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
            }
        }
        
    }

    public void SaveGameData(ref CharacterData characterData)
    {
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

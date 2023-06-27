using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    private static StatsUI instance;
    public static StatsUI GetInstance() { return instance; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many StatsUI instances");
        }
        instance = this;
    }
    public GameObject player;
    // Attributes
    public TMP_Text availablePointsAttribute;
    public TMP_Text strength;
    public TMP_Text agility;
    public TMP_Text intelligence;
    public TMP_Text constitution;
    public TMP_Text wits;

    // Abilities
    public TMP_Text availablePointsAbility;
    public TMP_Text shadow;
    public TMP_Text chaos;
    public TMP_Text luck;
    public TMP_Text charisma;


    // Attributes
    public void IncreaseStat(int statType)
    {
        if (player.GetComponent<Stats>().IncreaseAttributeStat(statType))
        {
            UpdateAttributesUI(statType, true);
        }
    }

    public void DecreaseStat(int statType)
    {
        if (player.GetComponent<Stats>().DecreaseAttributeStat(statType))
        {
            UpdateAttributesUI(statType, false);
        }
    }

    private void UpdateAttributesUI(int statType, bool increase)
    {
        int amount = increase ? 1 : -1;
        int currentNumber;
        switch (statType)
        {
            case (int)Constants.Attributes.Strength:
                currentNumber = int.Parse(strength.text) + amount;
                strength.text = currentNumber.ToString();
                break;
            case (int)Constants.Attributes.Agility:
                currentNumber = int.Parse(agility.text) + amount;
                agility.text = currentNumber.ToString();
                break;
            case (int)Constants.Attributes.Intelligence:
                currentNumber = int.Parse(intelligence.text) + amount;
                intelligence.text = currentNumber.ToString();
                break;
            case (int)Constants.Attributes.Constitution:
                currentNumber = int.Parse(constitution.text) + amount;
                constitution.text = currentNumber.ToString();
                break;
            case (int)Constants.Attributes.Wits:
                currentNumber = int.Parse(wits.text) + amount;
                wits.text = currentNumber.ToString();
                break;
        }
        int pointsLeft = int.Parse(availablePointsAttribute.text) - amount;
        availablePointsAttribute.text = pointsLeft.ToString();
    }

    public void IncreaseAbility(int statType)
    {
        if (player.GetComponent<Stats>().IncreaseAbilityStat(statType))
        {
            UpdateAbilitiesUI(statType, true);
        }
    }

    public void DecreaseAbility(int statType)
    {
        if (player.GetComponent<Stats>().DecreaseAbilityStat(statType))
        {
            UpdateAbilitiesUI(statType, false);
        }
    }

    private void UpdateAbilitiesUI(int statType, bool increase)
    {
        int amount = increase ? 1 : -1;
        int currentNumber;
        switch (statType)
        {
            case (int)Constants.Abilities.Shadow:
                currentNumber = int.Parse(shadow.text) + amount;
                shadow.text = currentNumber.ToString();
                break;
            case (int)Constants.Abilities.Chaos:
                currentNumber = int.Parse(chaos.text) + amount;
                chaos.text = currentNumber.ToString();
                break;
            case (int)Constants.Abilities.Luck:
                currentNumber = int.Parse(luck.text) + amount;
                luck.text = currentNumber.ToString();
                break;
            case (int)Constants.Abilities.Charisma:
                currentNumber = int.Parse(charisma.text) + amount;
                charisma.text = currentNumber.ToString();
                break;
        }
        int pointsLeft = int.Parse(availablePointsAbility.text) - amount;
        availablePointsAbility.text = pointsLeft.ToString();
    }

    public void SetAbilityValues()
    {
        strength.text = player.GetComponent<Stats>().attributes.strength.ToString();
        agility.text = player.GetComponent<Stats>().attributes.agility.ToString();
        intelligence.text = player.GetComponent<Stats>().attributes.intelligence.ToString();
        constitution.text = player.GetComponent<Stats>().attributes.constitution.ToString();
        wits.text = player.GetComponent<Stats>().attributes.wits.ToString();

        shadow.text = player.GetComponent<Stats>().shadow.ToString();
        chaos.text = player.GetComponent<Stats>().chaos.ToString();
        luck.text = player.GetComponent<Stats>().luck.ToString();
        charisma.text = player.GetComponent<Stats>().charisma.ToString();

        availablePointsAbility.text = player.GetComponent<Stats>().availableAbilityPoints.ToString();
        availablePointsAttribute.text = player.GetComponent<Stats>().availableAttributePoints.ToString();
    }

    private void Start()
    {
        //player = FindObjectOfType<PlayerController>().gameObject;
        //if (player == null)
        //{
        //    Debug.Log("Something went wrong here");
        //}
        SetAbilityValues();
    }

    //public void LoadGameData(CharacterData characterData)
    //{
    //    //player.GetComponent<CharacterCreation>
    //    //try
    //    //{
    //    //    this.male = characterData.isMale;
    //    //    if (male)
    //    //    {
    //    //        maleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
    //    //        maleCharacter.GetComponent<Stats>().LoadGameData(characterData);
    //    //        SetAbilityValues(ref maleCharacter);
    //    //   }
    //    //   else
    //    //    {
    //    //        femaleCharacter.GetComponent<CharacterCreation>().LoadGameData(characterData);
    //    //        femaleCharacter.GetComponent<Stats>().LoadGameData(characterData);
    //    //        SetAbilityValues(ref femaleCharacter);
    //    //    }
    //    //} catch (NullReferenceException)
    //    //{

    //    //}
    //    //player.GetComponent<Stats>().LoadGameData(characterData);

    //    //SetAbilityValues();

    //}

    //public void SaveGameData(ref CharacterData characterData)
    //{
    //    //player.GetComponent<CharacterCreation>().SaveGameData(ref characterData);
    //    player.GetComponent<Stats>().SaveGameData(ref characterData);


    //}
}

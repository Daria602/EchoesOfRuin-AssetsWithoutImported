using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public bool GetDamaged(int damage)
    {
        
        if (currentHealth - damage <= 0)
        {
            return true;
        }
        else
        {
            currentHealth -= damage;

            UIManager.GetInstance().UpdateHealthSlider(currentHealth, 0, CurrentMaxHealth);
            UIManager.GetInstance().UpdateHealthText(currentHealth.ToString());
            return false;
        }
        
    }

    public void UpdateCurrentHealth(bool increased)
    {
        if (increased)
        {
            int currentMaxHealth = CurrentMaxHealth;
            int previousMaxHealth = currentMaxHealth - Constants.HEALTH_PER_CONSTITUTION;
            float currentHealthPercent = (float)currentHealth / (float)previousMaxHealth;
            int newCurrentHealth = Mathf.RoundToInt(currentMaxHealth * currentHealthPercent);
            if (newCurrentHealth > currentMaxHealth)
            {
                currentHealth = currentMaxHealth;
            }
            else
            {
                currentHealth = newCurrentHealth;
            }
            Debug.Log("Increased max health; Current max health: " + currentMaxHealth +
                "; Previous max health: " + previousMaxHealth +
                "; currentHealthPercent: " + currentHealthPercent.ToString() +
                "; new current health: " + newCurrentHealth +
                "; current health is bigger than max health: " + (newCurrentHealth > currentMaxHealth).ToString());

        }
        else
        {
            int currentMaxHealth = CurrentMaxHealth;
            int previousMaxHealth = currentMaxHealth + Constants.HEALTH_PER_CONSTITUTION;
            float currentHealthPercent = (float)currentHealth / (float)previousMaxHealth;
            int newCurrentHealth = Mathf.RoundToInt(currentMaxHealth * currentHealthPercent);
            if (newCurrentHealth > currentMaxHealth)
            {
                currentHealth = currentMaxHealth;
            }
            else
            {
                currentHealth = newCurrentHealth;
            }
            Debug.Log("Decreased max health; Current max health: " + currentMaxHealth +
                "; Previous max health: " + previousMaxHealth +
                "; currentHealthPercent: " + currentHealthPercent.ToString() +
                "; new current health: " + newCurrentHealth +
                "; current health is bigger than max health: " + (newCurrentHealth > currentMaxHealth).ToString());

        }
        //GetMaxHealth();
    }

    public void UpdateHealthUI()
    {
        if (currentHealth > CurrentMaxHealth)
        {
            currentHealth = CurrentMaxHealth;
        }
        UIManager.GetInstance().UpdateHealthSlider(currentHealth, 0, CurrentMaxHealth);
        UIManager.GetInstance().UpdateHealthText(currentHealth.ToString());
    }

    public void LoadGameData(CharacterData characterData)
    {
        this.currentHealth = characterData.currentHealth;
        //this.healthModifier = characterData.healthModifier;
        //this.maxHealth = characterData.maxHealth;
        //this.m_currentMaxHealth = characterData.m_currentMaxHealth;
    }
    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.currentHealth = this.currentHealth;
        //characterData.healthModifier = this.healthModifier;
        //characterData.maxHealth = this.maxHealth;
        //characterData.m_currentMaxHealth = this.m_currentMaxHealth;
    }

}

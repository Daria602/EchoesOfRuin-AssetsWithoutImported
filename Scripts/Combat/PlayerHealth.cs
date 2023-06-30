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

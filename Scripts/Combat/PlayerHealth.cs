using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public bool GetDamaged(int damage)
    {
        
        if (currentHealth - damage <= 0)
        {
            if (OnDeathCallback != null)
            {
                OnDeathCallback.Invoke();
            }
            Die();
            return false;
        }
        else
        {
            currentHealth -= damage;
            if (OnHealthUpdateCallback != null)
            {
                OnHealthUpdateCallback.Invoke();
            }
            return true;
        }
        
    }
    public override void Die()
    {
        base.Die();
        CombatManager.GetInstance().SignalDeath(gameObject.GetComponent<CombatController>().characterId);
        gameObject.SetActive(false);
        Debug.Log("Player dies");
    }
    //public void Die()
    //{
    //    Debug.Log(gameObject.name + "dies");
    //}

    public void LoadGameData(CharacterData characterData)
    {
        this.currentHealth = characterData.currentHealth;
        this.healthModifier = characterData.healthModifier;
        this.maxHealth = characterData.maxHealth;
        this.m_currentMaxHealth = characterData.m_currentMaxHealth;
    }
    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.currentHealth = this.currentHealth;
        characterData.healthModifier = this.healthModifier;
        characterData.maxHealth = this.maxHealth;
        characterData.m_currentMaxHealth = this.m_currentMaxHealth;
    }

}

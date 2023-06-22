using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int healthModifier;
    public int maxHealth;
    public int m_currentMaxHealth;
    public delegate void UpdateHealth();
    public UpdateHealth OnHealthUpdateCallback;
    public delegate void OnDeath();
    public OnDeath OnDeathCallback;
    private void Awake()
    {
        healthModifier = 0;
        m_currentMaxHealth = maxHealth;
        currentHealth = m_currentMaxHealth;
    }

    public int CurrentMaxHealth {  get => m_currentMaxHealth; set { ModifyCurrentMaxHealth(value); } }

    private void ModifyCurrentMaxHealth(int value)
    {
        healthModifier = value;
        m_currentMaxHealth += m_currentMaxHealth * value;

    }

    public void Heal(int amount)
    {
        if (currentHealth + amount > m_currentMaxHealth)
        {
            currentHealth = m_currentMaxHealth;
        }
        else
        {
            currentHealth += amount;
        }
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + "dies");
    }

}

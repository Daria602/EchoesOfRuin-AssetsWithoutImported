using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float healthModifier;
    public float maxHealth;
    protected float m_currentMaxHealth;

    private void Awake()
    {
        healthModifier = 0;
        m_currentMaxHealth = maxHealth;
        currentHealth = m_currentMaxHealth;
    }

    public float CurrentMaxHealth {  get => m_currentMaxHealth; set { ModifyCurrentMaxHealth(value); } }

    private void ModifyCurrentMaxHealth(float value)
    {
        healthModifier = value;
        m_currentMaxHealth += m_currentMaxHealth * value;

    }

    public void Heal(float amount)
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

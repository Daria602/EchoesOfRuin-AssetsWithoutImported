using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth;
    //public int healthModifier;
    public int baseMaxHealth = 10;
    public delegate void UpdateHealth();
    public UpdateHealth OnHealthUpdateCallback;
    public delegate void OnDeath();
    public OnDeath OnDeathCallback;
    private void Awake()
    {
        //healthModifier = 0;
        //m_currentMaxHealth = maxHealth;
        //currentHealth = m_currentMaxHealth;
    }
    private void Start()
    {
        if (currentHealth < 0)
        {
            currentHealth = CurrentMaxHealth;
        }
    }

    public int CurrentMaxHealth {  get => GetMaxHealth(); }

    protected int GetMaxHealth()
    {
        int constitutionValue = GetComponent<Stats>().attributes.constitution;
        int constitutionAddition = Constants.HEALTH_PER_CONSTITUTION * constitutionValue;
        //Debug.Log("Got to update health and it is: " + (baseMaxHealth + constitutionAddition));
        return baseMaxHealth + constitutionAddition;
    }
    //private void ModifyCurrentMaxHealth(int value)
    //{
    //    healthModifier = value;
    //    m_currentMaxHealth += m_currentMaxHealth * value;

    //}

    //public void Heal(int amount)
    //{
    //    if (currentHealth + amount > m_currentMaxHealth)
    //    {
    //        currentHealth = m_currentMaxHealth;
    //    }
    //    else
    //    {
    //        currentHealth += amount;
    //    }
    //}

    public void PotionHeal(int amount)
    {
        float percent = amount * 0.01f;
        float healingAmount = CurrentMaxHealth * percent;
        if (currentHealth + Mathf.RoundToInt(healingAmount) > CurrentMaxHealth)
        {
            currentHealth = CurrentMaxHealth;
        }
        else
        {
            currentHealth += Mathf.RoundToInt(healingAmount);
        }
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + "dies");
    }

}

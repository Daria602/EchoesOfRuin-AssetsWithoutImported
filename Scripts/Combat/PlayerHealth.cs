using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public bool GetDamaged(int damage)
    {
        
        if (currentHealth - damage <= 0)
        {
            Die();
            if (OnHealthUpdateCallback != null)
            {
                OnHealthUpdateCallback.Invoke();
            }
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
        Debug.Log("Player dies");
    }
    //public void Die()
    //{
    //    Debug.Log(gameObject.name + "dies");
    //}
}

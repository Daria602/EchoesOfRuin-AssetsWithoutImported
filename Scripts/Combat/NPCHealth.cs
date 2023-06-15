using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : Health
{
    public bool GetDamaged(float damage)
    {
        if (currentHealth - damage <= 0)
        {
            Die();
            return false;
        }
        else
        {
            currentHealth -= damage;
            return true;
        }
    }
    public override void Die()
    {
        base.Die();
        Debug.Log("NPC dies");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static CombatManager;

public class NPCHealth : Health
{
    public bool GetDamaged(int damage)
    {
        
        if (currentHealth - damage <= 0)
        {
            //Die();
            // NPC dies
            FindObjectOfType<PlayerController>().AddXP(gameObject.GetComponent<Stats>().grantedXP);
            return true;
        }
        else
        {
            currentHealth -= damage;
            // NPC was damaged, but didn't die
            return false;
        }
    }
    public override void Die()
    {
        base.Die();
        // TODO:  handle loot shooting out of the NPC here
        //        and death animation as well
        Debug.Log("NPC dies");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static CombatManager;

public class NPCHealth : Health
{
    //public static event EnemyDeathEventHandler OnEnemyDeath;
    public int XPReward = 10;
    public bool GetDamaged(int damage)
    {
        
        if (currentHealth - damage <= 0)
        {
            //Die();
            // NPC dies
            return true;
        }
        else
        {
            currentHealth -= damage;
            //if (OnHealthUpdateCallback != null)
            //{
            //    OnHealthUpdateCallback.Invoke();
            //}
            // NPC was damaged, but didn't die
            return false;
        }
    }
    public override void Die()
    {
        base.Die();
        //OnEnemyDeath.Invoke(this);
        FindObjectOfType<PlayerController>().AddXP(gameObject.GetComponent<Stats>().grantedXP);
        CombatManager.GetInstance().SignalDeath(gameObject.GetComponent<CombatController>().characterId);
        Destroy(gameObject);
        Debug.Log("NPC dies");

    }
}

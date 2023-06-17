using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static CombatManager;

public class NPCHealth : Health
{
    //public static event EnemyDeathEventHandler OnEnemyDeath;
    public int XPReward = 10;
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
        //OnEnemyDeath.Invoke(this);
        FindObjectOfType<PlayerController>().AddXP(gameObject.GetComponent<Stats>().grantedXP);
        CombatManager.GetInstance().SignalDeath(gameObject.GetComponent<CombatController>().characterId);
        Destroy(gameObject);
        Debug.Log("NPC dies");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private static CombatManager instance;
    //public GameObject player;
    //public List<GameObject> participants;

    public static CombatManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many CombatManager instances. It's a singleton class");
        }
        instance = this;
    }

    //public void InitiateCombat(ref GameObject player, ref List<GameObject>)
    //{
    //    player.GetComponent<CombatController>().IsInCombat = true;
    //    Debug.Log("Player is in combat: " + player.GetComponent<CombatController>().IsInCombat);
    //    foreach (var enemy in participants)
    //    {
    //        enemy.GetComponent<CombatController>().IsInCombat = true;
    //        Debug.Log("Enemy is in combat: " + enemy.GetComponent<CombatController>().IsInCombat);
    //    }
    //}
}

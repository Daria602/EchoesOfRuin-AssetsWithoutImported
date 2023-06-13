using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private static CombatManager instance;
    public GameObject player;
    public GameObject[] participants;

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

    public void InitiateCombat()
    {
        player.GetComponent<PlayerMovement>().IsAllowedToMove = false;
        foreach (var enemy in participants)
        {
            enemy.GetComponent<NPCMovement>().IsAllowedToMove = false;
        }
    }
}

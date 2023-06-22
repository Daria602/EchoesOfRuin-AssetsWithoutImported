using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Create New Quest")]
public class Quest : ScriptableObject
{
    public int XPGranted;
    public bool isActive = false;
    public Constants.QuestType questType;
    
    [Header("For Kill type")]
    public int[] enemyIds;

    [Header("For Explore type")]
    public Vector3 positionToGetTo;

    [Header("For Talk type")]
    public int idNPC;

    [Header("Quest description")]
    public string questTitle;
    public string questDescription;

    
}

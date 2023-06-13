using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Create New Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public int cost;

    public void DoAction()
    {
        Debug.Log("Doing " + skillName);
    }
}

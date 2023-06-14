using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Create New Weapon")]
public class Weapon : ScriptableObject
{
    public int minDamage;
    public int maxDamage;
    public GameObject weaponPrefab;
    public int level;
}

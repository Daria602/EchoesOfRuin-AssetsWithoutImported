using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Create New Potion")]
public class Potion : Item
{
    [Header("If heals, put healing number, otherwise -1")]
    public int healsFor = -1;
    [Header("If doesn't heal, what does it increase?")]
    public int buffsForTurns;
    public Constants.Attributes[] attributesBuffed;
    public Constants.Abilities[] abilitiesBuffed;

    public override void Use()
    {
        base.Use();
        CombatManager.GetInstance().ApplyPotion(this);

    }
}

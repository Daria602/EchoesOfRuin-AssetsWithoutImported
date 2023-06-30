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
        /*
         if it's a healing potion, use it,
        if it's a buff potion, say that you can't use it out of combat
         */
        if (healsFor > 0)
        {
            // heal for healsFor
            PlayerHealth ph = FindObjectOfType<PlayerHealth>();
            ph.PotionHeal(healsFor);
            ph.UpdateHealthUI();
            InventoryManager.GetInstance().Remove(this);
            if (CombatManager.GetInstance().IsCombatGoing)
            {
                CombatManager.GetInstance().UpdatePortraits();
                
            }
        }
        else
        {
            if (CombatManager.GetInstance().IsCombatGoing)
            {
                CombatManager.GetInstance().ApplyPotion(this);
                InventoryManager.GetInstance().Remove(this);
            }
            else
            {
                Debug.Log("Can't use the potion outside of the combat");
                // say that you can't use potions outside of combat
            }
        }

    }

    public string GetTooltip()
    {
        string potionName = name;
        string tooltipString;
        if (healsFor < 0)
        {
            
            int addsPointsAmount = (attributesBuffed.Length == 0) ? abilitiesBuffed.Length : attributesBuffed.Length;
            string statBuffed = (attributesBuffed.Length == 0) ? abilitiesBuffed[0].ToString() : attributesBuffed[0].ToString();
            int turns = buffsForTurns;
            tooltipString =
                "~" + potionName + "`\n" +
                "Adds $+" + addsPointsAmount + " `to $" + statBuffed +
                " `for $" + turns + " `turns\n" +
                "!Can only be used in a battle";
        }
        else
        {
            tooltipString =
                "~" + potionName + "`\n" +
                "Heals for $20% of the maximum health";

        }
        return tooltipString;
    }
}

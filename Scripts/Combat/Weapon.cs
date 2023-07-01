using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Create New Weapon")]
public class Weapon : Item
{
    public int weaponId;
    public int minDamage;
    public int maxDamage;
    public GameObject weaponPrefab;
    public int level;
    public Constants.WeaponTypes weaponType;
    public bool isEquipped = false;
    // public bool equipped = false;
    //public int basicSkillId;

    public override void Use()
    {
        base.Use();
        //Debug.Log("Using weapon: " + name +
        //    "; Equipped is " + equipped);
        //if (equipped)
        //{
        //    equipped = false;
        //    FindObjectOfType<PlayerCombat>().PlayerUnequipsWeapon();
            
        //}
        //else
        //{
            //equipped = true;
        FindObjectOfType<PlayerCombat>().PlayerEquipsWeapon(weaponId);
        SkillPanelController.GetInstance().UpdateSkillbar();
            
        //}
        //Debug.Log("Using weapon: " + name +
            //";Second time Equipped is " + equipped);


    }

    public string GetTooltip()
    {
        string weaponName = weaponType.ToString();
        string minDam = minDamage.ToString();
        string maxDam = maxDamage.ToString();
        return "~" + weaponName + "\n" + "$Damage: " + minDam + " - " + maxDam; 

    }
}

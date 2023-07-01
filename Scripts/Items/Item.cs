using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Create New Item")]
public class Item : ScriptableObject
{
    
    new public string name = "New Item";
    public int itemId;
    public Sprite itemIcon = null;
    public bool isDefaultItem = false;
    public int price = 10;

    public virtual void Use()
    {
        Debug.Log("Using item " + name);
    }


}

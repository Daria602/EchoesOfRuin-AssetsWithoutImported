using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Create New Item")]
public class Item : ScriptableObject
{

    new public string name = "New Item";
    public Sprite itemIcon = null;
    public bool isDefaultItem = false;

    public virtual void Use()
    {
        Debug.Log("Using item " + name);
    }


}

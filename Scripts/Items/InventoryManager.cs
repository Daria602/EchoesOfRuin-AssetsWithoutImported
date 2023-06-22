using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //Singleton pattern, shared by all instances of this class
    private static InventoryManager instance;
    public static InventoryManager GetInstance()
    {
        return instance;
    }

    public List<Item> items = new List<Item>();
    public int inventorySpace = 25;
    //Trigger this callback to inform UI of inventory changes
    public OnItemChanged onItemChangedCallback;

    //Delegate
    public delegate void OnItemChanged();

    private void Awake()
    {
        //Setting it to this instance (Access by inventory.instance)
        if (instance != null)
        {
            Debug.LogWarning("More than one inventory");
            return;
        }
        instance = this;
    }
    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= inventorySpace)
            {
                Debug.Log("No more inventory space");
                return false;
            }
            items.Add(item);

            if (onItemChangedCallback != null) 
            {
                onItemChangedCallback.Invoke();
            }
            
        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    
}

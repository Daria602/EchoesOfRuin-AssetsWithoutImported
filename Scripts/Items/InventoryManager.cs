using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ILoadingData
{
    //Singleton pattern, shared by all instances of this class
    private static InventoryManager instance;
    public static InventoryManager GetInstance()
    {
        return instance;
    }

    public List<Item> items = new List<Item>();
    public List<int> itemIds = new List<int>();
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
            AddToIds(item);

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
        RemoveFromIds(item);
        UIManager.GetInstance().RefreshEquipped();

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    private void RemoveFromIds(Item item)
    {
        itemIds.Remove(item.itemId);
    }
    private void AddToIds(Item item)
    {
        itemIds.Add(item.itemId);
    }

    public void LoadGameData(CharacterData characterData)
    {
        foreach (int itemID in characterData.inventoryItems)
        {
            Add(ItemSystem.GetInstance().itemMap[itemID]);
            Debug.Log("Adding item to the inventory: " + ItemSystem.GetInstance().itemMap[itemID].name);
            //if (onItemChangedCallback != null)
            //{
            //    onItemChangedCallback.Invoke();
            //}
        }
        
    }

    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.inventoryItems = itemIds;
    }
}

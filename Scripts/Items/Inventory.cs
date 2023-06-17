using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    public int[] itemIds;
    public bool shouldGenerateRandom = false;
    public delegate void ItemModified();
    public ItemModified OnItemModified;

    // Start is called before the first frame update
    void Start()
    {
        if (shouldGenerateRandom)
        {
            // generate random keys
        }
        {
            items = new List<Item>();
            for (int i = 0; i < itemIds.Length; i++)
            {
                Item itemInstantiated = Instantiate(ItemSystem.GetInstance().itemMap[itemIds[i]]);
                items.Add(itemInstantiated);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            //if (items.Count >= inventorySpace)
            //{
            //    Debug.Log("No more inventory space");
            //    return false;
            //}
            items.Add(item);

            if (OnItemModified != null)
            {
                OnItemModified.Invoke();
            }

        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        //if (onItemChangedCallback != null)
        //{
        //    onItemChangedCallback.Invoke();
        //}
    }
}

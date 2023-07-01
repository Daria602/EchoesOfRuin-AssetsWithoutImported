using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour, ILoadingData
{
    public int[] itemIdKeys;
    public Item[] itemObjectValues;
    public Dictionary<int, Item> itemMap = new Dictionary<int, Item>();

    public List<int> itemsWerePickedUp = new List<int>();


    private static ItemSystem instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many ItemSystem instances. Should be only one");
        }
        instance = this;
        if (itemIdKeys.Length != itemObjectValues.Length)
        {
            Debug.LogError("Keys and values of character id's dictionary have different size");
        }
        else
        {
            for (int i = 0; i < itemIdKeys.Length; i++)
            {
                itemMap.Add(itemIdKeys[i], itemObjectValues[i]);
            }
        }

        //var foundItems = FindObjectsOfType<ItemPickup>();
        //foreach (ItemPickup itemPickup in foundItems)
        //{
        //    //if (itemPickup.item.id)

        //    itemPickup.gameObject.SetActive(false);
        //}

    }
    public static ItemSystem GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameData(CharacterData characterData)
    {
        this.itemsWerePickedUp = characterData.itemsWerePickedUp;
    }

    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.itemsWerePickedUp = this.itemsWerePickedUp;
    }
}

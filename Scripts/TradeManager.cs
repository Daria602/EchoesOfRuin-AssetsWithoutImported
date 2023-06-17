using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{

    public Transform itemParentNPC;
    public Transform itemParentPlayer;
    public GameObject inventoryUiNPC;

    Inventory inventory;

    InventorySlot[] slotsNPC;
    SlotScript[] slotsPlayer;
    

    void UpdateUI()
    {
        for (int i = 0; i < slotsNPC.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slotsNPC[i].AddItem(inventory.items[i]);
            }
            else
            {
                slotsNPC[i].RemoveItem();
            }
        }

        for (int i = 0; i < slotsPlayer.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slotsPlayer[i].AddItem(InventoryManager.instance.items[i]);
            }
            else
            {
                slotsPlayer[i].RemoveItem();
            }
        }

        Debug.Log("Updating UI");
    }




    public GameObject tradePanel;
    //public GameObject NPCInventory;
    //public GameObject playerInventory;

    private static TradeManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many TradeManager instances. Should be only one");
        }
        instance = this;
    }
    public static TradeManager GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        tradePanel.SetActive(false);
        //NPCInventory.SetActive(false);
        //playerInventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerTrade(Inventory inventory)
    {
        tradePanel.SetActive(true);
        for (int i = 0; i < inventory.items.Count; i++)
        {
            Debug.Log(inventory.items[i].name);
        }

        this.inventory = inventory;
        inventory.OnItemModified += UpdateUI;
        slotsNPC = itemParentNPC.GetComponentsInChildren<InventorySlot>();
        slotsPlayer = itemParentPlayer.GetComponentsInChildren<SlotScript>();
        UpdateUI();

        
        //NPCInventory.SetActive(false);
        //playerInventory.SetActive(false);
    }
}

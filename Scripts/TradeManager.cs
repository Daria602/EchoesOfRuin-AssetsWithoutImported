using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{

    public Transform itemParentNPC;
    public Transform itemParentPlayer;
    public GameObject inventoryUiNPC;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI priceText;
    Inventory inventory;

    InventorySlot[] slotsNPC;
    SlotScript[] slotsPlayer;
    Item currentItem = null;
    InventorySlot slot;

    public Image icon;
    public Button confirmPurchase;


    void UpdateUI()
    {
        Debug.Log(FindObjectOfType<PlayerController>().gold);
        goldText.text = FindObjectOfType<PlayerController>().gold.ToString();
        
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
            if (i < InventoryManager.GetInstance().items.Count)
            {
                slotsPlayer[i].AddItem(InventoryManager.GetInstance().items[i]);
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
        priceText.text = "";
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

    public void ExitTrade()
    {
        DialogueManager.GetInstance().ExitTrade();
        tradePanel.SetActive(false);
    }

    public void IsWillingToPurchaseItem(Item item, InventorySlot itemHolder)
    {
        currentItem = item;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        priceText.text = item.price.ToString();
        slot = itemHolder;
        Debug.Log("Got here");
        
    }

    public void Purchase()
    {
        if (currentItem != null)
        {
            if (currentItem.price <= FindObjectOfType<PlayerController>().gold)
            {
                FindObjectOfType<PlayerController>().gold -= currentItem.price;
                InventoryManager.GetInstance().Add(currentItem);
                slot.RemoveItem();
                currentItem = null;
                icon.sprite = null;
                icon.enabled = false;
                UpdateUI();
            }
        }
    }
}

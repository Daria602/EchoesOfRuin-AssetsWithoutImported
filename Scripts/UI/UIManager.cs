using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ILoadingData
{
    [Header("Escape menu")]
    public GameObject escapeMenu;

    [Header("Inventory and Stats")]
    public GameObject inventoryAndStatsUI;
    public Transform slotsParent;
    SlotScript[] slots;

    [Header("Player health and XP")]
    public Slider XPSlider;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI xpPopUp;

    public int itemEquipped;


    private static UIManager instance;
    public static UIManager GetInstance() { return instance; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many UIManager instances");
        }
        instance = this;
    }
    private void Start()
    {
        
        InventoryManager.GetInstance().onItemChangedCallback += UpdateInventoryUI;
        slots = slotsParent.GetComponentsInChildren<SlotScript>();
        UpdateInventoryUI();
        UpdateEquipped(this.itemEquipped);
    }

    private void Update()
    {
        ToggleInGameMenu();
        ToggleInventoryAndStats();
    }

    public void RemoveUI()
    {
        XPSlider.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        xpText.gameObject.SetActive(false);
}
    private void ToggleInGameMenu()
    {
        if (InputManager.GetInstance().EscapeInput())
        {
            escapeMenu.SetActive(!escapeMenu.activeSelf);
        }
    }

    private void ToggleInventoryAndStats()
    {
        if (InputManager.GetInstance().ToggleInventoryInput())
        {
            inventoryAndStatsUI.SetActive(!inventoryAndStatsUI.activeSelf);
        }
    }
    public void CloseInventoryAndStats()
    {
        
        inventoryAndStatsUI.SetActive(false);
        
    }


    public void QuitGame()
    {
        DataManager.GetInstance().QuitGame();
    }

    public void ClickSaveGame()
    {
        DataManager.GetInstance().SaveState();
        
    }
    public void ClickLoadGame()
    {
        DataManager.GetInstance().LoadSave();
    }

    void UpdateInventoryUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < InventoryManager.GetInstance().items.Count)
            {
                slots[i].AddItem(InventoryManager.GetInstance().items[i]);
            }
            else
            {
                slots[i].RemoveItem();
            }
        }
        Debug.Log("Updating UI");
    }

    public void UpdateEquipped(int itemId)
    {
        Debug.Log("Got to update equipped with new id of: " + itemId);
        this.itemEquipped = itemId;
        for (int i = 0; i < InventoryManager.GetInstance().items.Count; i++)
        {
            Debug.Log("Id is in inventory: " + (itemId == InventoryManager.GetInstance().items[i].itemId).ToString());
            //if (i % 2 == 0)
            //{
            //    slots[i].SetEquipped(true);
            //}
            //else
            //{
            //    slots[i].SetEquipped(false);
            //}
            if (itemId == InventoryManager.GetInstance().items[i].itemId)
            {
                slots[i].SetEquipped(true);
            }
            else
            {
                slots[i].SetEquipped(false);
            }
        }
    }

    public void RefreshEquipped()
    {
        UpdateEquipped(this.itemEquipped);
    }


    public void UpdateXPSlider(int actualValue, int minValue = -1, int maxValue = -1)
    {
        if (minValue != -1 && maxValue != -1)
        {
            XPSlider.minValue = minValue;
            XPSlider.maxValue = maxValue;
        }
        XPSlider.value = actualValue;
    }

    public void UpdateXPText(string newText)
    {
        xpText.text = newText;
    }

    public void UpdateHealthSlider(int actualValue, int minValue = -1, int maxValue = -1)
    {
        if (minValue != -1 && maxValue != -1)
        {
            healthSlider.minValue = minValue;
            healthSlider.maxValue = maxValue;
        }
        healthSlider.value = actualValue;
    }

    public void UpdateHealthText(string newText)
    {
        healthText.text = newText;
    }

    public void ShowXPRecieved(int xpRecieved, bool leveledUp)
    {
        string finalString = xpRecieved + " XP";
        if (leveledUp)
        {
            finalString += "\n LEVEL UP";
        }
        StartCoroutine(ShowXPGranted(finalString, 2f));
    }

    IEnumerator ShowXPGranted(string message, float delay)
    {
        xpPopUp.gameObject.SetActive(true);
        xpPopUp.text = message;
        yield return new WaitForSeconds(delay);
        xpPopUp.gameObject.SetActive(false);
    }

    public void LoadGameData(CharacterData characterData)
    {
        this.itemEquipped = characterData.itemEquipped;
    }

    public void SaveGameData(ref CharacterData characterData)
    {
        characterData.itemEquipped = this.itemEquipped;
    }
}

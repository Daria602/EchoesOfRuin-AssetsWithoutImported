using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
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
    }

    private void Update()
    {
        ToggleInGameMenu();
        ToggleInventoryAndStats();
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


    public void QuitGame()
    {
        Application.Quit();
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

    public void ShowXPRecieved(int xpRecieved)
    {
        StartCoroutine(ShowXPGranted(xpRecieved + " XP", 2f));
    }

    IEnumerator ShowXPGranted(string message, float delay)
    {
        xpPopUp.gameObject.SetActive(true);
        xpPopUp.text = message;
        yield return new WaitForSeconds(delay);
        xpPopUp.gameObject.SetActive(false);
    }

}

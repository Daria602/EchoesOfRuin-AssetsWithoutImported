using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemParent;
    public GameObject inventoryUi;
    public GameObject statsUi;

    InventoryManager inventory;

    SlotScript[] slots;

    void Start()
    {
        inventory = InventoryManager.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemParent.GetComponentsInChildren<SlotScript>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (inventoryUi.activeInHierarchy == false)
                inventoryUi.SetActive(true);
            else
            {
                inventoryUi.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            statsUi.SetActive(statsUi.activeInHierarchy ? false : true);
            //if (statsUi.activeInHierarchy == false)
            //    statsUi.SetActive(true);
            //else
            //{
            //    statsUi.SetActive(false);
            //}
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].RemoveItem();
            }
        }
        Debug.Log("Updating UI");
    }
}

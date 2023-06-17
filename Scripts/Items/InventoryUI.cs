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
            inventoryUi.SetActive(inventoryUi.activeInHierarchy ? false : true);
            statsUi.SetActive(statsUi.activeInHierarchy ? false : true);
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

using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    Item item;
    public Image icon;
    public Button removeItemButton;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        removeItemButton.interactable = true;
    }

    public void RemoveItem()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeItemButton.interactable = false;
    }

    public void OnClickRemoveItemButton()
    {
        InventoryManager.instance.Remove(item);
        Debug.Log("Removing item " + item);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }
}

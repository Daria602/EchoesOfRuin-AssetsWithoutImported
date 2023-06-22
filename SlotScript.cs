using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    Item item;
    public Image icon;
    public Button removeItemButton;
    SimpleTooltip tooltip;
    string tooltipString = "";


    private void Start()
    {
        
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        removeItemButton.interactable = true;
        //tooltip.enabled = true;
        Book book = item as Book;
        if (book != null)
        {
            //Debug.Log(book.skill.skillName);
            tooltipString = GetBookDescription(book);
        }
        else
        {
            tooltipString = item.name;
        }
    }

    private string GetBookDescription(Book book)
    {
        string finalString = "";
        finalString += "~" + book.skill.skillName + "  `*" + book.skill.cost + " AP\n`";
        finalString += book.skill.description + "\n";
        finalString += "$Damage: " + book.skill.baseDamageMin + " - " + book.skill.baseDamageMax + "`";
        return finalString;
    }

    private void OnEnable()
    {
        //Debug.Log("Got to tooltip");
        tooltip = GetComponent<SimpleTooltip>();
        tooltip.infoLeft = tooltipString;
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
        InventoryManager.GetInstance().Remove(item);
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

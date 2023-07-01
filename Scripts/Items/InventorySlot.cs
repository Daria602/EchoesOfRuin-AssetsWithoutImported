using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item item;
    public Image icon;
    public Button removeItemButton;
    SimpleTooltip tooltip;
    string tooltipString = "";

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        removeItemButton.interactable = true;
        Refresh();
    }

    public void RemoveItem()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeItemButton.interactable = false;
        Refresh();
    }

    public void OnClickRemoveItemButton()
    {
        //InventoryManager.instance.Remove(item);
        //Debug.Log("Removing item " + item);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

    public void BuyItem()
    {
        TradeManager.GetInstance().IsWillingToPurchaseItem(item, this);
    }

    private void OnEnable()
    {
        //Debug.Log("Got to tooltip");
        tooltip = GetComponent<SimpleTooltip>();
        tooltip.infoLeft = tooltipString;
    }

    private void Refresh()
    {
        if (enabled)
        {
            tooltip = GetComponent<SimpleTooltip>();
            GetTooltipForItem();
            tooltip.infoLeft = tooltipString;
        }
    }

    private string GetBookDescription(Book book)
    {
        string finalString = "Book for " + book.skill.GetTooltip();
        //finalString += "~" + book.skill.skillName + "  `*" + book.skill.cost + " AP\n`";
        //finalString += book.skill.description + "\n";
        //finalString += "$Damage: " + book.skill.baseDamageMin + " - " + book.skill.baseDamageMax + "`";
        return finalString;
    }

    private void GetTooltipForItem()
    {
        Book book = item as Book;
        Potion potion = item as Potion;
        Weapon weapon = item as Weapon;
        if (book != null)
        {
            //Debug.Log(book.skill.skillName);
            tooltipString = GetBookDescription(book);
        }
        else if (potion != null)
        {
            tooltipString = GetPotionDescription(potion);
        }
        else if (weapon != null)
        {
            tooltipString = GetWeaponDescription(weapon);
        }
        else
        {
            tooltipString = "";

        }
    }
    private string GetWeaponDescription(Weapon weapon)
    {
        string finalString = weapon.GetTooltip();
        //finalString += "~" + book.skill.skillName + "  `*" + book.skill.cost + " AP\n`";
        //finalString += book.skill.description + "\n";
        //finalString += "$Damage: " + book.skill.baseDamageMin + " - " + book.skill.baseDamageMax + "`";
        return finalString;
    }

    private string GetPotionDescription(Potion potion)
    {
        string finalString = potion.GetTooltip();
        return finalString;
    }
}

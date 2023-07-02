using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    Item item;
    public Image icon;
    public Button removeItemButton;
    SimpleTooltip tooltip;
    string tooltipString = "";
    public GameObject equippedTag;


    private void Start()
    {
        
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        removeItemButton.interactable = true;
        Refresh();
        //GetTooltipForItem();
        //tooltip.enabled = true;

    }

    public void SetEquipped(bool isEquipped)
    {
        equippedTag.SetActive(isEquipped);
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

    private string GetBookDescription(Book book)
    {
        string finalString = "Book for " + book.skill.GetTooltip();
        //finalString += "~" + book.skill.skillName + "  `*" + book.skill.cost + " AP\n`";
        //finalString += book.skill.description + "\n";
        //finalString += "$Damage: " + book.skill.baseDamageMin + " - " + book.skill.baseDamageMax + "`";
        return finalString;
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

    public void RemoveItem()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeItemButton.interactable = false;
        //UIManager.GetInstance().CloseInventoryAndStats();
        Refresh();
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
            if (item as Weapon != null)
            {
                Weapon weapon = item as Weapon;
                weapon.isEquipped = !weapon.isEquipped;
                //if (weapon.isEquipped)
                //{
                //    equippedTag.SetActive(false);
                //}
                //else
                //{
                //    equippedTag.SetActive(false);
                //}
                
            }
        }
    }
}

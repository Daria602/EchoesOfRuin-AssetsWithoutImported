using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    bool isItemPicked;
    public override void Interact()
    {
        base.Interact();
        PickItemUp();
    }
    void PickItemUp()
    {
        //If the item was added to the list return true
        if (item.name != "Gold")
            isItemPicked = InventoryManager.GetInstance().Add(item);
        else 
        {
            PlayerController pc = FindObjectOfType<PlayerController>();
            pc.gold += pc.GoldDropped();
            isItemPicked = true;
        }

        if (isItemPicked) {

            if (GetComponent<VisualItem>() != null)
            {
                GetComponent<VisualItem>().RemoveItemFromWorld();
            }
            else
            {
                Destroy(gameObject);
            }
            
            //GetComponent<VisualItem>().RemoveItemFromConst();
            //Destroy(gameObject);
        }      
    }

}

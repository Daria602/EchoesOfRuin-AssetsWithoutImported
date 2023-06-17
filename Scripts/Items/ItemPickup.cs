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
        isItemPicked = InventoryManager.instance.Add(item);

        if (isItemPicked) {
            Destroy(gameObject);
        }      
    }

}

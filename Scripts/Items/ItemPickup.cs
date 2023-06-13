using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public override void Interact()
    {
        base.Interact();
        PickUpItem();
    }
    void PickUpItem()
    {
        Debug.Log("Picking up item " + item.name);        
        bool wasItemPickedUp = InventoryManager.instance.Add(item);

        if (wasItemPickedUp) {
            Destroy(gameObject);
        }      
    }

}

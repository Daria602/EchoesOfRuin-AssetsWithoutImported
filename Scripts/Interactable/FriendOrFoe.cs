using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendOrFoe : Interactable
{
    // Start is called before the first frame update

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interacting from FOF with " + transform.name);
    }
}

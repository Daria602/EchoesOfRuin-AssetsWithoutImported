using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueTrigger : Interactable
{
    // TODO: rethink this so that the player is not attached here

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    //public GameObject p;

    public override void Interact()
    {
        base.Interact();
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON, gameObject);
        
    }


}

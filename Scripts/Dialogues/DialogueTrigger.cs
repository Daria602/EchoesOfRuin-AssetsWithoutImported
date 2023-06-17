using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueTrigger : Interactable
{
    // TODO: rethink this so that the player is not attached here
    //public GameObject playerGo = null;


    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    //public GameObject p;

    public override void Interact()
    {
        base.Interact();
        //CombatManager.GetInstance().InitiateCombat(ref p);
        gameObject.GetComponent<NPCMovement>().IsAllowedToMove = false;
        gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        var player = FindObjectOfType<PlayerController>();
        player.GetComponent<PlayerMovement>().IsAllowedToMove = false;
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON, gameObject.GetComponent<Inventory>());
    }


}

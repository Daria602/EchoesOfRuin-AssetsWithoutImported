using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueTrigger : Interactable
{
    [SerializeField] private int questId;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    public override void Interact()
    {
        base.Interact();
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON, gameObject);
        
    }


}

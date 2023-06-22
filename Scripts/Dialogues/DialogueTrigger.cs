using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueTrigger : Interactable
{
    public int questId;
    public bool wasTalkedTo = false;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    public override void Interact()
    {
        base.Interact();
        wasTalkedTo = true;
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON, gameObject);
    }


}

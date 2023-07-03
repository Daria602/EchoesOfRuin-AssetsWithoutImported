using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : CharacterController
{
    public GameObject talkToNpcPrefab;
    private GameObject npcSaysInstance;
    private bool isHoveringOver = false;
    private void Update()
    {
        if (GetComponent<CombatController>().IsInCombat == false && FindObjectOfType<PlayerCombat>().IsInCombat == false
            && isHoveringOver)
        {
            npcSaysInstance.SetActive(true);
            
        }
        else
        {
            npcSaysInstance.SetActive(false);
        }
    }
    public void SayTalk()
    {
        if (!isHoveringOver)
        {
            isHoveringOver = true;
            StartCoroutine(WaitBeforeDisappearing());
        }
    }

    IEnumerator WaitBeforeDisappearing()
    {
        yield return new WaitForSeconds(2f);
        isHoveringOver = false;
    }

    private void Start()
    {
        if (Constants.GetInstance().charactersDead.Contains(characterId))
        {
            gameObject.SetActive(false);
        }
        else
        {
            npcSaysInstance = Instantiate(talkToNpcPrefab, transform);
            npcSaysInstance.SetActive(false);
        }
        
    }
}

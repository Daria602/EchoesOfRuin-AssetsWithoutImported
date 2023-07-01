using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] public int characterId;
    protected Animator animator;
    protected Stats stats;
    public GameObject itemDrop;
    public Transform rightHand;

    public void SetDoneDeath()
    {
        Debug.Log("Should do thingy");
        DropItem();
        if (Constants.GetInstance().characters[Constants.PLAYER_ID].GetComponent<Stats>().abilities.luck > 0) 
        {
            Debug.Log("Should call dropGold");
            DropGold();
        }

        gameObject.SetActive(false);
    }

    private void DropItem() 
    {
        Debug.Log("Inside DropItem call");
        if (itemDrop != null)
        {
            GameObject go = Instantiate(itemDrop);
            go.transform.position = transform.position;
            go.SetActive(true);
        }
        else
        {
            Debug.Log("It's NUULLLL");
        }
    }

    private void DropGold()
    {
        GameObject go = Instantiate(Constants.GetInstance().goldPrefab);
        go.transform.position = transform.position;
        Debug.Log("Dropped gold");
    }

}

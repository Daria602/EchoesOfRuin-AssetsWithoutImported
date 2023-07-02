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

    private void Start()
    {
        if (itemDrop != null)
        {
            itemDrop.SetActive(false);
        }
        foreach (int characterId in Constants.GetInstance().charactersDead)
        {
            if (characterId == this.characterId)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void SetDoneDeath()
    {
        Debug.Log("Should do thingy");
        DropItem();
        if (Constants.GetInstance().characters[Constants.PLAYER_ID].GetComponent<Stats>().abilities.luck > 0) 
        {
            Debug.Log("Should call dropGold");
            DropGold();
        }
        Constants.GetInstance().charactersDead.Add(characterId);
        gameObject.SetActive(false);
    }

    private void DropItem() 
    {
        Debug.Log("Inside DropItem call");
        if (itemDrop != null)
        {
            //GameObject go = Instantiate(itemDrop);
            //go.GetComponent<VisualItem>
            itemDrop.transform.position = transform.position + Vector3.up;
            //go.transform.position = transform.position;
            itemDrop.SetActive(true);
            itemDrop.GetComponent<VisualItem>().isDroppedBySomebody = false;
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

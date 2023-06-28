using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] public int characterId;
    protected Animator animator;
    protected Stats stats;
    public GameObject itemDrop;

    public void SetDoneDeath()
    {
        if (itemDrop != null)
        {
            itemDrop.SetActive(true);
            itemDrop.transform.position = transform.position;
        }
        gameObject.SetActive(false);
    }

}

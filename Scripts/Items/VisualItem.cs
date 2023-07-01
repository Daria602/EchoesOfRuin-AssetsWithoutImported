using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualItem : MonoBehaviour
{
    public int itemId;
    public bool isDroppedBySomebody;
    // Start is called before the first frame update
    void Start()
    {
        //if (!isDroppedBySomebody)
        //{
            if (!CheckIfExists())
            {
                gameObject.SetActive(false);
            }
        //}
        
    }

    private bool CheckIfExists()
    {
        for (int i = 0; i < ItemSystem.GetInstance().itemsWerePickedUp.Count; i++)
        {
            if (ItemSystem.GetInstance().itemsWerePickedUp[i] == itemId)
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveItemFromWorld()
    {
        ItemSystem.GetInstance().itemsWerePickedUp.Add(itemId);
        gameObject.SetActive(false);
        //for (int i = 0; i < Constants.GetInstance().itemsStillPresent.Length; i++)
        //{
        //    if (Constants.GetInstance().itemsStillPresent[i] == itemId)
        //    {
        //        Constants.GetInstance().itemsStillPresent[i] = -1;
        //    }
        //}
    }
    
}

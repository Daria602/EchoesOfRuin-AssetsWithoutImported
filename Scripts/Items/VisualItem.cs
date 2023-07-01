using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualItem : MonoBehaviour
{
    public int itemId;
    // Start is called before the first frame update
    void Start()
    {
        //if (!CheckIfExists())
        //{
        //    gameObject.SetActive(false);
        //}
    }

    private bool CheckIfExists()
    {
        for (int i = 0; i < Constants.GetInstance().itemsStillPresent.Length; i++)
        {
            if (Constants.GetInstance().itemsStillPresent[i] == itemId)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItemFromConst()
    {
        for (int i = 0; i < Constants.GetInstance().itemsStillPresent.Length; i++)
        {
            if (Constants.GetInstance().itemsStillPresent[i] == itemId)
            {
                Constants.GetInstance().itemsStillPresent[i] = -1;
            }
        }
    }
    
}

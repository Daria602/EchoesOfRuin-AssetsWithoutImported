using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPoint : MonoBehaviour
{
    public Sprite active;
    public Sprite inactive;
    public bool isActive = false;

    public void ToggleActive(bool activity)
    {
        isActive = activity;
        if (isActive)
        {
            GetComponent<Image>().sprite = active;
        }
        else
        {
            GetComponent<Image>().sprite = inactive;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleActive(true);
        }
    }
}

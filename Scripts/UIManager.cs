using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool shouldDisplayUI = false;
    public GameObject canvas;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            shouldDisplayUI = !shouldDisplayUI;
            if (shouldDisplayUI)
            {
                ActivateUI();
            } 
            else
            {
                DeactivateUI();
            }
        }

    }

    private void ActivateUI()
    {
        canvas.SetActive(true);
    }

    private void DeactivateUI()
    {
        canvas?.SetActive(false);
    }
}

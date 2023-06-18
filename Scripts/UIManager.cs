using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool shouldDisplayOptionsMenu = false;
    public GameObject escapeMenu;
    private void Update()
    {
       

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shouldDisplayOptionsMenu = !shouldDisplayOptionsMenu;
            if (shouldDisplayOptionsMenu)
            {
                ActivateOptionsMenu();
            }
            else
            {
                DeactivateOptionsMenu();
            }
        }

    }

    private void ActivateOptionsMenu()
    {
        escapeMenu.SetActive(true);
    }

    private void DeactivateOptionsMenu()
    {
        escapeMenu.SetActive(false);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClickSaveGame()
    {
        DataManager.GetInstance().SaveState();
        
    }
    public void ClickLoadGame()
    {
        DataManager.GetInstance().LoadSave();
    }
}

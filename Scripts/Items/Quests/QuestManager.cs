using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager GetInstance()
    {
        return instance;
    }
    


    public GameObject journalPanel;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many QuestManager instances");
        }
        instance = this;
        journalPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal();
        }
    }

    public void ToggleJournal()
    {
        journalPanel.SetActive(!journalPanel.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour

{
    public GameObject storageUiPanel;
    private static StorageManager instance;
    //public GameObject player;
    //public int currentCharacterTurn = -1;

    public static StorageManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many StorageManager instances. It's a singleton class");
        }
        instance = this;
    }

    public void OpenStorageUi()
    {
        
    }
}

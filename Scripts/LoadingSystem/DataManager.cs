using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private CharacterData characterData;

    private List<ILoadingData> loadingDataScripts;

    [SerializeField]
    private string fileNameForSave;

    private LoadingFileHandler loadingFileHandler;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            Debug.Log("Something went wrong.");
        }
        this.loadingFileHandler = new LoadingFileHandler(Application.persistentDataPath, fileNameForSave);
        this.loadingDataScripts = GetAllLoadingDataScripts();
        LoadSave();
    }

    public static DataManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        
    }

    public void CreateNewSave()
    {
        this.characterData = new CharacterData();
    }

    public void LoadSave()
    {
        // load saved data from a file
        this.characterData = loadingFileHandler.LoadData();

        if (characterData == null)
        {
            Debug.Log("No save file found. Taking data from defaults");
            CreateNewSave();
        }

        // give the data to all other scripts
        for (int i = 0; i < loadingDataScripts.Count; i++)
        {
            loadingDataScripts[i].LoadGameData(characterData);
        }
    }

    public void SaveState()
    {
        foreach (ILoadingData dataObject in loadingDataScripts)
        {
            dataObject.SaveGameData(ref characterData);
        }

        loadingFileHandler.SaveData(characterData);
    }

    public void Continue()
    {
        SaveState();
        SceneManager.LoadScene(1);
    }

    private List<ILoadingData> GetAllLoadingDataScripts()
    {
        IEnumerable<ILoadingData> list = FindObjectsOfType<MonoBehaviour>().OfType<ILoadingData>();

        return new List<ILoadingData>(list);
    }
}

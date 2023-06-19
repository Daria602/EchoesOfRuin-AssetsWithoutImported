using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class LoadingFileHandler
{
    private string directoryPath = "";
    private string fileName = "";

    public LoadingFileHandler(string directoryPath, string fileName)
    {
        this.directoryPath = directoryPath;
        this.fileName = fileName;
    }

    public CharacterData LoadData()
    {
        string path = Path.Combine(directoryPath, fileName);
        CharacterData data;
        if (File.Exists(path))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        dataToLoad = sr.ReadToEnd();
                    }
                }

                data = JsonUtility.FromJson<CharacterData>(dataToLoad);
                return data;

            }
            catch (Exception)
            {
                Debug.Log("Error while reading the file");
            }
        }
        return null;
    }

    public void SaveData(CharacterData characterData)
    {
        string path = Path.Combine(directoryPath, fileName);
        try
        {
            Directory.CreateDirectory(directoryPath);

            // make json from characterData
            string dataToSave = JsonUtility.ToJson(characterData, true);

            // write to file

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(dataToSave);
                }
            }

        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class FileDataHandler
{
    string filePath;
    string fileName;
    public FileDataHandler(string filePath,string fileName)
    {
        this.filePath = filePath;
        this.fileName = fileName;
    }

    public void Save(GameData gameData)
    {
        string fullpath = Path.Combine(filePath,fileName);
        Debug.Log(fullpath);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

            string dataToStore = JsonUtility.ToJson(gameData);

            using (FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception ex) {
            Debug.LogError("Error occured when trying to save data to file: " + fullpath + "\n" + ex);
        }

    }

    public GameData Load()
    {

        string fullpath = Path.Combine(filePath, fileName);
        GameData loadedData = null;

        if (File.Exists(fullpath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullpath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullpath + "\n" + ex);
            }
        }
        return loadedData ;
    }
}


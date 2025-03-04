using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance;
    private FileDataHandler fileHandler;
    public GameData gameData;

    private List<IData> datas;

    string filePath;
    string fileName = "Totaly not all game information do not watch.Amogus";

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("DataPersistancemanager Have more than 1 Instance");
        }
        else
        {
            Instance = this;
        }

        fileHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        datas = FindAllIDataObjects();


        LoadGame();
    }

    private void Start()
    {
    }

    void Update()
    {

    }

    void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData =  fileHandler.Load();

        if(gameData == null)
        {
            NewGame();
        }

        if (datas == null)
        {
            Debug.LogError("Datas is null while Loading");
        }
        foreach (IData data in datas)
        {
            //data.Load(gameData);
        }
        //if (gameData == null)
        //{
        //}
        //return new GameData();
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogError("Game data is null while saving");
        }

        if (datas == null)
        {
            Debug.LogError("Datas is null while saving");
        }
        foreach (IData data in datas)
        {
           data.Save(gameData);
        }

        fileHandler.Save(gameData);
    }
    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}

    List<IData> FindAllIDataObjects()
    {
        IEnumerable<IData> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IData>();

        return new List<IData>(dataPersistenceObjects);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{

    public void LoadNextLevel()
    {
        int sectionNumber = GameManager.Instance.sectionNumber;
        int levelNumber = GameManager.Instance.levelNumber + 1;
        string nextLevel = $"Level {sectionNumber}-{levelNumber}";

        string scenePath = $"Assets/Scenes/Section {sectionNumber}/All/Level {sectionNumber}-{levelNumber}.unity";
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
        Debug.Log(SceneUtility.GetScenePathByBuildIndex(4));
        if (buildIndex != -1){
            
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            LoadLevelMenu();
        }

        //int nextLevelId = SceneManager.GetActiveScene().buildIndex + 1;
        //SceneManager.LoadScene(nextLevelId);
    }

    public void LoadLevel(int idx)
    {
        SceneManager.LoadScene(idx);
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevelMenu()
    {
        int sectionNum = GameManager.Instance.sectionNumber;
        string sectionName = $"Level Menu {sectionNum}";
        //int menuId = 2;
        SceneManager.LoadScene(sectionName);
    }

    public void LoadLevelMenu(int sectionNum)
    {
        //int sectionNum = GameManager.Instance.sectionNumber;
        string sectionName = $"Level Menu {sectionNum}";
        //int menuId = 2;
        SceneManager.LoadScene(sectionName);
    }

    public void LoadSectionMenu()
    {
        SceneManager.LoadScene("Sections Menu");
    }

    public void LoadStartMenu()
    {
        int StartMenuId = 0;

        SceneManager.LoadScene(StartMenuId);
    }

    public void PauseLevel(GameObject PauseMenu)
    {
        PauseMenu.SetActive(true);
        GameManager.Instance.UpdatePauseMenu();
    }

    public void UnPauseLevel(GameObject PauseMenu)
    {
        PauseMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum InputType { Mobile,PC}

[System.Serializable]
public class GameManager : MonoBehaviour, IData
{
    public static InputType GameInputType = InputType.Mobile;

    //[SerializeField] private IconHandler Handler;
    [SerializeField] private GameObject RestartMenu;
    [SerializeField] private UILevelResult WinMenu;
    [SerializeField] private UILevelResult LoseMenu;
    [SerializeField] private UILevelResult PauseMenu;

    [SerializeField] private SlingshotHandler sl;

    private List<Enemy> Enemies = new List<Enemy>();
    public List<Bird> Birds; //Не перевищувати за 10
    [SerializeField] private GameObject BirdsContainer;
    [SerializeField] private int score;
    [SerializeField] private int stars;
    [SerializeField] private TextMeshProUGUI ScoreUI;

    public static GameManager Instance;

    public Canvas UICanvas;
    public TextMeshProUGUI FloatTextPrefab;
    public TextMeshProUGUI LongFloatTextPrefab;

    public int maxNumOfShots;
    public int currNumOfShots;

    private float checkBeforeEnd = 10f;
    public bool Scoring = true;
    public int scoreMultiplicator = 10;

    //level Data
    public int maxScore;
    public int sectionNumber;
    public int levelNumber;
    public bool levelUnlocked;
    public bool newHightScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy.gameObject.CompareTag("Pig"))
            {
                Enemies.Add(enemy);
            }
        }

        BackGroundMusic music = FindFirstObjectByType<BackGroundMusic>();
        if (music != null)
        {
            music.StopMusic();
        }
        //Time.timeScale = 0.2f;
    }

    private void Start()
    {
        currNumOfShots = 0;

        Birds = new List<Bird>();
        Birds.AddRange(BirdsContainer.GetComponentsInChildren<Bird>());
        Birds[0].OnSlingshot = true;

        maxNumOfShots = Birds.Count;

        //foreach(Bird bird in BirdsContainer.GetComponentsInChildren<Bird>())
        //{
        //    Birds.Add(bird);
        //}
    }

    //Slingshot functions
    public void UseShot()
    {
        //Handler.UseShot(currNumOfShots);
        currNumOfShots++;
        CheckLastShoot();
    }
    public bool CanShoot()
    {
        return currNumOfShots != maxNumOfShots;
    }

    private void CheckLastShoot()
    {
        if (currNumOfShots == maxNumOfShots)
        {
        Debug.Log("LAst Shoot Check");
            StartCoroutine(nameof(CheckAfterTime));
        }
    }

    private IEnumerator CheckAfterTime()
    {
        yield return new WaitForSeconds(checkBeforeEnd);

        if (Enemies.Count <= 0)
        {
            //WinGame();
            WinSequence();

        }
        else
        {
            LoseGame();
        }

    }

    //Enemy functions
    public void RemoveEnemy(Enemy en)
    {
        Debug.Log("Enemy Removed");
        if (en.gameObject.CompareTag("Pig"))
        {

            Enemies.Remove(en);
            if (AllEnemiesDead())
            {
                //WinGame();
                //Debug.LogWarning("Win Sequence Method");

                WinSequence();
            }
        }
        
    }
    private bool AllEnemiesDead()
    {
        return Enemies.Count <= 0;
    }

    //Level Result functions
    private void WinGame()
    {
        sl.enabled = false;
        Scoring = false;

        Debug.Log("Win");
        //AddBirdsToScore();

        //RestartMenu.SetActive(true);

        DataPersistanceManager.Instance.gameData.UnlockLevelByNumber(sectionNumber, levelNumber + 1);
        if (levelNumber == DataPersistanceManager.Instance.gameData.Sections[sectionNumber - 1].Levels.Count)
        {
            DataPersistanceManager.Instance.gameData.UnlockSectionByNum(sectionNumber + 1);
        }
        stars = GetStarsCount(score);
        DataPersistanceManager.Instance.SaveGame();
        SetWinMenu();
        WinMenu.gameObject.SetActive(true);


        //Save();
    }

    private void LoseGame()
    {
        Debug.Log("Lost");
        //RestartMenu.SetActive(true);
        SetLoseMenu();
        LoseMenu.gameObject.SetActive(true);

        sl.enabled = false;
        Scoring = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Score functions
    public void AddScore(int points)
    {
        if (Scoring)
        {
            points *= scoreMultiplicator;
            score += points;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        ScoreUI.text = score.ToString();

    }


    //Particle functions
    public void CreateDamageParticle(Vector2 worldPos, int points)
    {
        points *= scoreMultiplicator;
        Vector2 cameraPos = Camera.main.WorldToScreenPoint(worldPos);

        var scoreText = Instantiate(FloatTextPrefab, cameraPos, Quaternion.identity, UICanvas.transform);
        scoreText.GetComponent<TextMeshProUGUI>().text = points.ToString();

        StartCoroutine(ClearParticle(scoreText.gameObject, 2f));
        //StartCoroutine(ClearParticle,(object) (scoreText.gameObject,2f));

    }
    public void CreateLongDamageParticle(Vector2 worldPos, int points)
    {
        Debug.Log("Creating long particle");
        points *= scoreMultiplicator;
        Vector2 cameraPos = Camera.main.WorldToScreenPoint(worldPos);

        var scoreText = Instantiate(LongFloatTextPrefab, cameraPos, Quaternion.identity, UICanvas.transform);
        scoreText.GetComponent<TextMeshProUGUI>().text = points.ToString();

        StartCoroutine(ClearParticle(scoreText.gameObject, 2f));
        //StartCoroutine(ClearParticle,(object) (scoreText.gameObject,2f));

    }
    private IEnumerator ClearParticle(GameObject part, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(part);
    }

    //Save & Load Shit
    public void Save(GameData gameData)
    {
        int sectionIdx = sectionNumber - 1;
        int levelIdx = levelNumber - 1;

        LevelStats level = gameData.GetLevel(sectionIdx, levelIdx);

        if (level.LevelScore < score)
        {
            level.LevelScore = score;
            level.StarsCount = stars;

            newHightScore = true;
        }
        //level.Unlocked = levelUnlocked;


        //gameData.Sections[sectionIdx].Levels[levelIdx].LevelScore = score;
        //gameData.Sections[sectionIdx].Levels[levelIdx].StarsCount = stars;
        //gameData.Sections[sectionIdx].Levels[levelIdx].Unlocked = levelUnlocked;
    }

    public void Load(GameData gameData)
    {
        int sectionIdx = sectionNumber - 1;
        int levelIdx = levelNumber - 1;

        LevelStats level = gameData.GetLevel(sectionIdx, levelIdx);
        score = level.LevelScore;
        stars = level.StarsCount;
        levelUnlocked = level.Unlocked;

        //score = gameData.Sections[sectionIdx].Levels[levelIdx].LevelScore;
        //stars = gameData.Sections[sectionIdx].Levels[levelIdx].StarsCount;
        //levelUnlocked = gameData.Sections[sectionIdx].Levels[levelIdx].Unlocked;


    }

    public void UnlockLevel(GameData gameData, int SectionIdx, int Levelidx)
    {
        gameData.Sections[SectionIdx].Levels[Levelidx].Unlocked = true;
    }

    public void SetWinMenu()
    {
        //Saved Score Part
        LevelStats level = DataPersistanceManager.Instance.gameData.GetLevel(sectionNumber - 1, levelNumber - 1);

        WinMenu.SetSavedHightScore(level.LevelScore);
        WinMenu.SetLevelNumber(sectionNumber, levelNumber);
        WinMenu.SetSavedStars(level.StarsCount);

        //MainPanel
        WinMenu.SetScore(score);
        WinMenu.SetStars(stars);
        if (newHightScore)
        {
            WinMenu.SetNewHightScoreSymbolVisible();
        }

    }

    public void SetLoseMenu()
    {
        //Saved Score Part
        LevelStats level = DataPersistanceManager.Instance.gameData.GetLevel(sectionNumber - 1, levelNumber - 1);

        LoseMenu.SetSavedHightScore(level.LevelScore);
        LoseMenu.SetLevelNumber(sectionNumber, levelNumber);
        LoseMenu.SetSavedStars(level.StarsCount);
    }

    public void SetPauseMenu()
    {
        PauseMenu.SetLevelNumber(sectionNumber, levelNumber);
    }

    public void PauseLevel()
    {
        Time.timeScale = 0;
        AudioListener.volume = 0;
        SetPauseMenu();
        PauseMenu.gameObject.SetActive(true);

        //if (!SoundButtonManager.Paused)
        //{
        //    AudioListener.volume = 0;
        //    SoundManager.PauseAllSounds();
        //}
        //AudioListener.pause = true;

    }

    public void UnPauseLevel()
    {
        PauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;

        if (!SoundButtonManager.Paused)
        {
            AudioListener.volume = 1;
            //SoundManager.UnPauseAllSounds();
        }

        //AudioListener.pause = false;
    }

    public void ExitLevel()
    {
        Time.timeScale = 1;
        SoundButtonManager.ReturnSoundIfNotPaused();
        //SoundManager.UnPauseAllSounds();
    }

    public int GetStarsCount(int levelScore)
    {
        float percent = levelScore / maxScore;
        if (percent > 0.8f)
        {
            return 3;
        }
        else if (percent > 0.4f)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private void WinSequence()
    {
        StartCoroutine(AddBirdsToScore());
    }

    private IEnumerator AddBirdsToScore()
    {
        yield return new WaitForSeconds(2f);
        List<Bird> tempBirds = GetLeftBirds();
        for (int i = 0; i < tempBirds.Count; i++)
        {
            Vector2 newPos = tempBirds[i].transform.position;
            CreateLongDamageParticle(newPos, 1000);
            AddScore(1000);
            yield return new WaitForSeconds(1f);
        }

        WinGame();
    }

    private List<Bird> GetLeftBirds()
    {
        List<Bird> tempBirds = new List<Bird>();
        for (int i = currNumOfShots; i < Birds.Count; i++)
        {
            tempBirds.Add(Birds[i]);
        }
        //Birds.RemoveRange(0, currNumOfShots);
        return tempBirds;
    }
}

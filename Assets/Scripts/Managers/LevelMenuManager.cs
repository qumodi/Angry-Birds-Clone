using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class LevelMenuManager : MonoBehaviour
{
    public int SectionIdx;

    const int rows = 3;
    const int cols = 7;
    //[SerializeField] private GameObject[][] LevelMenu = new GameObject[rows][];
    [SerializeField] private List<GameObject> LevelMenuRows;
    [SerializeField] private List<GameObject> LocksRows;

    [SerializeField] private ButtonsManager buttonsManager;

    [SerializeField] private List<GameObject> Pages;
    [SerializeField] private GameObject PagesContainer;

    [SerializeField] private Button LeftButton;
    [SerializeField] private Button RightButton;

    public int ActivePageNumber = 1;
    public bool TurningPage = false;

    private void Awake()
    {
        BackGroundMusic music = FindFirstObjectByType<BackGroundMusic>();
        if(music != null)
        {
        music.PlayMusic();
        }
    }

    void Start()
    {
        //LevelMenu = new GameObject[rows][];
        //for(int i = 0; i < rows; i++)
        //{
        //    LevelMenu[i] = new GameObject[cols];
        //}
        //DataPersistanceManager.Instance.gameData.GetLevel(1);

     

        for (int i = 0; i < LevelMenuRows.Count; i++)
        {
            Button[] RowLevels = LevelMenuRows[i].GetComponentsInChildren<Button>();
            Image[] RowLocks = LocksRows[i].GetComponentsInChildren<Image>();

            for (int j = 0; j < RowLevels.Length; j++)
            {
                int levelIdx = i * cols + j;
                LevelStats level = DataPersistanceManager.Instance.gameData.GetLevel(SectionIdx, levelIdx);
                if(level == null)
                {
                    Debug.LogError("LEvel is null \n" +
                        "levelIdx : " + levelIdx);

                }
                if (level.Unlocked)
                {
                    RowLevels[j].interactable = true;
                    RowLocks[j].enabled = false;

                    string levelName = $"Level {SectionIdx + 1}-{levelIdx + 1}";
                    RowLevels[j].onClick.AddListener(() => buttonsManager.LoadLevel(levelName));

                    TextMeshProUGUI buttonText = RowLevels[j].GetComponentInChildren<TextMeshProUGUI>();
                    buttonText.text = (levelIdx + 1).ToString() ;
                }
                else
                {
                    RowLevels[j].interactable = false;
                    RowLocks[j].enabled = true;
                    //RowLevels[j].gameObject.SetActive(false);
                    //RowLocks[j].gameObject.SetActive(true);
                }
            }
        }
    }

    public void ClickedButton()
    {
        Debug.Log("Button Clicked");
    }
    public void ClickedButtonInt(int i)
    {
        Debug.Log("Button Clicked");
    }

    public void TurnRight()
    {
        if (TurningPage)
        {
            return;
        }

        if (ActivePageNumber < Pages.Count)
        {
            ActivePageNumber += 1;

            RectTransform rect = PagesContainer.GetComponent<RectTransform>();
            MoveX(rect, -1920, 1f);

            CheckIfButtonsActive();
        }

        
    }
    public void TurnLeft()
    {
        if (TurningPage)
        {
            return;
        }

        if (ActivePageNumber > 1)
        {
            ActivePageNumber -= 1;

            RectTransform rect = PagesContainer.GetComponent<RectTransform>();
            MoveX(rect, 1920, 1f);

            CheckIfButtonsActive();
        }

        
    }

    //public void MakePageMain(int PageNumber)
    //{
    //    //Vector2 MainPoint = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
    //    RectTransform rect = PagesContainer.GetComponent<RectTransform>();
    //    //rect.DOMoveX(1920,);
    //}

    private void CheckIfButtonsActive()
    {
        if (ActivePageNumber == Pages.Count)
        {
            RightButton.gameObject.SetActive(false);
        }
        else
        {
            RightButton.gameObject.SetActive(true);
        }

        if (ActivePageNumber == 1)
        {
            LeftButton.gameObject.SetActive(false);
        }
        else
        {
            
            LeftButton.gameObject.SetActive(true);
        }
    }


    public void MoveX(RectTransform rect, float move, float time)
    {
        StartCoroutine(MoveXCycle(rect, move, time));
    }

    private IEnumerator MoveXCycle(RectTransform rect, float move, float time)
    {
        Vector2 StartPos = rect.position;
        Vector2 EndPos = new Vector2(rect.position.x + move, rect.position.y);
        TurningPage = true;
        float timePassed = 0f;
        while (timePassed < time)
        {
            timePassed += Time.deltaTime;
            float percent = timePassed / time;
            rect.position = Vector2.Lerp(StartPos, EndPos, percent);
            //yield return new WaitForSeconds(WaitTime);
            yield return null;
        }
        TurningPage = false;
        //yield break;
    }
}

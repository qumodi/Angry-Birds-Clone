using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelResult : MonoBehaviour
{

    //Saved Score Panel
    [SerializeField] private TextMeshProUGUI LevelNumber;
    [SerializeField] private TextMeshProUGUI SavedHightScore;
    [SerializeField] private List<Image> SavedStars;
    public void SetSavedHightScore(int hightScore)
    {
        SavedHightScore.text = hightScore.ToString();
    }

    public void SetLevelNumber(int sectionNumber, int levelNumber)
    {
        LevelNumber.text = $"{sectionNumber}-{levelNumber}";
    }

    public void SetSavedStars(int starsCount)
    {
        for (int i = 0; i < SavedStars.Count; i++)
        {
            SavedStars[i].color = new Color(0, 0, 0, 0.5f);
        }

        for (int i = 0; i < starsCount; i++)
        {
            SavedStars[i].color = Color.white;
        }
    }

    //MainPanel
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private GameObject NewHightScoreSymbol;
    // [SerializeField] private bool newHightScore;
    [SerializeField] private List<Image> Stars;

    public void SetScore(int levelScore)
    {
        Score.text = $"Score : {levelScore.ToString()}";
    }

    public void SetNewHightScoreSymbolVisible()
    {
        NewHightScoreSymbol.SetActive(true);
    }

    public void SetStars(int starsCount)
    {
        for (int i = 0; i < Stars.Count; i++)
        {
            Stars[i].color = new Color(0, 0, 0, 0.5f);
        }

        for (int i = 0; i < starsCount; i++)
        {
            Stars[i].color = Color.white;
        }
    }

    private void Start()
    {
        if (NewHightScoreSymbol != null)
        {
            NewHightScoreSymbol.SetActive(false);

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectionMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Sections;

    private void Awake()
    {
        BackGroundMusic music = FindFirstObjectByType<BackGroundMusic>();
        if (music != null)
        {
            music.PlayMusic();
        }
    }

    void Start()
    {
        Debug.Log(Sections.Count);
        for (int i = 0; i < Sections.Count; i++)
        {
            if (i == 2)
            {
                Debug.Log("Las");
            }
            SectionManager secMan = Sections[i].GetComponent<SectionManager>();

            Section currSection = DataPersistanceManager.Instance.gameData.Sections[i];
            if (!currSection.Unlocked)
            {
                Sections[i].gameObject.GetComponent<Button>().interactable = false;
                secMan.SetLockVisible();
                continue;
            }

            int SectionScoreSum = 0;
            int SectionStarsSum = 0;
            foreach (LevelStats level in currSection.Levels)
            {
                if (!level.Unlocked)
                {
                    break;
                }
                SectionScoreSum += level.LevelScore;
                SectionStarsSum += level.StarsCount;
            }

            secMan.SetScore(SectionScoreSum);
            secMan.SetStars(SectionStarsSum);
        }
    }

}

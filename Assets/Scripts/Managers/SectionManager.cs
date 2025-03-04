using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private TextMeshProUGUI Stars;
    [SerializeField] private Image LockImage;
    public void SetScore(int score)
    {
        Score.text = score.ToString();
    }

    public void SetStars(int stars)
    {
        Stars.text = stars.ToString();
    }

    public void SetLockVisible()
    {
        LockImage.gameObject.SetActive(true);
    }
}

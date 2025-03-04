using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] Birds;
    [SerializeField] private Color UsedColor;

    private void Start()
    {
        for(int i = 0; i < GameManager.Instance.Birds.Count; i++)
        {
            Birds[i].enabled = true;
            Birds[i].sprite = GameManager.Instance.Birds[i].GetComponent<SpriteRenderer>().sprite;
        }
    }
    public void UseShot(int shotNumber)
    {
        Destroy(Birds[shotNumber].gameObject);
        //Birds[shotNumber].color = UsedColor;
    }


}

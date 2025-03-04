using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private int dotsNumber = 10;
    [SerializeField] private GameObject dotsParent;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotSpacing;
    [SerializeField] private float dotMinScale = 1;
    [SerializeField] private float dotMaxScale = 2;

    Transform[] dotsList;

    Vector2 pos;
    //dot pos
    float timeStamp;
    float offset = 0f;
    float MaxOffset = 0.05f;

    void Start()
    {
        Hide();
        PrepareDots();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Show();
        //}
        offset += MaxOffset * Time.deltaTime;
        if(offset >= MaxOffset)
        {
            offset = 0f;
        }
    }

    void PrepareDots()
    {
        dotsList = new Transform[dotsNumber];
        dotPrefab.transform.localScale = Vector3.one * dotMaxScale;

        float scale = dotMaxScale;
        float scaleFactor = scale / dotsNumber;

        for (int i = 0; i < dotsNumber; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = dotsParent.transform;

            dotsList[i].localScale = Vector3.one * scale;
            if (scale > dotMinScale)
                scale -= scaleFactor;
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    {

        
        timeStamp = dotSpacing;
        for (int i = 0; i < dotsNumber; i++)
        {
            //pos.x = (ballPos.x + forceApplied.x * timeStamp);
            //pos.y = (ballPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

            float t = i * dotSpacing;
            t += offset;
            pos = (Vector2) ballPos +(forceApplied * t) + 0.5f * Physics2D.gravity * (t*t) ;
            //you can simlify this 2 lines at the top by:
            //pos = (ballPos+force*time)-((-Physics2D.gravity*time*time)/2f);
            //
            //but make sure to turn "pos" in Ball.cs to Vector2 instead of Vector3	

            dotsList[i].position = pos;
            //timeStamp += dotSpacing;
        }
    }

    public void Show()
    {
        dotsParent.SetActive(true);
    }

    public void Hide()
    {
        dotsParent.SetActive(false);
    }
}

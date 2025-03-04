using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotArea : MonoBehaviour
{
    [SerializeField] LayerMask SlingshotAreaMask;
    public bool InSlingshotArea()
    {
        Vector2 pos = new Vector2();
        if (GameManager.GameInputType == InputType.Mobile)
        {
            if (Input.touches.Length > 0)
            {
                //Touch touch = Input.GetTouch(0);
                pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Debug.Log(pos);

            }

        }
        else if (GameManager.GameInputType == InputType.PC)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        return Physics2D.OverlapPoint(pos, SlingshotAreaMask);
    }
}

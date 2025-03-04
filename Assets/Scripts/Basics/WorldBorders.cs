using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBorders : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<Enemy>())
        {
            coll.gameObject.GetComponent<Enemy>().Die();
        }else if (coll.gameObject.GetComponent<Bird>())
        {
            coll.gameObject.GetComponent<Bird>().Die();
        }
    }

}

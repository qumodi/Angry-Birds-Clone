using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Birds;

    [SerializeField] private List<Transform> Backgrounds = new List<Transform>();
    [SerializeField] private float leftBorder = -36;
    [SerializeField] private float rightBorder = 36;

    private float leftMove = -18;
    //private float Move = -18;
    public float moveTime = 10f;

    //Generate

    [SerializeField] private Transform LeftDown;
    [SerializeField] private Transform RightDown;
    [SerializeField] private Transform LeftUp;
    [SerializeField] private Transform RightUp;

    public float BackBirdMinMoveTime = 3f;
    public float BackBirdMaxMoveTime = 10f;
    public float BackBirdRepeatMin = 1f;
    public float BackBirdRepeatMax = 5f;

    //button
    [SerializeField] private 
    void Start()
    {
        StartCoroutine(MoveCycle());
        StartCoroutine(GenerateBirdsCycle());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Back 1 x : {Backgrounds[0].transform.position.x}" +"\n" + 
            $"Back 3 x : {Backgrounds[2].transform.position.x}");
    }

    private IEnumerator MoveCycle()
    {
        //StartCoroutine(MoveBackGround);
        for (int i = 0; i < Backgrounds.Count; i++)
        {
            if (Backgrounds[i].position.x <= leftBorder)
            {
                Backgrounds[i].position = new Vector3(rightBorder, Backgrounds[i].position.y, Backgrounds[i].position.z);
            }
        }
        MoveBackGround();

        yield return new WaitForSeconds(moveTime);
        StartCoroutine(MoveCycle());
    }

    private void MoveBackGround()
    {
        for (int i = 0; i < Backgrounds.Count; i++)
        {
            StartCoroutine(BezierManager.MoveX(Backgrounds[i], leftMove, moveTime));
        }
    }

    private IEnumerator GenerateBirdsCycle()
    {

        float repeatTime = Random.Range(BackBirdRepeatMin, BackBirdRepeatMax+1);
        GenerateBackBirds();
        yield return new WaitForSeconds(repeatTime);

        StartCoroutine(GenerateBirdsCycle());
    }
    

    private void GenerateBackBirds()
    {
        GameObject bird = Instantiate(Birds[Random.Range(0, Birds.Count)]);
        //bird.Original = false;

        List<Vector3> newCurve = BezierManager.GenerateCurve(LeftDown, RightDown, LeftUp, RightUp);
        
        
        float moveTime = Random.Range(BackBirdMinMoveTime,BackBirdMaxMoveTime);
        StartCoroutine(BezierManager.MoveBezier(bird.transform, newCurve, 20, moveTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bird"))
        {
            Destroy(collision.gameObject);
        }
    }
}

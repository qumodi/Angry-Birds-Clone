using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class BezierManager : MonoBehaviour
{
    public static BezierManager Instance;

    [SerializeField] List<Transform> points;
    [SerializeField] AnimationCurve curve;
    public float time = 0;
    public int segments = 10;

    [SerializeField] private Transform obj;

    //public GameObject square;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //StartCoroutine(MovePath(obj,points, 3,1f));
    }
    void Update()
    {
        //for(int)
        //Debug.DrawLine(Vector3.zero, Vector3.up);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(Vector3.zero, Vector3.up);
        if (points == null || points.Count == 0) { return; }

        Gizmos.color = Color.green;
        float part = 1.0f / segments;
        Vector3 prevPoint = points[0].position;

        for (float i = 1; i <= segments; i++)
        {
            float time = i * part;
            Vector3 thisPoint = Bezier.GetPoint(points, time);
            //Vector3 thisPoint = Bezier.GetPoint(points[0].position, points[1].position, points[2].position, points[3].position, time;

            //Gizmos.DrawLine(prevPoint, Bezier.GetPoint(points, i));
            Gizmos.DrawLine(prevPoint, thisPoint);

            prevPoint = thisPoint;
        }

        //StartCoroutine(MovePath(this.transform, 3));
    }

    //public IEnumerator MovePath(Transform obj, List<Transform> Path, int segments, float time)
    //{
    //    float timePassed = 0;
    //    float part = time / segments;

    //    while (timePassed <= time)
    //    {
    //        timePassed += Time.deltaTime;
    //        float percent = timePassed / time;
    //        float progress = curve.Evaluate(percent);
    //        obj.position = Bezier.GetPoint(points, progress);
    //        yield return null;
    //    }

    //    yield return new WaitForEndOfFrame();
    //}

    public IEnumerator MoveToSlingshot(Transform obj, int segments, float time)
    {
        List<Vector3> newPath = new List<Vector3>();
        newPath.Add(obj.position);
        for (int i = 1; i < points.Count; i++)
        {
            newPath.Add(points[i].position);
        }
        float timePassed = 0;
        float part = time / segments;

        while (timePassed <= time)
        {
            timePassed += Time.deltaTime;
            float percent = timePassed / time;
            float progress = curve.Evaluate(percent);
            obj.position = Bezier.GetPoint(newPath, progress);

            yield return null;
        }

        yield return new WaitForEndOfFrame();
    }

    public static IEnumerator MoveBezier(Transform obj, List<Vector3> curve, int segments, float time)
    {
        float timePassed = 0;
        float part = time / segments;

        while (timePassed <= time)
        {
            if (obj == null) {
                yield break;
            }
            timePassed += Time.deltaTime;
            float percent = timePassed / time;
            obj.position = Bezier.GetPoint(curve, percent);

            yield return null;
        }

        yield return new WaitForEndOfFrame();
    }

    //public IEnumerator MoveTo(Transform obj, Vector3 Start, Vector3 End, float time)
    //{
    //    float timePassed = 0;
    //    float part = time / segments;

    //    while (timePassed <= time)
    //    {
    //        timePassed += Time.deltaTime;
    //        float percent = timePassed / time;
    //        obj.position = Vector3.Lerp(Start, End, percent);
    //        yield return null;
    //    }

    //    yield return new WaitForEndOfFrame();
    //}

    public static IEnumerator MoveX(Transform obj, float X, float time)
    {
        int segments = 10;
        Vector3 Start = obj.position;
        Vector3 End = new Vector3(Start.x + X, Start.y, Start.z);
        float timePassed = 0;
        float part = time / segments;

        while (timePassed <= time)
        {
            timePassed += Time.deltaTime;
            float percent = timePassed / time;
            obj.position = Vector3.Lerp(Start, End, percent);
            yield return new WaitForEndOfFrame();
            //yield return null;
        }

        yield return new WaitForEndOfFrame();
    }

    public static List<Vector3> GenerateCurve(Transform LeftDown, Transform RightDown, Transform LeftUp, Transform RightUp)
    {
        List<Vector3> curve = new List<Vector3>();
        curve.Add(GeneratePointInRectangle(LeftDown));
        curve.Add(GeneratePointInRectangle(LeftUp));
        curve.Add(GeneratePointInRectangle(RightUp));
        curve.Add(GeneratePointInRectangle(RightDown));
        return curve;
    }

    public static Vector3 GeneratePointInRectangle(Transform rect)
    {
        Vector3 worldPos = rect.position;

        float maxX = rect.localScale.x; 
        float maxY = rect.localScale.y;

        float newX = Random.Range(-maxX, maxX + 1);
        float newY = Random.Range(-maxY, maxY + 1);
        Vector3 newPoint = new Vector3(newX,newY,0) + worldPos;
        //Debug.Log(newPoint)
        //Instantiate(square, newPoint,Quaternion.identity);
        return newPoint;
    }
}

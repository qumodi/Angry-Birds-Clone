using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier
{
    public static Vector3 GetPoint(List<Vector3> points, float t)
    {
        List<Vector3> calculatedPoints = new List<Vector3>();
        calculatedPoints.AddRange(points);

        while(calculatedPoints.Count > 1)
        {
            calculatedPoints = Calculate(calculatedPoints, t);
        }

        return calculatedPoints[0];
    }
    public static Vector3 GetPoint(List<Transform> points, float t)
    {
        List<Vector3> calculatedPoints = new List<Vector3>();
        for(int i = 0 ;i < points.Count; i++)
        {
            calculatedPoints.Add(points[i].position);
        }

        while (calculatedPoints.Count > 1)
        {
            calculatedPoints = Calculate(calculatedPoints, t);
        }

        return calculatedPoints[0];
    }

    public static List<Vector3> Calculate(List<Vector3> points,float t)
    {
        List<Vector3> calculatedPoints = new List<Vector3>();
        for(int i = 0; i < points.Count - 1; i++)
        {
            Vector3 point = Vector3.Lerp(points[i], points[i + 1], t);
            calculatedPoints.Add(point);
        }
        return calculatedPoints;    
    }

    public static Vector3 GetPoint(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float t)
    {
        Vector3 point01 = Vector3.Lerp(point0, point1, t);
        Vector3 point12 = Vector3.Lerp(point1, point2, t);
        Vector3 point23 = Vector3.Lerp(point2, point3, t);

        Vector3 point012 = Vector3.Lerp(point01, point12, t);
        Vector3 point123 = Vector3.Lerp(point12, point23, t);

        Vector3 point0123 = Vector3.Lerp(point012, point23, t);

        return point0123;
    }
}

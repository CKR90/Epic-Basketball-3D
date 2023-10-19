using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mat : MonoBehaviour
{
    public static float radyan = Mathf.PI / 180;
    public static float map(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        from = Mathf.Clamp(from, fromMin, fromMax);
        fromMax -= fromMin;
        toMax -= toMin;
        from -= fromMin;
        float rank = from / fromMax;
        float val = toMax * rank;
        val += toMin;

        return val;
    }
    public static Vector3 rotx(float x, float y, float z, float a)
    {
        Vector3 H = new Vector3();
        H.x = x;
        H.y = Mathf.Cos(a * radyan) * y - Mathf.Sin(a * radyan) * z;
        H.z = Mathf.Sin(a * radyan) * y + Mathf.Cos(a * radyan) * z;
        if (H.x < 0.0001f && H.x > -0.0001f) H.x = 0f;
        if (H.y < 0.0001f && H.y > -0.0001f) H.y = 0f;
        if (H.z < 0.0001f && H.z > -0.0001f) H.z = 0f;
        return H;


    }
    public static Vector3 roty(float x, float y, float z, float a)
    {
        Vector3 H = new Vector3();
        H.x = Mathf.Cos(a * radyan) * x + Mathf.Sin(a * radyan) * z;
        H.y = y;
        H.z = -Mathf.Sin(a * radyan) * x + Mathf.Cos(a * radyan) * z;
        if (H.x < 0.0001f && H.x > -0.0001f) H.x = 0f;
        if (H.y < 0.0001f && H.y > -0.0001f) H.y = 0f;
        if (H.z < 0.0001f && H.z > -0.0001f) H.z = 0f;
        return H;


    }
    public static Vector3 rotz(float x, float y, float z, float a)
    {
        Vector3 H = new Vector3();
        H.x = Mathf.Cos(a * radyan) * x - Mathf.Sin(a * radyan) * y;
        H.y = Mathf.Sin(a * radyan) * x + Mathf.Cos(a * radyan) * y;
        H.z = z;
        if (H.x < 0.0001f && H.x > -0.0001f) H.x = 0f;
        if (H.y < 0.0001f && H.y > -0.0001f) H.y = 0f;
        if (H.z < 0.0001f && H.z > -0.0001f) H.z = 0f;
        return H;


    }
    public static float sin(float x)
    {
        float xVal;
        xVal = Mathf.Sin(x * radyan);
        return xVal;
    }
    public static float cos(float x)
    {
        float xVal;
        xVal = Mathf.Cos(x * radyan);
        return xVal;
    }
    public static float tan(float x)
    {
        float xVal;
        xVal = Mathf.Tan(x * radyan);
        return xVal;
    }
    public static float asin(float x)
    {
        float xVal;
        xVal = Mathf.Asin(x) / radyan;
        if (double.IsNaN(xVal)) xVal = 0;
        return xVal;
    }
    public static float acos(float x)
    {
        float xVal;
        xVal = Mathf.Acos(x) / radyan;
        if (double.IsNaN(xVal)) xVal = 0;
        return xVal;
    }
    public static float atan(float x)
    {
        float xVal;
        xVal = Mathf.Atan(x) / radyan;
        if (double.IsNaN(xVal)) xVal = 90;
        return xVal;
    }
    public static float sq(float x)
    {
        x = Mathf.Pow(x, 2);
        return x;
    }
    public static float FindAngleOfTriangleKnownOnThreeEdges(float OppositeEdgeOfAngle, float OhterSideEdge1, float OtherSideEdge2)
    {
        float angle = 0f;
        if ((OhterSideEdge1 + OtherSideEdge2) - OppositeEdgeOfAngle < 0.0001f) angle = 180f;
        else
        {
            angle = acos((sq(OppositeEdgeOfAngle) - sq(OhterSideEdge1) - sq(OtherSideEdge2)) / (-2 * OhterSideEdge1 * OtherSideEdge2));
        }
        return angle;
    }
    public static float heron_H(float floor, float b, float c)
    {
        float s = (floor + b + c) / 2f;
        float h = (Mathf.Sqrt(s * (s - floor) * (s - b) * (s - c))) / (0.5f * floor);
        return h;
    }
    public static float atan360xz(Vector3 v)
    {
        float a = 0f;
        if (v.x == 0f && v.z == 0f) a = 0f;
        else if (v.x == 0f && v.z > 0f) a = 90f;
        else if (v.x == 0f && v.z < 0f) a = 270f;
        else
        {
            a = atan(Mathf.Abs(v.z / v.x));
            if (v.x < 0f && v.z >= 0f) a = 180f - a;
            else if (v.x < 0f && v.z < 0f) a += 180f;
            else if (v.x >= 0f && v.z < 0f) a = 360f - a;
        }
        return a;
    }
    public static float atan360xy(Vector3 v)
    {
        float a = 0f;
        if (v.x == 0f && v.y == 0f) a = 0f;
        else if (v.x == 0f && v.y > 0f) a = 90f;
        else if (v.x == 0f && v.y < 0f) a = 270f;
        else
        {
            a = atan(Mathf.Abs(v.y / v.x));
            if (v.x < 0f && v.y >= 0f) a = 180f - a;
            else if (v.x < 0f && v.y < 0f) a += 180f;
            else if (v.x >= 0f && v.y < 0f) a = 360f - a;
        }
        return a;
    }
    public static float atan360zy(Vector3 v)
    {
        float a = 0f;
        if (v.z == 0f && v.y == 0f) a = 0f;
        else if (v.z == 0f && v.y > 0f) a = 90f;
        else if (v.z == 0f && v.y < 0f) a = 270f;
        else
        {
            a = atan(Mathf.Abs(v.y / v.z));
            if (v.z < 0f && v.y >= 0f) a = 180f - a;
            else if (v.z < 0f && v.y < 0f) a += 180f;
            else if (v.z >= 0f && v.y < 0f) a = 360f - a;
        }
        return a;
    }
    public static float mod360(float f)
    {
        while (f >= 360f)
        {
            f -= 360;
        }
        while (f < 0f)
        {
            f += 360;
        }

        return f;
    }
}



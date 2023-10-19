using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HoopTransformCopy))]
[CanEditMultipleObjects]
public class TransformCopy : Editor
{

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying) return;
        DrawDefaultInspector();
        HoopTransformCopy myTarget = (HoopTransformCopy)target;
        if (myTarget == null) return;
        if(myTarget.PlayerHoopTransform == null) return;
        myTarget.PlayerHoopTransform.position = myTarget.transform.position;
        myTarget.PlayerHoopTransform.rotation = myTarget.transform.rotation;

        if(myTarget.OpponentHoopTransform == null) return;
        Vector3 p = myTarget.transform.position;
        p.x *= -1f;
        myTarget.OpponentHoopTransform.position = p;

        p = myTarget.transform.localEulerAngles;
        p.y *= -1f;
        myTarget.OpponentHoopTransform.localEulerAngles = p;
    }
}

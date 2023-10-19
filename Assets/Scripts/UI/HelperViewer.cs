using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperViewer : MonoBehaviour
{

    public LineRenderer lr;
    public Transform target;
    public Transform focus;
    private Throw th;
    private Transform correctPos;

    private void Start()
    {
        th = GameObject.Find("BallControllerForPlayer").GetComponent<Throw>();
        correctPos = transform.Find("CorrectPosition");
    }
    void FixedUpdate()
    {

        Vector3 v = correctPos.position + correctPos.right * th.HorizontalHelperLength;
        Vector3 v2 = correctPos.position - correctPos.right * th.HorizontalHelperLength;
        lr.SetPosition(0, v);
        lr.SetPosition(1, v2);

        target.position = th.targetPos;
        focus.position = th.focusPos;
    }


}

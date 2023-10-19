using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopVelocity : MonoBehaviour
{
    [HideInInspector] public Vector3 velocity;
    private Vector3 posNew, posOld;
    private Throw th;
    private void Start()
    {
        th = GameObject.Find("BallControllerForPlayer").GetComponent<Throw>();
        posNew = transform.position;
        posOld = transform.position;
    }
    private void FixedUpdate()
    {
        posNew = transform.position;
        velocity = (posNew - posOld) / Time.deltaTime;
        posOld = posNew;

        if (velocity.magnitude > 0.001f) th.Calculate_CorrectPosition();
    }
}

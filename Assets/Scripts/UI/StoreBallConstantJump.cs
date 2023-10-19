using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBallConstantJump : MonoBehaviour
{
    [Min(0f)] public float JumpVelocity = 1f;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up * 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.up * JumpVelocity;
    }
}

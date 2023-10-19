using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopBend : MonoBehaviour
{
    private Animator a;
    void Start()
    {
        a = transform.parent.GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        a.SetTrigger("Bend");
    }
}

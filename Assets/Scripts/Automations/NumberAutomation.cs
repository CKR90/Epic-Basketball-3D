using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberAutomation : MonoBehaviour
{
    void Start()
    {
        transform.up = transform.position - Camera.main.transform.position;
        transform.Rotate(transform.up, 180f);
        
        Destroy(transform.parent.gameObject, 2f);
    }
}

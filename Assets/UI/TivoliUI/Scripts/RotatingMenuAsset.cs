using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMenuAsset : MonoBehaviour
{
    
    void Update()
    {
        transform.Rotate(Vector3.back * 20 * Time.deltaTime, Space.Self);
    }
}

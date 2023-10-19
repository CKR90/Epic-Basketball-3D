using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraResolutionSettings : MonoBehaviour
{
    public Vector3 HDPosition;
    public Vector3 HDRotation;
    [Space(10)]
    public Vector3 FHDPosition;
    public Vector3 FHDRotation;

    void Start()
    {
        Resolution res = Screen.currentResolution;

        float width = res.width;
        float height = res.height;


        if (width <= 1400 && height <= 2000)
        {
            HD();
        }
        else if (width <= 1400 && height > 2000 && height <= 2500)
        {
            FHD();
        }
        else
        {
            HD();
        }
    }

    private void HD()
    {
        transform.position = HDPosition;
        transform.localEulerAngles = HDRotation;
    }
    private void FHD()
    {
        transform.position = FHDPosition;
        transform.localEulerAngles = FHDRotation;
    }
}

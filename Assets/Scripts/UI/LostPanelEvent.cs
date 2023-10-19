using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LostPanelEvent : MonoBehaviour
{
    public GameObject DoubleBar;
    public GameObject SingleBar;
    public GameObject OnBoard;
    void Start()
    {
        if (DoubleBar != null) Destroy(DoubleBar);
        if (SingleBar != null) Destroy(SingleBar);
        if (OnBoard != null) Destroy(OnBoard);
    }
}

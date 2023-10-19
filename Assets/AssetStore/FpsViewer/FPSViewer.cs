using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSViewer : MonoBehaviour
{
    public Text FPS;
    private float fpsTimer = 0f;
    private float fpsCount = 0f;
    private int fpsTimeCounter = 0;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        GetComponent<Canvas>().worldCamera = cam;

    }

    // Update is called once per frame
    void Update()
    {
        FPSView();
    }

    private void FPSView()
    {
        if (FPS == null) return;

        fpsCount += 1f / Time.deltaTime;
        fpsTimer += Time.deltaTime;
        fpsTimeCounter++;
        if (fpsTimer >= .5f)
        {

            fpsCount /= fpsTimeCounter;
            float ms = 1000f / fpsCount;
            FPS.text = fpsCount.ToString("0") + " FPS\n" + ms.ToString("0.0") + " ms";

            fpsTimer = 0f;
            fpsCount = 0f;
            fpsTimeCounter = 0;

        }

        FPS.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -Screen.height / 20f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarnShower : MonoBehaviour
{
    [HideInInspector] public int earn = 0;
    private TextMeshProUGUI tm;
    private RectTransform rt;
    private Vector3 target = new Vector3(250f, -400f, 0f);
    private Color firstColor;

    private bool start = false;
    void Start()
    {
        tm = GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();

        firstColor = tm.color;
    }
    void Update()
    {
        if (!start && earn > 0)
        {
            start = true;
            tm.SetText("-" + earn.ToString() + "$");
        }
        if (!start) return;

        
        Color c = tm.color;
        float a = c.a;
        a = Mathf.Lerp(a, 0f, Time.deltaTime / 3f);
        c.a = a;
        tm.color = c;

        rt.anchoredPosition3D = Vector3.Lerp(rt.anchoredPosition3D, target, Time.deltaTime * 4f);
        if (Vector3.Distance(rt.anchoredPosition3D, target) < 1f) Destroy(gameObject);
    }
}

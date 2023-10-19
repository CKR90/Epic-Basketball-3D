using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopColorPanel : MonoBehaviour
{
    [Min(1f)]
    public float flashMultiplier = 20f;
    [Range(0f, 1f)]
    public float maxOpaque = 1f;
    public List<Color> colors = new List<Color>();
    public bool test = false;

    private Material m;
    private bool animate = false;
    private int index = 0;
    private Color selected;
    private float a = 0f;
    private bool increase = true;

    void Start()
    {
        m = GetComponent<MeshRenderer>().sharedMaterials[0];
    }
    void Update()
    {
        if(test)
        {
            AnimateColor();
        }
        if (!animate) return;

        if(increase)
        {
            a += Time.deltaTime * flashMultiplier;
            if(a >= maxOpaque)
            {
                a = maxOpaque;
                increase = false;
            }
        }
        else
        {
            a -= Time.deltaTime * flashMultiplier;
            if(a <= 0f)
            {
                a = 0f;
                animate = false;
                test = false;
            }
        }
        selected.a = a;
        m.color = selected;
    }

    public void AnimateColor()
    {
        if (animate) return;
        if (colors.Count < 1) return;
        index = Random.Range(0, colors.Count - 1);
        selected = colors[index];
        selected.a = 0f;
        a = maxOpaque / 2f;
        increase = true;
        animate = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerRect : MonoBehaviour
{
    private bool t = false;
    private Vector3 target;
    private float v;
    private bool money = false;
    private WinEvent win;
    private int increase = 1;

    void Update()
    {
        if(t)
        {
            Vector3 pos = GetComponent<RectTransform>().anchoredPosition3D;
            pos = Vector3.MoveTowards(pos, target, Time.deltaTime * v);
            GetComponent<RectTransform>().anchoredPosition3D = pos;
            if (Vector3.Distance(pos, target) < .5f)
            {
                if (money) win.IncreaseMoney(increase);
                else win.IncreaseCup();
                Destroy(gameObject);
            }
        }
    }

    public void Throw(Vector3 startPoint, Vector3 endPoint, float velocity, bool isMoney, WinEvent getScript, int increaseValue)
    { 
        increase = increaseValue;
        target = endPoint;
        v = velocity;
        money = isMoney;
        win = getScript;
        GetComponent<RectTransform>().anchoredPosition3D = startPoint;
        t = true;
    }
}

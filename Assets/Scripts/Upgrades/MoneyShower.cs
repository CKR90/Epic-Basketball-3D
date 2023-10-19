using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyShower : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    [HideInInspector] public float add = 0f;
    [HideInInspector] public GesturesEvent gestures;
    private bool startAnim = false;
    private RectTransform r;

    private int switcher = 0;
    private float sinVal = 0f;
    private float timer = 0f;
    void Start()
    {
        r = GetComponent<RectTransform>();
        r.anchoredPosition3D = new Vector3(500f, -500f, 0f);
        r.localScale = Vector3.zero;
    }


    void Update()
    {
        StartCheck();
        if (!startAnim) return;

        switch(switcher)
        {
            case 0:
                sinVal += Time.deltaTime * 600f;
                SinScale();
                break;
            case 1:
                sinVal = 90f;
                switcher++;
                break;
            case 2:
                SinMove();
                SinScale();
                break;

        }
    }
    private void StartCheck()
    {
        if(add > 0f)
        {
            startAnim = true;
            moneyText.text = "+" + add.ToString();
        }
    }
    private void SinScale()
    {
        
        float val = Mat.sin(sinVal) * 2f;

        if (sinVal > 90f)
        {
            switcher++;
        }
        else
        {
            r.localScale = Vector3.one * val;
        }
    }
    private void Delay(float f)
    {
        timer += Time.deltaTime;
        if(timer >= f)
        {
            timer = 0f;
            switcher++;
        }
    }
    private void SinMove()
    {
        sinVal -= Time.deltaTime * 100f;
        sinVal = Mathf.Max(sinVal, 0f);

        float speed = 1f - Mat.sin(sinVal);

        r.anchoredPosition3D = Vector3.MoveTowards(r.anchoredPosition3D, Vector3.zero, Time.deltaTime * 5000f * speed);

        if(Vector3.Distance(r.anchoredPosition3D,Vector3.zero) < 1f)
        {
            int money = int.Parse(gestures.money.text);
            gestures.money.SetText((money + add).ToString());
            Destroy(gameObject);
        }
    }
}

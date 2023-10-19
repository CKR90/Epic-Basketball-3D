using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSkin : MonoBehaviour
{
    
    public GameObject MainMenu;
    public InputField input;

    private StoreButtonEvent StoreButtonEvent;
    void Start()
    {
        StoreButtonEvent = GameObject.FindObjectOfType<StoreButtonEvent>();
    }
    void Update()
    {
        
    }

    public void CheckInputField()
    {
        string text = input.text;
        if (string.IsNullOrEmpty(text)) return;
        if (text.Contains("-") || text.Contains("+") || text.Contains(".") || text.Contains(","))
        {
            input.text = "";
            return;
        }

        int i = int.Parse(text);
        if (i < 1) i = 1;
        if (i > 9) i = 9;
        input.text = i.ToString();
    }
    public void CheckEditEnd()
    {
        if (input.text == "") input.text = "1";

        int val = int.Parse(input.text);

        string text = "";

        for(int i = 0; i < val; i++)
        {
            text += i.ToString();
        }
        PlayerPrefs.SetString("UnlockedBallIndices", text);
        PlayerPrefs.SetInt("SelectedBallIndex", val - 1);
        StoreButtonEvent.GenerateStoreCells();
    }



    public void BackButton()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}

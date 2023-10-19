using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugChangeLevel : MonoBehaviour
{
    public GameObject MainMenu;
    public InputField input;

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
        if (i > 50) i = 50;
        input.text = i.ToString();
    }
    public void GoToLevel()
    {
        string text = input.text;
        if (string.IsNullOrEmpty(text)) return;

        int i = int.Parse(text);
        if (i < 1) i = 1;
        if (i > 50) i = 50;
        SceneManager.LoadScene(i);
    }
    public void BackButton()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}

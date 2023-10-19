using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private int currentLevel = 1;
    void Start()
    {
        
        if (PlayerPrefs.HasKey("level"))
        {
            currentLevel = PlayerPrefs.GetInt("level");
        }
        else
        {
            PlayerPrefs.SetInt("level", 1);
        }
        Invoke("Load_Scene", 3f);
    }

    private void Load_Scene()
    {
        SceneManager.LoadScene(currentLevel, LoadSceneMode.Single);
    }


}

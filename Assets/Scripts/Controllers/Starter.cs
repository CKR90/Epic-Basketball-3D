using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Starter : MonoBehaviour
{
    [SerializeField] private bool isDeveloper = false;
    public Material BallMaterial;
    public Sprite BallFirstSprite;
    [SerializeField] private GameObject Menu;
    private int level = 1;
    private string DBKey = "Key001";

    void Awake()
    {

        if (!PlayerPrefs.HasKey("DataBaseKey"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("DataBaseKey", DBKey);
        }
        else
        {
            string key = PlayerPrefs.GetString("DataBaseKey");
            if(key != DBKey)
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetString("DataBaseKey", DBKey);
            }
        }

        if (!PlayerPrefs.HasKey("First Play")) PlayerPrefs.SetInt("First Play", 1);
        if (!PlayerPrefs.HasKey("Score")) PlayerPrefs.SetInt("Score", 0);
        if (!PlayerPrefs.HasKey("Loop")) PlayerPrefs.SetInt("Loop", 0);
        if (!PlayerPrefs.HasKey("Level")) PlayerPrefs.SetInt("Level", 1);
        if (!PlayerPrefs.HasKey("Cups")) PlayerPrefs.SetInt("Cups", 0);
        if (!PlayerPrefs.HasKey("Money")) PlayerPrefs.SetInt("Money", 0);
        if (!PlayerPrefs.HasKey("OldCups")) PlayerPrefs.SetInt("OldCups", 0);
        if (!PlayerPrefs.HasKey("OldMoney")) PlayerPrefs.SetInt("OldMoney", 0);
        if (!PlayerPrefs.HasKey("TimeToRebuild")) PlayerPrefs.SetFloat("TimeToRebuild", 2f);
        if (!PlayerPrefs.HasKey("CompletedLevelCount")) PlayerPrefs.SetInt("CompletedLevelCount", 1);
        if (!PlayerPrefs.HasKey("CompletedRaceLevelCount")) PlayerPrefs.SetInt("CompletedRaceLevelCount", 1);
        if (!PlayerPrefs.HasKey("LastLevelToShowUpgrade")) PlayerPrefs.SetInt("LastLevelToShowUpgrade", 0);
        if (!PlayerPrefs.HasKey("UserName")) PlayerPrefs.SetString("UserName", "You");
        if (!PlayerPrefs.HasKey("Country")) PlayerPrefs.SetInt("Country", (int)Country.UnitedStates);
        if (!PlayerPrefs.HasKey("Unlock Ball"))
        {
            PlayerPrefs.SetInt("Unlock Ball", 0);
            BallMaterial.SetTexture("_MainTex", BallFirstSprite.texture);
        }
        if (!PlayerPrefs.HasKey("UnlockedBallIndices")) PlayerPrefs.SetString("UnlockedBallIndices", "0");
        if (!PlayerPrefs.HasKey("SelectedBallIndex")) PlayerPrefs.SetInt("SelectedBallIndex", 0);
        level = PlayerPrefs.GetInt("Level");

        if(isDeveloper) Menu.SetActive(true);
        else SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
    public void ResetConfig()
    {
        PlayerPrefs.SetInt("First Play", 1);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Loop", 0);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Cups", 0);
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("OldCups", 0);
        PlayerPrefs.SetInt("OldMoney", 0);
        PlayerPrefs.SetInt("CompletedLevelCount", 1);
        PlayerPrefs.SetInt("CompletedRaceLevelCount", 1);
        PlayerPrefs.SetInt("LastLevelToShowUpgrade", 0);
        PlayerPrefs.SetFloat("TimeToRebuild", 2f);
        PlayerPrefs.SetString("UserName", "You");
        PlayerPrefs.SetInt("Country", (int)Country.UnitedStates);
        PlayerPrefs.SetInt("Unlock Ball", 0);
        PlayerPrefs.SetString("UnlockedBallIndices", "0");
        PlayerPrefs.SetInt("SelectedBallIndex", 0);
        BallMaterial.SetTexture("_MainTex", BallFirstSprite.texture);
        level = 1;
    }
}

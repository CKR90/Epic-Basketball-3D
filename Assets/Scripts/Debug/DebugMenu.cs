using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    public Text Header;
    public GameObject MainMenu;
    public GameObject ChangeLevel;
    public GameObject CameraTransforms;
    public GameObject Skin;
    public GameObject HoopTransform;
    public GameObject CourtTransform;
    public GameObject CourtColor;
    public Button Button_HoopTransform;
    public GameObject FinalHoop;

    private StoreButtonEvent storeButtonEvent;
    private GameObject ballsButton;
    private GameObject onBoard;

    private Throw th;
    private ThrowBot tb;
    

    void Start()
    {
        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();

        Header.text = "Debug Menu\nLevel Index: " + buildIndex;

        storeButtonEvent = GameObject.FindObjectOfType<StoreButtonEvent>();
        ballsButton = GameObject.Find("Ball Skins Button");

        if(SceneManager.GetActiveScene().name.Contains("Skate"))
        {
            Button_HoopTransform.interactable = false;
        }

        CheckDatabase();
    }
    void Update()
    {
        
    }

    private void CheckDatabase()
    {
        th = GameObject.FindObjectOfType<Throw>();
        tb = GameObject.FindObjectOfType<ThrowBot>();

        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();

        if (!PlayerPrefs.HasKey("DefaultPlayerHoopTransforms_" + buildIndex))
        {
            string text = "";
            foreach (var v in th.HoopTransforms)
            {
                text += v.position.x.ToString("0.000") + "_" + v.position.y.ToString("0.000") + "_" + v.position.z.ToString("0.000") + "_"
                     + v.localEulerAngles.x.ToString("0.000") + "_" + v.localEulerAngles.y.ToString("0.000") + "_" + v.localEulerAngles.z.ToString("0.000") + "*";
            }
            PlayerPrefs.SetString("DefaultPlayerHoopTransforms_" + buildIndex, text);
        }
        if (!PlayerPrefs.HasKey("DefaultFinalHoopTransform_" + buildIndex))
        {
            string text = "";
            Transform v = FinalHoop.transform;
            text += v.position.x.ToString("0.000") + "_" + v.position.y.ToString("0.000") + "_" + v.position.z.ToString("0.000") + "_"
                     + v.localEulerAngles.x.ToString("0.000") + "_" + v.localEulerAngles.y.ToString("0.000") + "_" + v.localEulerAngles.z.ToString("0.000");
            PlayerPrefs.SetString("DefaultFinalHoopTransform_" + buildIndex, text);
        }
        if (!PlayerPrefs.HasKey("DefaultCameraTransform_" + buildIndex))
        {
            string text = "";
            Transform t = Camera.main.transform;
            text += t.position.y.ToString("0.000") + "_" + t.localEulerAngles.x.ToString("0.000");
            PlayerPrefs.SetString("DefaultCameraTransform_" + buildIndex, text);
        }
        if (!PlayerPrefs.HasKey("DefaultCourtTransform_" + buildIndex))
        {
            string text = "";
            Transform v = GameObject.Find("Court").transform;
            text += v.position.x.ToString("0.000") + "_" + v.position.y.ToString("0.000") + "_" + v.position.z.ToString("0.000") + "_"
                     + v.localEulerAngles.x.ToString("0.000") + "_" + v.localEulerAngles.y.ToString("0.000") + "_" + v.localEulerAngles.z.ToString("0.000");
            PlayerPrefs.SetString("DefaultCourtTransform_" + buildIndex, text);
        }
        if (!PlayerPrefs.HasKey("DefaultCourtMaterialName_" + buildIndex))
        {
            PlayerPrefs.SetString("DefaultCourtMaterialName_" + buildIndex, GameObject.Find("Backdrop").GetComponent<MeshRenderer>().sharedMaterial.name);
        }
    }

    public void EnableDebugMode()
    {
        storeButtonEvent.menuLock = true;
    }
    public void OpenDebugMenu()
    {
        MainMenu.SetActive(true);
        storeButtonEvent.TapToPlayText.SetActive(false);
        onBoard = GameObject.Find("OnBoard Panel");
        onBoard.SetActive(false);
        ballsButton.SetActive(false);
    }
    public void DisableDebugMode()
    {
        storeButtonEvent.menuLock = false;
        storeButtonEvent.TapToPlayText.SetActive(true);
        ballsButton.SetActive(true);
        onBoard.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void Open_ChangeLevel()
    {
        ChangeLevel.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void Open_Camera()
    {
        CameraTransforms.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void Open_Skin()
    {
        Skin.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void Open_HoopTransform()
    {
        HoopTransform.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void Open_CourtTransform()
    {
        CourtTransform.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void Open_CourtColor()
    {
        CourtColor.SetActive(true);
        MainMenu.SetActive(false);
    }
}

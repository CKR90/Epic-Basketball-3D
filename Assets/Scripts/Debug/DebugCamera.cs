using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugCamera : MonoBehaviour
{
    public GameObject MainMenu;
    public Slider sliderAngle;
    public Slider sliderHeight;
    public Text HeightText;
    public Text AngleText;
    private Transform cam;
    private Vector3 firstPos;
    private Vector3 firstRot;
    void Start()
    {
        cam = Camera.main.transform;
        firstPos = cam.position;
        firstRot = cam.localEulerAngles;
        HeightText.text = "Heigth\n" + (0f).ToString("0.00");
        AngleText.text = "Angle\n" + (0f).ToString("0.00");

    }

    public void Height(float val)
    {
        Vector3 pos = firstPos;
        pos.y += val * 5f;
        cam.position = pos;

        HeightText.text = "Heigth\n" + val.ToString("0.00");
    }
    public void Angle(float val)
    {
        Vector3 rot = firstRot;
        rot.x -= val * 70f;
        cam.localEulerAngles = rot;

        AngleText.text = "Angle\n" + val.ToString("0.00");
    }
    public void BackButton()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetDataBase()
    {
        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();
        string text = "";
        Transform t = Camera.main.transform;
        text += t.position.y.ToString("0.000") + "_" + t.localEulerAngles.x.ToString("0.000");
        PlayerPrefs.SetString("EditCameraTransform_" + buildIndex, text);
    }
    public void RestoreDefaults()
    {
        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();
        if (PlayerPrefs.HasKey("DefaultCameraTransform_" + buildIndex))
        {
            sliderAngle.value = 0f;
            sliderHeight.value = 0f;

            string text = PlayerPrefs.GetString("DefaultCameraTransform_" + buildIndex);
            List<string> data = text.Split('_').ToList();

            Vector3 pos = Camera.main.transform.position;
            Vector3 rot = Camera.main.transform.localEulerAngles;

            pos.y = float.Parse(data[0]);
            rot.x = float.Parse(data[1]);

            Camera.main.transform.position = pos;
            Camera.main.transform.localEulerAngles = rot;

            PlayerPrefs.DeleteKey("EditCameraTransform_" + buildIndex);
        }
    }
}

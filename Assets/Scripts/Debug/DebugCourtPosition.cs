using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugCourtPosition : MonoBehaviour
{
    public GameObject MainMenu;
    public Slider angleSlider;

    private Transform court;
    private Vector3 position;
    private Vector3 rotation;


    private bool forward = false;
    private bool right = false;
    private bool left = false;
    private bool back = false;
    void Start()
    {
        court = GameObject.Find("Court").transform;
        position = court.position;
        rotation = court.localEulerAngles;
    }

    private void Update()
    {
        Move_Forward();
        Move_Right();
        Move_Left();
        Move_Back();
    }


    public void Forward(bool enabled)
    {
        forward = enabled;
        if (!enabled) SetDataBase();
    }
    public void Right(bool enabled)
    {
        right = enabled;
        if (!enabled) SetDataBase();
    }
    public void Left(bool enabled)
    {
        left = enabled;
        if (!enabled) SetDataBase();
    }
    public void Back(bool enabled)
    {
        back = enabled;
        if (!enabled) SetDataBase();
    }
    public void RotateCourt(float f)
    {
        Vector3 rot = rotation;
        rot.y += (f * 180f);
        court.localEulerAngles = rot;
    }

    public void ResetTransform()
    {
        court.position = position;
        angleSlider.value = 0f;
    }
    public void BackButton()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void Move_Forward()
    {
        if (!forward || back) return;

        Vector3 pos = court.position;
        pos.z -= Time.deltaTime * 5f;
        court.position = pos;
    }
    private void Move_Right()
    {
        if (!right || left) return;

        Vector3 pos = court.position;
        pos.x -= Time.deltaTime * 5f;
        court.position = pos;
    }
    private void Move_Left()
    {
        if (!left || right) return;

        Vector3 pos = court.position;
        pos.x += Time.deltaTime * 5f;
        court.position = pos;
    }
    private void Move_Back()
    {
        if (!back || forward) return;

        Vector3 pos = court.position;
        pos.z += Time.deltaTime * 5f;
        court.position = pos;
    }

    public void SetDataBase()
    {
        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();
        string text = "";

        text += court.position.x.ToString("0.000") + "_" + court.position.y.ToString("0.000") + "_" + court.position.z.ToString("0.000") + "_" +
            court.localEulerAngles.x.ToString("0.000") + "_" + court.localEulerAngles.y.ToString("0.000") + "_" + court.localEulerAngles.z.ToString("0.000");
        PlayerPrefs.SetString("EditCourtTransform_" + buildIndex, text);
    }
    public void RestoreDefaults()
    {
        angleSlider.value = 0f;
        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();
        string text = PlayerPrefs.GetString("DefaultCourtTransform_" + buildIndex);
        List<string> data = text.Split('_').ToList();

        Vector3 pos = Vector3.zero;
        Vector3 euler = Vector3.zero;
        pos.x = float.Parse(data[0]);
        pos.y = float.Parse(data[1]);
        pos.z = float.Parse(data[2]);
        euler.x = float.Parse(data[3]);
        euler.y = float.Parse(data[4]);
        euler.z = float.Parse(data[5]);

        court.position = pos;
        court.localEulerAngles = euler;

        PlayerPrefs.DeleteKey("EditCourtTransform_" + buildIndex);
    }
}

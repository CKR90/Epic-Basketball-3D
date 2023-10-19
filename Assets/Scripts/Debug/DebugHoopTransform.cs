using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugHoopTransform : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ButtonPlayer;
    public GameObject ButtonFinal;
    public GameObject Restore;
    public GameObject ButtonForward;
    public GameObject ButtonRight;
    public GameObject ButtonLeft;
    public GameObject ButtonBack;
    public GameObject Slider;
    public GameObject SliderSensitivity;
    public GameObject SliderText;
    public GameObject bgImage1;
    public GameObject bgImage2;
    public GameObject TransformButton;

    private List<GameObject> buttons = new List<GameObject>();

    private Image ownImage;
    private Color bg;

    private bool modeMove = false;

    private GameObject PlayerHoop;
    private GameObject BotHoop;
    private GameObject FinalHoop;
    private Throw th;
    private ThrowBot tb;


    private bool forward = false;
    private bool right = false;
    private bool left = false;
    private bool back = false;

    private Transform selectedTransform_Player;
    private Transform selectedTransform_Bot;

    private bool movePlayerHoops = false;
    private int TransformIndex = -1;

    private List<Vector3> playerFirstEulers = new List<Vector3>();
    private Vector3 finalFirstEuler = Vector3.zero;


    void Start()
    {
        ownImage = GetComponent<Image>();
        bg = ownImage.color;


        PlayerHoop = GameObject.Find("Hoop Player");
        BotHoop = GameObject.Find("Hoop Bot");
        FinalHoop = GameObject.Find("Hoop Final");

        th = GameObject.FindObjectOfType<Throw>();
        tb = GameObject.FindObjectOfType<ThrowBot>();

        if (!th.ApplyFinalHoop) ButtonFinal.SetActive(false);

        for(int i = 0; i < th.HoopTransforms.Count; i++)
        {
            playerFirstEulers.Add(th.HoopTransforms[i].localEulerAngles);
        }
        if(th.ApplyFinalHoop)
        {
            finalFirstEuler = FinalHoop.transform.localEulerAngles;
        }
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

    private void Move_Forward()
    {
        if (!forward || back) return;

        if (movePlayerHoops)
        {
            movePlayerHoop(Vector3.forward * Time.deltaTime);
        }
        else moveFinalHoop(Vector3.forward * Time.deltaTime);
    }
    private void Move_Right()
    {
        if (!right || left) return;

        if (movePlayerHoops)
        {
            movePlayerHoop(Vector3.right * Time.deltaTime);
        }
        else moveFinalHoop(Vector3.right * Time.deltaTime);
    }
    private void Move_Left()
    {
        if (!left || right) return;

        if (movePlayerHoops)
        {
            movePlayerHoop(-Vector3.right * Time.deltaTime);
        }
        else moveFinalHoop(-Vector3.right * Time.deltaTime);
    }
    private void Move_Back()
    {
        if (!back || forward) return;

        if (movePlayerHoops)
        {
            movePlayerHoop(-Vector3.forward * Time.deltaTime);
        }
        else moveFinalHoop(-Vector3.forward * Time.deltaTime);
    }

    private void movePlayerHoop(Vector3 move)
    {

        if (TransformIndex < 0 || TransformIndex >= th.HoopTransforms.Count) return;

        move *= SliderSensitivity.GetComponent<Slider>().value;
        Vector3 posPlayer = th.HoopTransforms[TransformIndex].position;


        posPlayer.z += move.z;
        posPlayer.x += move.x;
        if (BotHoop != null && posPlayer.x < 0f)
        {
            posPlayer.x = 0f;
        }

        th.HoopTransforms[TransformIndex].position = posPlayer;
        PlayerHoop.transform.position = posPlayer;

        if(BotHoop != null)
        {
            posPlayer.x *= -1f;
            tb.HoopTransforms[TransformIndex].position = posPlayer;
            BotHoop.transform.position = posPlayer;
        }
        
    }
    private void moveFinalHoop(Vector3 move)
    {
        move *= SliderSensitivity.GetComponent<Slider>().value;
        Vector3 pos = FinalHoop.transform.position;
        pos += move;
        FinalHoop.transform.position = pos;
    }

    public void BackButton()
    {
        if (!modeMove)
        {
            MainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            ItemButton_OnClick(0);
            modeMove = false;
            ShowMenu();
        }
    }

    public void MovePlayers()
    {
        InitializeTransformButtons();
        modeMove = true;
        movePlayerHoops = true;
        HideMenu();
        
    }
    public void MoveFinal()
    {
        modeMove = true;
        movePlayerHoops = false;
        HideMenu();
    }


    public void RotateSlider(float val)
    {
        if (movePlayerHoops) RotatePlayers(val);
        else RotateFinal(val);
    }

    private void RotatePlayers(float val)
    {
        Vector3 rot = playerFirstEulers[TransformIndex];
        rot.y += val * 180f;

        th.HoopTransforms[TransformIndex].localEulerAngles = rot;
        PlayerHoop.transform.localEulerAngles = rot;

        if (BotHoop != null)
        {
            rot.y *= -1f;
            tb.HoopTransforms[TransformIndex].localEulerAngles = rot;
            BotHoop.transform.localEulerAngles = rot;
        }
    }
    private void RotateFinal(float val)
    {
        Vector3 rot = finalFirstEuler;
        rot.y += val * 180f;

        FinalHoop.transform.localEulerAngles = rot;
    }

    private void ShowMenu()
    {
        ButtonPlayer.SetActive(true);
        if (th.ApplyFinalHoop) ButtonFinal.SetActive(true);
        Restore.SetActive(true);
        ButtonForward.SetActive(false);
        ButtonRight.SetActive(false);
        ButtonLeft.SetActive(false);
        ButtonBack.SetActive(false);
        Slider.SetActive(false);
        SliderSensitivity.SetActive(false);
        SliderText.SetActive(false);
        bgImage1.SetActive(false);
        bgImage2.SetActive(false);
        ownImage.color = bg;

        for (int i = buttons.Count - 1; i >= 0; i--)
        {
            Destroy(buttons[i]);
        }
        buttons.Clear();
    }
    private void HideMenu()
    {
        ButtonPlayer.SetActive(false);
        ButtonFinal.SetActive(false);
        Restore.SetActive(false);
        ButtonForward.SetActive(true);
        ButtonRight.SetActive(true);
        ButtonLeft.SetActive(true);
        ButtonBack.SetActive(true);
        Slider.SetActive(true);
        SliderSensitivity.SetActive(true);
        SliderText.SetActive(true);
        bgImage1.SetActive(true);
        bgImage2.SetActive(true);
        ownImage.color = new Color(0f, 0f, 0f, 0f);


    }

    private void InitializeTransformButtons()
    {
        for (int i = 0; i < th.HoopTransforms.Count; i++)
        {
            GameObject g = Instantiate(TransformButton, transform);
            g.SetActive(true);
            g.name = "Button Pos " + i.ToString();
            Vector3 anchor = g.GetComponent<RectTransform>().anchoredPosition3D;
            anchor.x = i * 200f;

            anchor.y = -Mathf.Floor(anchor.x / 1000f) * 200f - 100f;
            anchor.x = anchor.x % 1000f;

            g.GetComponent<RectTransform>().anchoredPosition3D = anchor;

            g.transform.GetChild(0).GetComponent<Text>().text = "Pos\n" + (i + 1).ToString();
            AddTrigerEvents(g, i);
            buttons.Add(g);
        }
        ItemButton_OnClick(0);
    }
    private void AddTrigerEvents(GameObject buttonObject, int Parameter)
    {
        EventTrigger trigger = buttonObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerClick;
        pointerDown.callback.AddListener((e) => ItemButton_OnClick(Parameter));
        trigger.triggers.Add(pointerDown);
    }

    private void ItemButton_OnClick(int i)
    {
        TransformIndex = i;

        selectedTransform_Player = th.HoopTransforms[i];
        PlayerHoop.transform.position = selectedTransform_Player.position;
        PlayerHoop.transform.localEulerAngles = selectedTransform_Player.localEulerAngles;

        if(BotHoop != null)
        {
            selectedTransform_Bot = tb.HoopTransforms[i];
            BotHoop.transform.position = selectedTransform_Bot.position;
            BotHoop.transform.localEulerAngles = selectedTransform_Bot.localEulerAngles;
        }
        for (int y = 0; y < buttons.Count; y++)
        {
            if (y == i)
            {
                buttons[y].GetComponent<Image>().color = Color.green;
            }
            else
            {
                buttons[y].GetComponent<Image>().color = Color.white;
            }
        }

    }

    public void SetDataBase()
    {
        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();
        string text = "";
        if (movePlayerHoops)
        {
            foreach (var v in th.HoopTransforms)
            {
                text += v.position.x.ToString("0.000") + "_" + v.position.y.ToString("0.000") + "_" + v.position.z.ToString("0.000") + "_"
                     + v.localEulerAngles.x.ToString("0.000") + "_" + v.localEulerAngles.y.ToString("0.000") + "_" + v.localEulerAngles.z.ToString("0.000") + "*";
            }
            PlayerPrefs.SetString("EditPlayerHoopTransforms_" + buildIndex, text);
        }
        else
        {
            if(FinalHoop != null)
            {
                Transform v = FinalHoop.transform;
                text += v.position.x.ToString("0.000") + "_" + v.position.y.ToString("0.000") + "_" + v.position.z.ToString("0.000") + "_"
                         + v.localEulerAngles.x.ToString("0.000") + "_" + v.localEulerAngles.y.ToString("0.000") + "_" + v.localEulerAngles.z.ToString("0.000");
                PlayerPrefs.SetString("EditFinalHoopTransform_" + buildIndex, text);
            }
        }
    }

    public void RestoreDefaults()
    {
        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();

        if (PlayerPrefs.HasKey("DefaultPlayerHoopTransforms_" + buildIndex))
        {
            string text = PlayerPrefs.GetString("DefaultPlayerHoopTransforms_" + buildIndex);

            List<string> list = text.Split('*').ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (!string.IsNullOrEmpty(list[i]))
                {
                    List<string> data = list[i].Split('_').ToList();
                    Vector3 pos = Vector3.zero;
                    Vector3 euler = Vector3.zero;
                    pos.x = float.Parse(data[0]);
                    pos.y = float.Parse(data[1]);
                    pos.z = float.Parse(data[2]);
                    euler.x = float.Parse(data[3]);
                    euler.y = float.Parse(data[4]);
                    euler.z = float.Parse(data[5]);

                    th.HoopTransforms[i].position = pos;
                    th.HoopTransforms[i].localEulerAngles = euler;

                    if(BotHoop != null)
                    {
                        pos.x *= -1f;
                        euler.y *= -1f;
                        tb.HoopTransforms[i].position = pos;
                        tb.HoopTransforms[i].localEulerAngles = euler;
                    }
                }
            }
            PlayerHoop.transform.position = th.HoopTransforms[0].position;
            PlayerHoop.transform.localEulerAngles = th.HoopTransforms[0].localEulerAngles;

            if(BotHoop != null)
            {
                BotHoop.transform.position = tb.HoopTransforms[0].position;
                BotHoop.transform.localEulerAngles = tb.HoopTransforms[0].localEulerAngles;
            }
        }

        if(FinalHoop != null)
        {
            if (PlayerPrefs.HasKey("DefaultFinalHoopTransform_" + buildIndex))
            {
                string text = PlayerPrefs.GetString("DefaultFinalHoopTransform_" + buildIndex);
                List<string> data = text.Split('_').ToList();

                Vector3 pos = Vector3.zero;
                Vector3 euler = Vector3.zero;
                pos.x = float.Parse(data[0]);
                pos.y = float.Parse(data[1]);
                pos.z = float.Parse(data[2]);
                euler.x = float.Parse(data[3]);
                euler.y = float.Parse(data[4]);
                euler.z = float.Parse(data[5]);

                FinalHoop.transform.position = pos;
                FinalHoop.transform.localEulerAngles = euler;
            }
        }
        PlayerPrefs.DeleteKey("EditPlayerHoopTransforms_" + buildIndex);
        PlayerPrefs.DeleteKey("EditFinalHoopTransforms_" + buildIndex);

    }
}

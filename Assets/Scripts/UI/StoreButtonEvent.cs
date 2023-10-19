using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class StoreButtonEvent : MonoBehaviour
{
    public GameObject TapToPlayText;
    public GameObject UpgradeCanvas;
    public GameObject StoreBall;
    public RectTransform SkinPanel;
    public RectTransform OnboardPanel;
    public Transform StoreBallInstantiateTransform;
    public Transform CameraDefaultPosition;
    public Transform CameraStorePosition;
    public RectTransform Button;
    public Material BallMaterial;
    public List<BallButton> Balls;

    private GameObject ball;
    private GameObject debugMenuButton;


    [HideInInspector] public bool menuLock = false;
    private bool buttonDown = false;
    void Awake()
    {
        debugMenuButton = GameObject.Find("Debug Menu Button");
        GenerateStoreCells();
    }
    void Update()
    {
        if (!menuLock)
        {
            if (Input.GetMouseButton(0) && !buttonDown)
            {
                buttonDown = true;
            }
            else if (!Input.GetMouseButton(0) && buttonDown)
            {
                buttonDown = false;
                if (!menuLock) TapToPlay();
            }
        }
        else
        {
            buttonDown = false;
        }
    }


    public void GenerateStoreCells()
    {
        string unlocks = PlayerPrefs.GetString("UnlockedBallIndices");
        int selectedBall = PlayerPrefs.GetInt("SelectedBallIndex");
        BallMaterial.SetTexture("_MainTex", Balls[selectedBall].MaterialTexture);

        for (int i = 0; i < Balls.Count; i++)
        {
            Balls[i].Icon.sprite = Balls[i].IconSprite;

            if (unlocks.Contains(i.ToString())) BallCell_Activate(i);
            else BallCell_DeActivate(i);

            if (selectedBall == i) BallCell_Select(i);
            else BallCell_Deselect(i);

        }
    }
    public void GoToStoreEvent()
    {
        menuLock = true;
        TapToPlayText.SetActive(false);
        Button.DOAnchorPos3DX(-750f, .3f);
        OnboardPanel.DOAnchorPos3DY(700f, .3f);
        SkinPanel.DOAnchorPos3DY(0f, .5f).SetEase(Ease.OutSine);
        Camera.main.transform.DOMove(CameraStorePosition.position, .5f).SetEase(Ease.OutSine);
        Camera.main.transform.DORotate(CameraStorePosition.localEulerAngles, .5f).SetEase(Ease.OutSine);

        InstantiateStoreBall();
    }
    public void TapToPlay()
    {
        if (debugMenuButton != null) Destroy(debugMenuButton);

        Destroy(TapToPlayText);
        Button.DOAnchorPos3DX(-750, .15f).OnComplete(OpenUpgradeCanvasAndDestroyOwn);
    }
    public void SelectBall(int index)
    {
        int oldIndex = PlayerPrefs.GetInt("SelectedBallIndex");

        if (oldIndex == index) return;

        BallCell_Select(index);
        PlayerPrefs.SetInt("SelectedBallIndex", index);
        BallMaterial.SetTexture("_MainTex", Balls[index].MaterialTexture);
        ball.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        ball.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }
    public void ReturnButton()
    {
        menuLock = false;
        TapToPlayText.SetActive(true);
        Button.DOAnchorPos3DX(0f, .3f);
        OnboardPanel.DOAnchorPos3DY(0f, .3f);
        SkinPanel.DOAnchorPos3DY(-1500f, .5f).SetEase(Ease.InSine);

        GameObject debugCanvas = GameObject.Find("DebugCanvas");
        if(debugCanvas != null)
        {
            string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();
            if (PlayerPrefs.HasKey("EditCameraTransform_" + buildIndex))
            {
                string text = PlayerPrefs.GetString("EditCameraTransform_" + buildIndex);
                List<string> data = text.Split('_').ToList();

                Vector3 pos = Camera.main.transform.position;
                Vector3 rot = Camera.main.transform.localEulerAngles;

                pos.y = float.Parse(data[0]);
                rot.x = float.Parse(data[1]);

                Camera.main.transform.DOMove(pos, .5f).SetEase(Ease.OutSine);
                Camera.main.transform.DORotate(rot, .5f).SetEase(Ease.OutSine);
            }
            else
            {
                Camera.main.transform.DOMove(CameraDefaultPosition.position, .5f).SetEase(Ease.OutSine);
                Camera.main.transform.DORotate(CameraDefaultPosition.localEulerAngles, .5f).SetEase(Ease.OutSine);
            }
        }
        else
        {
            Camera.main.transform.DOMove(CameraDefaultPosition.position, .5f).SetEase(Ease.OutSine);
            Camera.main.transform.DORotate(CameraDefaultPosition.localEulerAngles, .5f).SetEase(Ease.OutSine);
        }
        DestroyBallStepByStep();
    }
    private void DestroyBallStepByStep()
    {
        Destroy(ball.transform.GetChild(1).gameObject);
        Destroy(ball.GetComponent<AudioSource>());
        ball.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(ball.GetComponent<Collider>());
        Destroy(ball, 2f);
        ball.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        ball.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    private void InstantiateStoreBall()
    {
        ball = Instantiate(StoreBall);
        ball.name = "Store Ball";
        ball.transform.position = StoreBallInstantiateTransform.position;
        ball.transform.rotation = StoreBallInstantiateTransform.rotation;
    }
    private void OpenUpgradeCanvasAndDestroyOwn()
    {
        UpgradeCanvas.SetActive(true);
        Destroy(gameObject);
        Destroy(SkinPanel.gameObject);
    }
    private void BallCell_Select(int index)
    {
        for (int i = 0; i < Balls.Count; i++)
        {
            BallCell_Deselect(i);
        }

        Balls[index].Active.enabled = true;
        Balls[index].Tick.enabled = true;
    }
    private void BallCell_Deselect(int index)
    {
        Balls[index].Active.enabled = false;
        Balls[index].Tick.enabled = false;
    }
    private void BallCell_Activate(int index)
    {
        Balls[index].Deactive.enabled = false;
        Balls[index].button.interactable = true;
    }
    private void BallCell_DeActivate(int index)
    {
        Balls[index].Active.enabled = false;
        Balls[index].Tick.enabled = false;
        Balls[index].Deactive.enabled = true;
        Balls[index].button.interactable = false;
    }
}

[System.Serializable]
public class BallButton
{
    public string Name;
    public Button button;
    public Image Active;
    public Image Icon;
    public Sprite IconSprite;
    public Image Deactive;
    public Image Tick;
    public Texture MaterialTexture;
}
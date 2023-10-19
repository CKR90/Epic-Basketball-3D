using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GesturesEvent : MonoBehaviour
{
    public Throw th;
    public GameObject onboard;
    public GameObject upgrade;
    public GameObject Gestures_UI_TapFirst;
    public GameObject Gestures_UI_Tap;
    public GameObject Gestures_UI;
    public RectTransform singleProgress;
    public RectTransform doubleProgress;
    public List<GameObject> childGestures;
    [Space(10)]
    public RectTransform moneyPanel;
    public RectTransform cupPanel;
    public GameObject levelPanel;
    public TextMeshProUGUI money;
    public TextMeshProUGUI cup;
    public TextMeshProUGUI level;
    [Space(10)]
    public GameObject MoneyAddShow;

    private Vector3 singleOldPos;
    private Vector3 doubleOldPos;
    private bool isPlay = false;
    private bool isStart = false;
    private bool lerpOnboard = false;
    private bool initializeComplete = false;

    void Start()
    {
        if (singleProgress != null)
        {
            singleOldPos = singleProgress.anchoredPosition3D;
            singleProgress.anchoredPosition3D = singleOldPos + Vector3.up * 500f;
        }
        if (doubleProgress != null)
        {
            doubleOldPos = doubleProgress.anchoredPosition3D;
            doubleProgress.anchoredPosition3D = doubleOldPos + Vector3.up * 500f;
        }
        th.gestures = this;

        money.SetText(PlayerPrefs.GetInt("OldMoney").ToString());
        cup.SetText(PlayerPrefs.GetInt("OldCups").ToString());
        level.SetText("Level " + PlayerPrefs.GetInt("CompletedLevelCount").ToString());
        onboard.SetActive(true);
    }
    private void Update()
    {
        if (upgrade != null) return;
        Gestures_Setup();
        CheckPlay();
        LerpProgress();
        LerpOnboard();
    }
    private void Gestures_Setup()
    {
        if (isStart) return;
        isStart = true;

        if (th.ControlMethod == Controls.Tap && SceneManager.GetActiveScene().buildIndex == 1 && PlayerPrefs.GetInt("Loop") == 0)
        {
            Gestures_UI_TapFirst.SetActive(true);
        }
        else if (th.ControlMethod == Controls.Tap)
        {
            Gestures_UI_Tap.SetActive(true);
        }
        else
        {
            Gestures_UI.SetActive(true);
        }
    }
    private void CheckPlay()
    {
        if (initializeComplete) return;
        isPlay = true;
        foreach (var v in childGestures)
        {
            if (v.gameObject.activeSelf) isPlay = false;
        }
        if (isPlay && onboard != null && th.TapToPlay)
        {
            Destroy(levelPanel);
            lerpOnboard = true;
        }
    }
    private void LerpProgress()
    {
        if (!isPlay || initializeComplete) return;
        if (singleProgress != null)
        {
            Vector3 pos = singleProgress.anchoredPosition3D;
            singleProgress.anchoredPosition3D = Vector3.Lerp(pos, singleOldPos, Time.deltaTime * 10f);
        }
        if (doubleProgress != null)
        {
            Vector3 pos = doubleProgress.anchoredPosition3D;
            doubleProgress.anchoredPosition3D = Vector3.Lerp(pos, doubleOldPos, Time.deltaTime * 10f);
        }
    }
    private void LerpOnboard()
    {
        if (initializeComplete || !lerpOnboard) return;

        moneyPanel.localScale = Vector3.Lerp(moneyPanel.localScale, new Vector3(.7f, .7f, .7f), Time.deltaTime * 10f);
        moneyPanel.anchoredPosition3D = Vector3.Lerp(moneyPanel.anchoredPosition3D, new Vector3(110f, -35f, 0f), Time.deltaTime * 10f);

        cupPanel.localScale = Vector3.Lerp(cupPanel.localScale, Vector3.one * .7f, Time.deltaTime * 10f);
        cupPanel.anchoredPosition3D = Vector3.Lerp(cupPanel.anchoredPosition3D, new Vector3(-55f, -35f, 0f), Time.deltaTime * 10f);

        if (Vector3.Distance(cupPanel.anchoredPosition3D, new Vector3(-55f, -35f, 0f)) < .01f)
        {
            initializeComplete = true;
        }
    }
    public void AddMoney(int money)
    {
        GameObject g = Instantiate(MoneyAddShow, onboard.transform);
        g.GetComponent<MoneyShower>().add = money;
        g.GetComponent<MoneyShower>().gestures = this;
    }
}

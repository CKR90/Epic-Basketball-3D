using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class WinEvent : MonoBehaviour
{
    #region Variables
    public bool showEarn = true;
    public bool showRanking = true;
    public bool showUnlockScreen = true;
    public Image DarkerPanel;
    public Throw t;
    public GameObject RankingPanel;
    public GameObject UnlockPanel;
    public Image BackGround;
    public GameObject Emoji;
    public Color BackgroundColor;
    public Text Score;
    public Text Percent;
    public Image BallLoader;
    public GameObject NextButton;
    [Space(10f)]
    public GameObject progressbarSingle;
    public GameObject progressbarDouble;
    [Space(10f)]
    public TextMeshProUGUI totalMoney;
    public TextMeshProUGUI totalCups;
    public TextMeshProUGUI earnMoney;
    public TextMeshProUGUI earnCups;
    public TextMeshProUGUI comboCount;
    public GameObject throwMoney;
    public GameObject throwCup;
    [Space(10f)]
    public Vector3 ThrowMoneyStart;
    public Vector3 ThrowMoneyEnd;
    public Vector3 ThrowCupStart;
    public Vector3 ThrowCupEnd;
    [Space(10f)]
    public Transform rankMask;
    public GameObject RankingList;
    public GameObject EarnPanel;
    public GameObject rankCellTemplate;
    public Color myCellColor;
    [Space(10f)]
    public Image UnlockBall;
    public GameObject NewBallUnlockLighter;
    public Material BallMaterial;
    public List<Sprite> UnlockBallTextures;
    public List<Sprite> BallSprites;
    [Space(10)]
    public List<User> otherUsers;
    [Space(10)]
    [Header("Flag Sprites", order = 10)]
    public Sprite flagBelgium;
    public Sprite flagBrazil;
    public Sprite flagGermany;
    public Sprite flagJapan;
    public Sprite flagFrance;
    public Sprite flagNetherlands;
    public Sprite flagSpain;
    public Sprite flagTurkey;
    public Sprite flagUnitedKingdom;
    public Sprite flagUnitedStates;
    [Space(10)]
    public AudioSource MoneyCount;
    public AudioSource MoneyRegister;
    public AudioSource MoneyBell;
    public AudioSource UnlockBallSoundEffect;
    public GameObject NewBallUnlockText;

    private List<AudioSource> MoneyBells;
    private int bellIndex = 0;

    private int switcher = 0;
    private float timer = 0f;

    private float score = 0f;
    [HideInInspector] public int targetScore = 0;
    private int ConstantWinValue = 10;
    [HideInInspector] public int unlockBallIndex = 0;
    private Dictionary<Country, Sprite> flags = new Dictionary<Country, Sprite>();
    private int cups = 0;
    private int oldCups = 0;
    private int money = 0;
    private int oldMoney = 0;
    private int earnedMoney = 0;
    private int earnedCups = 0;
    private bool isEarnShow = false;
    private bool isRankingShow = false;
    private bool isUnlockShow = false;
    private Vector3 myCellLocalScale = Vector3.one;
    private User user = new User();


    private float sin = 0f;
    private bool isMoneyThrowComplete = false;
    private bool isCupThrowComplete = false;
    private float moneyTimer = 0f;
    private float cupTimer = 0f;
    private float moneyTime = .3f;
    private float moneyCountScaler = 1f;
    private float cupTime = .3f;
    private int totalThrowedMoney = 0;
    private int totalThrowedCups = 0;

    private float myRectHeight = 0f;
    private int myNewListIndex = 0;
    private int myNewIndex = 0;
    private int totalUnderCells = 0;
    private int totalUpperCells = 0;
    private int totalCellMovements = 0;
    private int totalFinishMovements = 0;

    public HoopAudio WinnerBallHoopAudio;

    [HideInInspector] public int newUnlockBallIndex = -1;
    [HideInInspector] public int selectedBallIndex = 0;
    #endregion

    void Start()
    {
        if (showRanking || showEarn)
        {
            GameObject.Find("OnBoard Panel").SetActive(false);
        }

        money = PlayerPrefs.GetInt("Money");
        oldMoney = PlayerPrefs.GetInt("OldMoney");
        totalMoney.SetText(oldMoney.ToString());
        earnedMoney = money - oldMoney;
        earnMoney.SetText(earnedMoney.ToString());

        oldCups = PlayerPrefs.GetInt("OldCups");
        cups = PlayerPrefs.GetInt("Cups");
        cups += (int)Mathf.Clamp((money - oldMoney) / 5, 0, 10);
        totalCups.SetText(oldCups.ToString());
        earnedCups = cups - oldCups;
        earnCups.SetText(earnedCups.ToString());


        int texIndex = PlayerPrefs.GetInt("Unlock Ball") % BallSprites.Count;
        score = PlayerPrefs.GetInt("Score");
        targetScore = PlayerPrefs.GetInt("Score") + ConstantWinValue;
        targetScore %= 100;

        unlockBallIndex = PlayerPrefs.GetInt("Unlock Ball");
        UnlockBall.sprite = UnlockBallTextures[(unlockBallIndex + 1) % UnlockBallTextures.Count];
        selectedBallIndex = PlayerPrefs.GetInt("SelectedBallIndex");


        //BallMaterial.SetTexture("_MainTex", BallSprites[unlockBallIndex].texture);
        if (t.comboCount > 0) comboCount.SetText("Combo x" + t.comboCount.ToString());
        else comboCount.SetText("Level Cup");
        if (showRanking)
        {
            UpdateFlags();
            SetPlayerRanks();
            CreateOwnRankingCell();
            CreateOtherCells();
        }


        for (int i = 0; i < 5; i++)
        {
            MoneyBells = new List<AudioSource>();
            AudioSource a = gameObject.AddComponent<AudioSource>();
            a.playOnAwake = false;
            a.volume = MoneyBell.volume;
            a.clip = MoneyBell.clip;
            MoneyBells.Add(a);
        }
    }
    void Update()
    {
        Switcher();
    }
    private void UpdateFlags()
    {
        flags.Clear();
        flags.Add(Country.Belgium, flagBelgium);
        flags.Add(Country.Brazil, flagBrazil);
        flags.Add(Country.France, flagFrance);
        flags.Add(Country.Germany, flagGermany);
        flags.Add(Country.Japan, flagJapan);
        flags.Add(Country.Netherlands, flagNetherlands);
        flags.Add(Country.Spain, flagSpain);
        flags.Add(Country.Turkey, flagTurkey);
        flags.Add(Country.UnitedKingdom, flagUnitedKingdom);
        flags.Add(Country.UnitedStates, flagUnitedStates);
    }
    private void CreateOwnRankingCell()
    {
        int rankNumber = otherUsers.IndexOf(user) + 1;

        if (user.cell == null)
        {
            user.cell = Instantiate(rankCellTemplate, rankMask);
            user.cell.transform.Find("Flag").GetComponent<Image>().sprite = flags[user.country];
            user.cell.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(PlayerPrefs.GetString("UserName"));
            user.cell.transform.Find("Rank Number").GetComponent<TextMeshProUGUI>().SetText(rankNumber.ToString());
            user.cell.transform.Find("Cup Number").GetComponent<TextMeshProUGUI>().SetText(cups.ToString());
            user.cell.GetComponent<Image>().color = myCellColor;
            myCellLocalScale = user.cell.GetComponent<RectTransform>().localScale;
        }

        int myIndex = otherUsers.FindIndex(x => x == user);
        int underPlayerCount = otherUsers.Count - myIndex - 1;
        if (underPlayerCount < 3)
        {
            myRectHeight = -105f + underPlayerCount * 15f;
        }
        else if (myIndex < 4)
        {
            myRectHeight = -(15f * myIndex);
        }
        else
        {
            myRectHeight = -60f;
        }

        Vector2 ap = user.cell.GetComponent<RectTransform>().anchoredPosition;
        ap.y = myRectHeight;
        user.cell.GetComponent<RectTransform>().anchoredPosition = ap;
    }
    private void CreateOtherCells()
    {
        int myIndex = otherUsers.FindIndex((x) => x == user);
        for (int i = 0; i < 8; i++)
        {
            if ((myIndex + i + 1) < otherUsers.Count)
            {
                float thisRectHeight = myRectHeight - ((i + 1) * 15f);
                if (thisRectHeight <= -115f || thisRectHeight > 5) break;

                User u = otherUsers[myIndex + i + 1];
                if (u.cell == null)
                {
                    u.cell = Instantiate(rankCellTemplate, rankMask);
                    u.cell.transform.Find("Flag").GetComponent<Image>().sprite = flags[u.country];
                    u.cell.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(u.name);
                    u.cell.transform.Find("Rank Number").GetComponent<TextMeshProUGUI>().SetText((myIndex + i + 2).ToString());
                    u.cell.transform.Find("Cup Number").GetComponent<TextMeshProUGUI>().SetText(u.cups.ToString());
                }
                Vector2 p = u.cell.GetComponent<RectTransform>().anchoredPosition;
                p.y = thisRectHeight;
                u.cell.GetComponent<RectTransform>().anchoredPosition = p;
            }
            else
            {
                break;
            }
        }


        for (int i = 0; i < 8; i++)
        {
            if ((myIndex - i - 1) >= 0)
            {
                float thisRectHeight = myRectHeight + ((i + 1) * 15f);
                if (thisRectHeight <= -115f || thisRectHeight > 5) break;

                User u = otherUsers[myIndex - i - 1];
                u.cell = Instantiate(rankCellTemplate, rankMask);
                u.cell.transform.Find("Flag").GetComponent<Image>().sprite = flags[u.country];
                u.cell.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(u.name);
                u.cell.transform.Find("Rank Number").GetComponent<TextMeshProUGUI>().SetText((myIndex - i).ToString());
                u.cell.transform.Find("Cup Number").GetComponent<TextMeshProUGUI>().SetText(u.cups.ToString());
                Vector2 p = u.cell.GetComponent<RectTransform>().anchoredPosition;
                p.y = thisRectHeight;
                u.cell.GetComponent<RectTransform>().anchoredPosition = p;
            }
            else
            {
                break;
            }
        }
        user.cell.transform.SetAsLastSibling();
    }
    private void SetPlayerRanks()
    {
        user.name = PlayerPrefs.GetString("UserName");
        user.country = (Country)PlayerPrefs.GetInt("Country");
        user.cups = oldCups;
        List<User> ranks = otherUsers.OrderByDescending(x => x.cups).ToList();

        otherUsers = new List<User>();
        otherUsers = ranks;

        int? index = otherUsers.FindIndex(x => x.cups <= user.cups);
        if (index != null && index.Value >= 0 && index.Value < otherUsers.Count)
        {
            otherUsers.Insert(index.Value, user);
        }
        else
        {
            otherUsers.Add(user);
        }
    }
    private void Switcher()
    {
        switch (switcher)
        {
            case 0:
                Delay(1.5f);
                break;
            case 1:
                BackgroundLerp();
                break;
            case 2:
                ChoosePanel();
                break;
            //-------------------------------------------------
            case 50:
                OpenRankingPanel();
                break;
            case 51:
                Lerp_ProgressBars();
                LerpRanking();
                break;
            case 52:
                Delay(.5f);
                break;
            case 53:
                CalculateMoneyAndCupThrowFrequenceTime();
                break;
            case 54:
                MoneyThrow();
                CupThrow();
                CheckThrowComplete();
                break;
            case 55:
                Delay(.5f);
                break;
            case 56:
                CheckOtherPanelsAfterEarn();
                break;
            case 57:
                isRankingShow = true;
                OpenRankingList();
                break;
            case 58:
                Lerp_RankingList();
                break;
            case 59:
                Delay(.1f);
                break;
            case 60:
                CalculateNewRanks();
                break;
            case 61:
                RankCells();
                break;
            case 62:
                Delay(.1f);
                break;
            case 63:
                Open_NextButton();
                break;
            //--------------------------------------------------------------
            case 100:
                isRankingShow = true;
                OpenRankingPanel();
                OpenRankingListOnZero();
                break;
            case 101:
                Lerp_ProgressBars();
                LerpRanking();
                break;
            case 102:
                CalculateMoneyAndCupThrowFrequenceTime();
                break;
            case 103:
                MoneyThrow(true);
                CupThrow(true);
                //MoneyHeaderCounter();
                //CupHeaderCounter();
                CheckThrowComplete();
                break;
            case 104:
                CalculateNewRanks();
                break;
            case 105:
                RankCells();
                break;
            case 106:
                Delay(.1f);
                break;
            case 107:
                Open_NextButton();
                break;

            //--------------------------------------------------------------
            case 150:
                CloseRankingPanel(0);
                OpenUnlockPanel();
                break;
            case 151:
                Lerp_ProgressBars();
                LerpUnlock();
                break;
            case 152:
                ScoreCounter();
                break;
            case 153:
                Delay(3f);
                break;
            case 154:
                WinSoundFadeOut();
                Run_DarkerPanel();
                break;
            case 155:
                Delay(.3f);
                break;
            case 156:
                Button_Next();
                break;

            case 180:
                Delay(1f);
                break;
            case 181:
                WinSoundFadeOut();
                Run_DarkerPanel();
                break;
            case 182:
                Delay(.3f);
                break;
            case 183:
                Button_Next();
                break;
            //--------------------------------------------------------------
            case 200:
                OpenUnlockPanel();
                break;
            case 201:
                LerpUnlock();
                break;
            case 202:
                switcher = 152;
                break;
            //--------------------------------------------------------------
            case 250:
                Delay(.5f);
                break;
            case 251:
                switcher = 152;
                //Ball3DTexLerp();
                break;
        }
    }
    //----------------------------------------------------
    //-------------------Open-Close-----------------------
    private void OpenRankingPanel(int SwitchIncrease = 1)
    {
        RankingPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1500f, 0f, 0f);
        RankingPanel.SetActive(true);
        switcher += SwitchIncrease;
    }
    private void CloseRankingPanel(int SwitchIncrease = 1)
    {
        RankingPanel.SetActive(false);
        switcher += SwitchIncrease;
    }
    private void OpenRankingList(int SwitchIncrease = 1)
    {
        Vector3 v = RankingList.GetComponent<RectTransform>().anchoredPosition3D;
        v.x = 1500f;
        RankingList.GetComponent<RectTransform>().anchoredPosition3D = v;
        RankingList.SetActive(true);
        switcher += SwitchIncrease;
    }
    private void CloseRankingList(int SwitchIncrease = 1)
    {
        RankingList.SetActive(false);
        switcher += SwitchIncrease;
    }
    private void OpenUnlockPanel(bool isZero = false, int SwitchIncrease = 1)
    {
        NextButton.GetComponent<Button>().interactable = false;
        BallLoader.fillAmount = 1f - (score / 100f);
        Percent.text = score + "%";
        if (!isZero) UnlockPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1500f, 0f, 0f);
        UnlockPanel.SetActive(true);
        isUnlockShow = true;
        switcher += SwitchIncrease;
    }
    private void CloseUnlockPanel(int SwitchIncrease = 1)
    {
        UnlockPanel.SetActive(false);
        switcher += SwitchIncrease;
    }
    private void CloseEarnPanel(int SwitchIncrease = 1)
    {
        EarnPanel.SetActive(false);
        switcher += SwitchIncrease;
    }
    private void Open_NextButton()
    {
        Vector3 pos = NextButton.GetComponent<RectTransform>().anchoredPosition3D;
        pos.x = 0f;
        NextButton.GetComponent<RectTransform>().anchoredPosition3D = pos;
        NextButton.SetActive(true);
        switcher = -1;
    }
    private void Close_NextButton(int SwitchIncrease = 1)
    {
        NextButton.SetActive(false);
        switcher += SwitchIncrease;
    }
    private void Run_DarkerPanel()
    {
        float a = DarkerPanel.color.a;
        a = Mathf.MoveTowards(a, 1f, Time.deltaTime);
        DarkerPanel.color = new Color(0f, 0f, 0f, a);
        if (a >= .999f) switcher++;
    }
    private void WinSoundFadeOut()
    {
        WinnerBallHoopAudio.Win.volume = Mathf.MoveTowards(WinnerBallHoopAudio.Win.volume, 0f, Time.deltaTime * .7f);
    }
    //----------------------------------------------------
    //---------------Lerp-Move----------------------------
    private void BackgroundLerp()
    {
        BackGround.color = Color.Lerp(BackGround.color, BackgroundColor, Time.deltaTime * 5f);
        if (Mathf.Abs(BackgroundColor.a - BackGround.color.a) < .03f) switcher++;
    }
    private void LerpRanking(int SwitchIncrease = 1)
    {
        Vector3 pos = RankingPanel.GetComponent<RectTransform>().anchoredPosition3D;
        Vector3 lerp = pos;
        lerp.x = 0f;
        pos = Vector3.MoveTowards(pos, lerp, Time.deltaTime * 2500f);
        RankingPanel.GetComponent<RectTransform>().anchoredPosition3D = pos;
        if (Vector3.Distance(lerp, pos) < .1f)
        {
            RankingPanel.GetComponent<RectTransform>().anchoredPosition3D = lerp;
            switcher += SwitchIncrease;
        }
    }
    private void LerpUnlock()
    {
        Vector3 lerp = Vector3.MoveTowards(UnlockPanel.GetComponent<RectTransform>().anchoredPosition3D, Vector3.zero, Time.deltaTime * 2500f);
        UnlockPanel.GetComponent<RectTransform>().anchoredPosition3D = lerp;
        if (Vector3.Distance(lerp, Vector3.zero) < .1f)
        {
            NextButton.GetComponent<Button>().interactable = true;
            NextButton.SetActive(false);
            Vector3 pos = NextButton.GetComponent<RectTransform>().anchoredPosition3D;
            pos.x = 0f;
            NextButton.GetComponent<RectTransform>().anchoredPosition3D = pos;
            UnlockPanel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

            UnlockPanel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            switcher++;
        }

        Vector3 pos3 = NextButton.GetComponent<RectTransform>().anchoredPosition3D;
        lerp = pos3;
        lerp.x = -1500f;
        NextButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.MoveTowards(pos3, lerp, Time.deltaTime * 2500f);

        pos3 = RankingPanel.GetComponent<RectTransform>().anchoredPosition3D;
        lerp = pos3;
        lerp.x = -1500f;
        pos3 = Vector3.MoveTowards(pos3, lerp, Time.deltaTime * 2500f);
        RankingPanel.GetComponent<RectTransform>().anchoredPosition3D = pos3;

    }
    private void Lerp_ProgressBars()
    {
        if (progressbarDouble != null)
        {
            Vector3 pos = progressbarDouble.GetComponent<RectTransform>().anchoredPosition3D;
            pos.x = -1500f;
            Vector3 lerp = Vector3.MoveTowards(progressbarDouble.GetComponent<RectTransform>().anchoredPosition3D, pos, Time.deltaTime * 2500f);
            progressbarDouble.GetComponent<RectTransform>().anchoredPosition3D = lerp;
        }
        if (progressbarSingle != null)
        {
            Vector3 pos = progressbarSingle.GetComponent<RectTransform>().anchoredPosition3D;
            pos.x = -1500f;
            Vector3 lerp = Vector3.MoveTowards(progressbarSingle.GetComponent<RectTransform>().anchoredPosition3D, pos, Time.deltaTime * 2500f);
            progressbarSingle.GetComponent<RectTransform>().anchoredPosition3D = lerp;
        }
    }
    private void Lerp_RankingList()
    {
        Vector3 pos = RankingList.GetComponent<RectTransform>().anchoredPosition3D;
        Vector3 lerp = pos;
        lerp.x = 0f;

        pos = Vector3.MoveTowards(pos, lerp, Time.deltaTime * 2500f);
        RankingList.GetComponent<RectTransform>().anchoredPosition3D = pos;
        if (Vector3.Distance(lerp, pos) < .5f)
        {
            RankingList.GetComponent<RectTransform>().anchoredPosition3D = lerp;
            switcher++;
        }
    }
    //----------------------------------------------------
    //-----------------Counters---------------------------
    private void CalculateMoneyAndCupThrowFrequenceTime()
    {
        moneyTime = (1f / earnedMoney);
        if (moneyTime < .05f)
        {
            moneyCountScaler = .05f / moneyTime;
            moneyCountScaler = Mathf.Floor(moneyCountScaler) + 1f;
        }
        cupTime = (1f / earnedCups);

        moneyTime *= moneyCountScaler;
        moneyTimer = moneyTime;
        cupTimer = cupTime;
        switcher++;
    }
    private void MoneyThrow(bool siblingBack = false)
    {
        if (isMoneyThrowComplete) return;
        moneyTimer += Time.deltaTime;
        if (moneyTimer > moneyTime)
        {
            if (!MoneyCount.isPlaying) MoneyCount.Play();
            moneyTimer = 0f;
            totalThrowedMoney += (int)moneyCountScaler;
            GameObject g = Instantiate(throwMoney, RankingPanel.transform);
            if (siblingBack) g.transform.SetSiblingIndex(3);

            int throwCount = (int)moneyCountScaler;
            if (totalThrowedMoney > earnedMoney)
            {
                throwCount -= totalThrowedMoney - earnedMoney;
                totalThrowedMoney = earnedMoney;
            }


            g.GetComponent<ThrowerRect>().Throw(ThrowMoneyStart, ThrowMoneyEnd, 3000f, true, this, (int)throwCount);
            earnMoney.SetText((int.Parse(earnMoney.text) - 1).ToString());
            if (totalThrowedMoney >= earnedMoney)
            {
                MoneyCount.Stop();
                MoneyRegister.Play();
                isMoneyThrowComplete = true;
            }
        }
    }
    private void CupThrow(bool siblingBack = false)
    {
        if (isCupThrowComplete) return;
        cupTimer += Time.deltaTime;
        if (cupTimer > cupTime)
        {
            cupTimer = 0f;
            totalThrowedCups++;
            GameObject g = Instantiate(throwCup, RankingPanel.transform);
            if (siblingBack) g.transform.SetSiblingIndex(3);
            g.GetComponent<ThrowerRect>().Throw(ThrowCupStart, ThrowCupEnd, 3000f, false, this, 1);
            earnCups.SetText((int.Parse(earnCups.text) - 1).ToString());
            if (totalThrowedCups >= earnedCups) isCupThrowComplete = true;

            if (t.comboCount > 0)
            {
                t.comboCount--;
                if (t.comboCount > 0) comboCount.SetText("Combo x" + t.comboCount.ToString());
                else comboCount.SetText("Level Cup");
            }
            else
            {
                comboCount.transform.parent.gameObject.SetActive(false);
            }
        }
    }
    private void MoneyHeaderCounter()
    {
        if (isMoneyThrowComplete) return;
        moneyTimer += Time.deltaTime;
        if (moneyTimer > moneyTime)
        {
            if (!MoneyCount.isPlaying) MoneyCount.Play();
            moneyTimer = 0f;
            totalThrowedMoney++;

            int money = int.Parse(totalMoney.text);
            money++;
            totalMoney.SetText(money.ToString());

            if (totalThrowedMoney == earnedMoney)
            {
                MoneyCount.Stop();
                MoneyRegister.Play();
                isMoneyThrowComplete = true;
            }
        }
    }
    private void CupHeaderCounter()
    {
        if (isCupThrowComplete) return;
        cupTimer += Time.deltaTime;
        if (cupTimer > cupTime)
        {
            cupTimer = 0f;
            totalThrowedCups++;
            if (totalThrowedCups == earnedCups) isCupThrowComplete = true;

            if (t.comboCount > 0)
            {
                t.comboCount--;
                if (t.comboCount > 0) comboCount.SetText("Combo x" + t.comboCount.ToString());
                else comboCount.SetText("Level Cup");
            }
            else
            {
                comboCount.transform.parent.gameObject.SetActive(false);
            }

            int cup = int.Parse(totalCups.text);
            cup++;
            totalCups.SetText(cup.ToString());
            MoneyBell.Play();
        }
    }
    private void CheckThrowComplete()
    {
        if (isMoneyThrowComplete && isCupThrowComplete) switcher++;
    }
    public void IncreaseMoney(int increase)
    {
        int money = int.Parse(totalMoney.text);
        money += increase;
        totalMoney.SetText(money.ToString());
    }
    public void IncreaseCup()
    {
        int cup = int.Parse(totalCups.text);
        cup++;
        totalCups.SetText(cup.ToString());
        MoneyBells[bellIndex].Play();
        bellIndex = (bellIndex + 1) % MoneyBells.Count;
    }
    //----------------------------------------------------
    //----------------------------------------------------
    private void Delay(float time)
    {
        timer += Time.deltaTime;
        if (timer >= time)
        {
            timer = 0f;
            switcher++;
        }
    }
    private void ChoosePanel()
    {
        if (showEarn && !isEarnShow) switcher = 50;
        else if (showEarn && isEarnShow && showRanking && !isRankingShow) switcher = 57;
        else if (showRanking && !isRankingShow) switcher = 100;
        else if (!isEarnShow && !isRankingShow && showUnlockScreen && !isUnlockShow) switcher = 150;
        else if ((isEarnShow || isRankingShow) && showUnlockScreen && !isUnlockShow) switcher = 200;
        else
        {
            NextButton.SetActive(true);
            switcher = -1;
        }
    }
    private void CheckOtherPanelsAfterEarn()
    {
        isEarnShow = true;
        if (showUnlockScreen && !isUnlockShow && !showRanking) Open_NextButton();
        else
        {
            ChoosePanel();
        }
    }
    private void OpenRankingListOnZero()
    {
        RankingList.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        RankingList.SetActive(true);
    }
    private void CalculateNewRanks()
    {
        int myOldIndex = otherUsers.IndexOf(user);
        int underPlayersCount_old = otherUsers.Count - myOldIndex - 1;
        int myListIndex_old = 4;

        if (underPlayersCount_old < 3)
        {
            myListIndex_old = 7 - underPlayersCount_old;
        }
        else if (myOldIndex < 4)
        {
            myListIndex_old = myOldIndex;
        }


        user.cups = cups;
        List<User> ranks = otherUsers.OrderByDescending(x => x.cups).ToList();
        int index = ranks.IndexOf(ranks.First(x => x.cups <= cups));
        ranks.Remove(user);
        ranks.Insert(index, user);
        myNewIndex = ranks.IndexOf(user);
        int underPlayersCount = ranks.Count - myNewIndex - 1;
        User lastCellObject = user;
        for (int i = ranks.Count - 1; i >= 0; i--)
        {
            if (ranks[i].cell != null)
            {
                lastCellObject = ranks[i];
                break;
            }
        }
        myNewListIndex = 4;

        if (underPlayersCount < 3)
        {
            myNewListIndex = 7 - underPlayersCount;
        }
        else if (myNewIndex < 4)
        {
            myNewListIndex = myNewIndex;
        }
        int underDif = ranks.IndexOf(lastCellObject) - ranks.IndexOf(user);

        for (int i = 1; i <= underDif; i++)
        {
            if (myNewIndex + i >= ranks.Count) break;

            totalUnderCells++;
            User u = ranks[myNewIndex + i];
            u.rankMove = true;
            int oldDist = otherUsers.IndexOf(user) - otherUsers.IndexOf(u);
            if (u.cell == null)
            {
                u.cell = Instantiate(rankCellTemplate, rankMask);
                u.cell.transform.Find("Flag").GetComponent<Image>().sprite = flags[u.country];
                u.cell.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(u.name);
                int oldIndex = otherUsers.IndexOf(u);
                u.cell.transform.Find("Rank Number").GetComponent<TextMeshProUGUI>().SetText((oldIndex + 1).ToString());
                u.cell.transform.Find("Cup Number").GetComponent<TextMeshProUGUI>().SetText(u.cups.ToString());
                Vector2 p = u.cell.GetComponent<RectTransform>().anchoredPosition;
                p.y = -(myListIndex_old - oldDist) * 15f;
                u.cell.GetComponent<RectTransform>().anchoredPosition = p;
            }
        }
        for (int i = 1; i <= myNewListIndex; i++)
        {
            if (myNewIndex - i < 0) break;

            totalUpperCells++;
            User u = ranks[myNewIndex - i];
            u.rankMove = true;
            int oldDist = otherUsers.IndexOf(user) - otherUsers.IndexOf(u);
            if (u.cell == null)
            {
                u.cell = Instantiate(rankCellTemplate, rankMask);
                u.cell.transform.Find("Flag").GetComponent<Image>().sprite = flags[u.country];
                u.cell.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(u.name);
                int oldIndex = otherUsers.IndexOf(u);
                u.cell.transform.Find("Rank Number").GetComponent<TextMeshProUGUI>().SetText((oldIndex + 1).ToString());
                u.cell.transform.Find("Cup Number").GetComponent<TextMeshProUGUI>().SetText(u.cups.ToString());
                Vector2 p = u.cell.GetComponent<RectTransform>().anchoredPosition;
                p.y = -(myListIndex_old - oldDist) * 15f;
                u.cell.GetComponent<RectTransform>().anchoredPosition = p;
            }
        }
        otherUsers = ranks;
        user.cell.transform.SetAsLastSibling();
        user.rankMove = true;
        totalCellMovements = totalUnderCells + totalUpperCells + 1;
        switcher++;
    }
    private void RankCells()
    {
        if (user.rankMove)
        {
            Vector3 pos = user.cell.GetComponent<RectTransform>().anchoredPosition3D;
            Vector3 newPos = pos;
            newPos.y = -(myNewListIndex * 15f);
            pos = Vector3.MoveTowards(pos, newPos, Time.deltaTime * 100f);
            user.cell.GetComponent<RectTransform>().anchoredPosition3D = pos;

            if (Vector3.Distance(pos, newPos) < .5f)
            {
                user.cell.GetComponent<RectTransform>().anchoredPosition3D = newPos;
                user.cell.transform.Find("Rank Number").GetComponent<TextMeshProUGUI>().SetText((otherUsers.IndexOf(user) + 1).ToString());
                user.rankMove = false;
                totalFinishMovements++;
            }
        }


        for (int i = myNewIndex + 1; i <= myNewIndex + totalUnderCells; i++)
        {
            if (otherUsers[i].rankMove)
            {
                Vector3 pos = otherUsers[i].cell.GetComponent<RectTransform>().anchoredPosition3D;
                Vector3 newPos = pos;
                newPos.y = -(myNewListIndex * 15f) - ((i - myNewIndex) * 15f);
                pos = Vector3.MoveTowards(pos, newPos, Time.deltaTime * 200f);
                otherUsers[i].cell.GetComponent<RectTransform>().anchoredPosition3D = pos;
                if (Vector3.Distance(pos, newPos) < .5f)
                {
                    otherUsers[i].cell.GetComponent<RectTransform>().anchoredPosition3D = newPos;
                    otherUsers[i].cell.transform.Find("Rank Number").GetComponent<TextMeshProUGUI>().SetText((i + 1).ToString());
                    otherUsers[i].rankMove = false;
                    totalFinishMovements++;
                }
            }

        }
        for (int i = myNewIndex - 1; i >= myNewIndex - totalUpperCells; i--)
        {
            if (otherUsers[i].rankMove)
            {
                Vector3 pos = otherUsers[i].cell.GetComponent<RectTransform>().anchoredPosition3D;
                Vector3 newPos = pos;
                newPos.y = -(myNewListIndex * 15f) + ((myNewIndex - i) * 15f);
                pos = Vector3.MoveTowards(pos, newPos, Time.deltaTime * 200f);
                otherUsers[i].cell.GetComponent<RectTransform>().anchoredPosition3D = pos;
                if (Vector3.Distance(pos, newPos) < .5f)
                {
                    otherUsers[i].cell.GetComponent<RectTransform>().anchoredPosition3D = newPos;
                    otherUsers[i].cell.transform.Find("Rank Number").GetComponent<TextMeshProUGUI>().SetText((i + 1).ToString());
                    otherUsers[i].rankMove = false;
                    totalFinishMovements++;
                }
            }
        }
        if (totalFinishMovements == totalCellMovements) switcher++;
    }
    private void FocusMe()
    {

        sin += Time.deltaTime * 200f;
        if (sin < 180f)
        {
            user.cell.GetComponent<RectTransform>().localScale = myCellLocalScale * (1f + (Mathf.Abs(Mat.sin(sin)) * .07f));
        }
        else
        {
            user.cell.GetComponent<RectTransform>().localScale = myCellLocalScale;
            switcher++;
        }


    }
    private void ScoreCounter()
    {
        score += Time.deltaTime * 20f;

        if (score >= 100f)
        {
            UnlockBallSoundEffect.Play();
            if (NewBallUnlockText != null) NewBallUnlockText.SetActive(true);
            NewBallUnlockLighter.SetActive(true);
            unlockBallIndex++;
            unlockBallIndex = unlockBallIndex % BallSprites.Count;
            BallLoader.fillAmount = 0f;
            Percent.text = "100%";
            score -= 100f;
            switcher++;

            int index = (unlockBallIndex) % UnlockBallTextures.Count;
            //UnlockBall.sprite = UnlockBallTextures[(index + 1) % UnlockBallTextures.Count];

            string unlockBallIndices = PlayerPrefs.GetString("UnlockedBallIndices");
            if (!unlockBallIndices.Contains(index.ToString()))
            {
                newUnlockBallIndex = index;
                selectedBallIndex = index;
            }



            return;

        }
        int intScore = (int)score;
        Percent.text = intScore + "%";
        BallLoader.fillAmount = 1f - (score / 100f);

        if ((int)score == targetScore) switcher = 180;
    }
    public void Button_Next()
    {
        if ((showRanking && !isRankingShow) || (showUnlockScreen && !isUnlockShow || (showEarn && !isEarnShow))) switcher = 2;
        else
        {
            PlayerPrefs.SetInt("OldMoney", money);
            PlayerPrefs.SetInt("OldCups", cups);

            int i = PlayerPrefs.GetInt("CompletedLevelCount");
            i++;
            PlayerPrefs.SetInt("CompletedLevelCount", i);


            GameObject bot = GameObject.Find("BallControllerForBot");
            if(bot != null)
            {
                int y = PlayerPrefs.GetInt("CompletedRaceLevelCount");
                y++;
                PlayerPrefs.SetInt("CompletedRaceLevelCount", y);
            }
            t.LoadLevel();
        }
    }
}

[System.Serializable]
public class User
{
    public string name = "";
    public Country country = Country.UnitedStates;
    public int cups = 0;
    [HideInInspector] public GameObject cell;
    public bool rankMove = false;
}

public enum Country
{
    Belgium = 0,
    Brazil = 1,
    Germany = 2,
    Japan = 3,
    France = 4,
    Netherlands = 5,
    Spain = 6,
    Turkey = 7,
    UnitedKingdom = 8,
    UnitedStates = 9
}

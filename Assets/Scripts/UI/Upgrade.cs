using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class Upgrades
{
    public string Name = "";
    public string HeaderDescription = "";
    public int Cost = 0;
    [Min(0f)] public float destroyTime = 0f;
    public GameObject ImageObject;
    public AudioClip paySound;
    public bool AlwaysShow = false;
    public bool AlwaysHide = false;

}
[System.Serializable]
public class Card
{
    public string Name = "";
    public TextMeshProUGUI DescriptionText;
    public RectTransform cardAnchor;
    public TextMeshProUGUI MoneyText;
    public Button miniButton;
    public GameObject Clock;
    public TextMeshProUGUI Time;
    public TextMeshProUGUI miniDescription;
    

    [HideInInspector] public Upgrades selectedUpgrade;
}
[System.Serializable]
public class UpgradeObjects
{
    public GameObject BowlingBall;
    public GameObject USBall;
    public GameObject BlockHand;
}


public class Upgrade : MonoBehaviour
{
    public Throw th;
    public ThrowBot tb;
    public RectTransform UpgradePanel;
    public float YPos = 20f;
    public int showEvery___Levels = 2;
    public GameObject x2MoneyTM;
    public TextMeshProUGUI TotalMoneyText;
    public GameObject EarnText;
    public Color LockColor;
    public Color UnlockColor;
    public UpgradeObjects upgradeObjects;
    public List<Card> cards = new List<Card>();
    public List<Upgrades> upgrades = new List<Upgrades>();
    public AudioSource UpgradeAudio;
    private Upgrades selectedUpgrade;
    private Vector3 firstUpgradePos;
    private bool block_ball = false;
    private bool chain_ball = false;
    private bool change_ball = false;
    private bool fire_ball = false;
    private bool double_ball = false;
    private bool fast_ball = false;
    private bool smaller_ball = false;
    private bool double_money = false;
    private int selectedCard = 0;
    private Transform x2MoneyParent;
    private int x2Switcher = 0;
    private float x2FontSize = 0f;
    private bool stopLerpCanvas = false;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Level") < 3) Destroy(gameObject);

        ShowOrDestroyUpgradeCanvas();
        RemoveCompletedUpgrades();
        RemoveHideUpgrades();
        RemoveRaceUpgradesIfSingleLevel();
        RemoveSomeUpgradesIfHoopMoves();
    }
    void Start()
    {
        firstUpgradePos = UpgradePanel.anchoredPosition3D;
        GenerateRandomCards();
    }
    void Update()
    {
        LerpCanvas();
        SwitcherController();

        if (th.FinishGame) x2MoneyTM.SetActive(false);
    }

    private void LerpCanvas()
    {
        if (stopLerpCanvas) return;

        Vector3 p = UpgradePanel.anchoredPosition3D;
        Vector3 t = p;
        t.y = YPos;
        UpgradePanel.anchoredPosition3D = Vector3.Lerp(p, t, Time.deltaTime * 8f);
        if ((t - p).magnitude < 3f)
        {
            stopLerpCanvas = true;
        }
        
    }
    private void ShowOrDestroyUpgradeCanvas()
    {
        int lastIndex = PlayerPrefs.GetInt("LastLevelToShowUpgrade");
        int currentIndex = PlayerPrefs.GetInt("CompletedLevelCount");

        if (currentIndex - lastIndex >= showEvery___Levels)
        {
            PlayerPrefs.SetInt("LastLevelToShowUpgrade", currentIndex);
            ShowUpgradePanel();
        }
        else
        {
            ApplyEarnAndDestroyUpgrade();
        }
    }
    private void RemoveCompletedUpgrades()
    {
        float time = PlayerPrefs.GetFloat("TimeToRebuild");
        if (time <= .31f) upgrades.RemoveAt(upgrades.FindIndex(x => x.Name == "fast_ball"));
    }
    private void RemoveHideUpgrades()
    {
        upgrades.RemoveAll(x => x.AlwaysHide);
    }
    private void RemoveRaceUpgradesIfSingleLevel()
    {
        if (!th.OpponentController.activeSelf)
        {
            upgrades.RemoveAt(upgrades.FindIndex(x => x.Name == "block_ball"));
            upgrades.RemoveAt(upgrades.FindIndex(x => x.Name == "change_ball"));
            upgrades.RemoveAt(upgrades.FindIndex(x => x.Name == "fire_ball"));
        }
    }
    private void RemoveSomeUpgradesIfHoopMoves()
    {
        if (th.playerHoopCanMove)
        {
            int index = upgrades.FindIndex(x => x.Name == "chain_ball");
            if(index >= 0) upgrades.RemoveAt(index);
        }
    }
    private void GenerateRandomCards()
    {
        int money = PlayerPrefs.GetInt("Money");
        List<Upgrades> always = upgrades.FindAll(x => x.AlwaysShow);
        List<Upgrades> randoms = upgrades.FindAll(x => !x.AlwaysShow);

        List<Upgrades> alwaysCanBuy = always.FindAll(x => x.Cost <= money);
        List<Upgrades> randomsCanBuy = randoms.FindAll(x => x.Cost <= money);

        foreach(var v in alwaysCanBuy)
        {
            always.Remove(v);
        }
        foreach (var v in randomsCanBuy)
        {
            randoms.Remove(v);
        }

        int index = 0;
        int cardIncrease = 0;
        int always_i = 0;
        int randoms_i = 0;

        if(always.Count > 0)
        {
            int r = Random.Range(0, always.Count);
            Upgrades selected = always[r];
            Upgrades last = always[always.Count - 1];
            always[r] = last;
            always[always.Count - 1] = selected;
            cards[cards.Count - 1].selectedUpgrade = selected;
            always_i++;
            cardIncrease++;
        }
        else if(randoms.Count > 0)
        {
            int r = Random.Range(0, randoms.Count);
            Upgrades selected = randoms[r];
            Upgrades last = randoms[randoms.Count - 1];
            randoms[r] = last;
            randoms[randoms.Count - 1] = selected;
            cards[cards.Count - 1].selectedUpgrade = selected;
            randoms_i++;
            cardIncrease++;
        }

        for (int i = 0; i < alwaysCanBuy.Count; i++)
        {
            if (index >= cards.Count - cardIncrease) break;

            int r = Random.Range(0, alwaysCanBuy.Count - i);
            Upgrades selected = alwaysCanBuy[r];
            Upgrades last = alwaysCanBuy[alwaysCanBuy.Count - (i + 1)];
            alwaysCanBuy[r] = last;
            alwaysCanBuy[alwaysCanBuy.Count - 1] = selected;
            cards[index].selectedUpgrade = selected;
            index++;
        }
        for (int i = 0; i < randomsCanBuy.Count; i++)
        {
            if (index >= cards.Count - cardIncrease) break;

            int r = Random.Range(0, randomsCanBuy.Count - i);
            Upgrades selected = randomsCanBuy[r];
            Upgrades last = randomsCanBuy[randomsCanBuy.Count - (i + 1)];
            randomsCanBuy[r] = last;
            randomsCanBuy[randomsCanBuy.Count - 1] = selected;
            cards[index].selectedUpgrade = selected;
            index++;
        }
        for (int i = always_i; i < always.Count; i++)
        {
            if (index >= cards.Count - cardIncrease) break;

            int r = Random.Range(0, always.Count - i);
            Upgrades selected = always[r];
            Upgrades last = always[always.Count - (i + 1)];
            always[r] = last;
            always[always.Count - 1] = selected;
            cards[index].selectedUpgrade = selected;
            index++;
        }
        for (int i = randoms_i; i < randoms.Count; i++)
        {
            if (index >= cards.Count - cardIncrease) break;

            int r = Random.Range(0, randoms.Count - i);
            Upgrades selected = randoms[r];
            Upgrades last = randoms[randoms.Count - (i + 1)];
            randoms[r] = last;
            randoms[randoms.Count - 1] = selected;
            cards[index].selectedUpgrade = selected;
            index++;
        }

        foreach (Card card in cards)
        {
            if (card.selectedUpgrade != null && card.selectedUpgrade.ImageObject != null)
            {
                card.selectedUpgrade.ImageObject.transform.SetParent(card.cardAnchor.transform);
                card.selectedUpgrade.ImageObject.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                card.selectedUpgrade.ImageObject.SetActive(true);
                card.DescriptionText.text = card.selectedUpgrade.HeaderDescription;

                if (card.selectedUpgrade.Name == "fast_ball") FastReload_Override(card.selectedUpgrade, card.miniDescription);
                card.MoneyText.text = card.selectedUpgrade.Cost.ToString();

                if  (card.selectedUpgrade.Cost > money)
                {
                    card.MoneyText.color = LockColor;
                    card.cardAnchor.transform.GetComponent<Button>().interactable = false;
                    card.miniButton.interactable = false;
                }
                else card.MoneyText.color = UnlockColor;

                if (card.selectedUpgrade.destroyTime > 0f)
                {
                    card.Clock.SetActive(true);
                    card.Time.SetText(card.selectedUpgrade.destroyTime.ToString("0") + "sec");
                }
            }
        }
    }
    private void ApplyEarnAndDestroyUpgrade(float time = 0f)
    {
        if(selectedUpgrade.paySound != null)
        {
            UpgradeAudio.clip = selectedUpgrade.paySound;
            UpgradeAudio.Play();
        }
        int money = PlayerPrefs.GetInt("Money");
        money -= selectedUpgrade.Cost;
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("OldMoney", money);
        EarnText.GetComponent<EarnShower>().earn = selectedUpgrade.Cost;
        EarnText.SetActive(true);
        TotalMoneyText.SetText(money.ToString());
        Destroy(gameObject, time);
    }
    private void ShowUpgradePanel()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    private void HideUpgradePanel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ButtonEvent(int card)
    {
        if(card == -1)
        {
            Destroy(gameObject);
            return;
        }

        stopLerpCanvas = true;
        selectedCard = card;
        int selectedUpgradeIndex = upgrades.FindIndex(x => x == cards[card].selectedUpgrade);
        if (selectedUpgradeIndex >= 0) SetUpgrade(selectedUpgradeIndex);
    }
    private void SetUpgrade(int upgradeIndex)
    {
        if (upgradeIndex >= upgrades.Count) return;

        selectedUpgrade = upgrades[upgradeIndex];

        if (selectedUpgrade.Name == "block_ball") block_ball = true;
        else if (selectedUpgrade.Name == "chain_ball") chain_ball = true;
        else if (selectedUpgrade.Name == "change_ball") change_ball = true;
        else if (selectedUpgrade.Name == "fire_ball") fire_ball = true;
        else if (selectedUpgrade.Name == "double_ball") double_ball = true;
        else if (selectedUpgrade.Name == "fast_ball") fast_ball = true;
        else if (selectedUpgrade.Name == "smaller_ball") smaller_ball = true;
        else if (selectedUpgrade.Name == "double_money") double_money = true;
    }
    private void SwitcherController()
    {
        if (block_ball) Run_block_ball();
        else if (chain_ball) Run_chain_ball();
        else if (change_ball) Run_change_ball();
        else if (fire_ball) Run_fire_ball();
        else if (double_ball) Run_double_ball();
        else if (fast_ball) Run_fast_ball();
        else if (smaller_ball) Run_smaller_ball();
        else if (double_money) Run_double_money();
    }
    private void Run_block_ball()
    {
        tb.BlockBall = Instantiate(upgradeObjects.BlockHand);
        tb.BlockBall.GetComponent<BlockHand>().DestroyTime = selectedUpgrade.destroyTime;
        ApplyEarnAndDestroyUpgrade();
    }
    private void Run_chain_ball()
    {
        th.Chain.SetActive(true);
        th.Chain.GetComponent<ChainHoop>().destroyTime = selectedUpgrade.destroyTime;
        ApplyEarnAndDestroyUpgrade();
    }
    private void Run_change_ball()
    {
        tb.ChangeBall = true;
        tb.ChangeBallStopTime = selectedUpgrade.destroyTime;
        ApplyEarnAndDestroyUpgrade();
    }
    private void Run_fire_ball()
    {
        th.ApplyFireBall = true;
        ApplyEarnAndDestroyUpgrade();
    }
    private void Run_double_ball()
    {
        th.enableDoubleBall = true;
        ApplyEarnAndDestroyUpgrade();
    }
    private void Run_fast_ball()
    {
        float time = PlayerPrefs.GetFloat("TimeToRebuild");
        time -= .1f;
        time = Mathf.Max(time, .3f);
        PlayerPrefs.SetFloat("TimeToRebuild", time);
        th.ApplyRebuild = true;
        ApplyEarnAndDestroyUpgrade();
    }
    private void Run_smaller_ball()
    {
        th.BallScale = .5f;
        ApplyEarnAndDestroyUpgrade();
    }
    private void Run_double_money()
    {
        switch(x2Switcher)
        {
            case 0:
                th.MoneyMultiplier = 2;
                DisableCards();
                break;
            case 1:
                SetX2Pos();
                break;
            case 2:
                LerpFontSize();
                LerpUpgrade();
                x2Move();
                break;
            case 3:
                ApplyEarnAndDestroyUpgrade(.2f);
                x2Switcher++;
                break;
        }
    }
    private void DisableCards()
    {
        foreach(Card card in cards)
        {
            card.cardAnchor.transform.GetComponent<Button>().interactable = false;
            card.miniButton.interactable = false;
        }
        x2Switcher++;
    }
    private void SetX2Pos()
    {
        x2MoneyParent = x2MoneyTM.transform.parent;
        x2MoneyTM.transform.SetParent(cards[selectedCard].cardAnchor.transform);
        x2MoneyTM.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-190f, -90f, 0f);
        x2MoneyTM.transform.SetParent(x2MoneyParent);
        x2FontSize = x2MoneyTM.GetComponent<TextMeshProUGUI>().fontSize;
        x2MoneyTM.GetComponent<TextMeshProUGUI>().fontSize = 20f;
        x2MoneyTM.SetActive(true);
        x2MoneyTM.GetComponent<x2Destroyer>().th = th;
        x2Switcher++;
    }
    private void LerpFontSize()
    {
        if (Mathf.Abs(x2MoneyTM.GetComponent<TextMeshProUGUI>().fontSize - x2FontSize) > .1f)
        {
            x2MoneyTM.GetComponent<TextMeshProUGUI>().fontSize = Mathf.MoveTowards(x2MoneyTM.GetComponent<TextMeshProUGUI>().fontSize, x2FontSize, Time.deltaTime * 600f);
        }
    }
    private void x2Move()
    {
        RectTransform r = x2MoneyTM.GetComponent<RectTransform>();

        r.anchoredPosition3D = Vector3.MoveTowards(r.anchoredPosition3D, Vector3.zero, Time.deltaTime * 6000f);

        if (r.anchoredPosition3D.magnitude < .1f) x2Switcher++;
    }
    private void LerpUpgrade()
    {
        UpgradePanel.anchoredPosition3D = Vector3.Lerp(UpgradePanel.anchoredPosition3D, firstUpgradePos, Time.deltaTime * 8f);
    }
    private void FastReload_Override(Upgrades u, TextMeshProUGUI tm)
    {
        float reloadTime = PlayerPrefs.GetFloat("TimeToRebuild");
        int multiplier = (int)Mat.sq(((2f - reloadTime) * 10f));
        u.Cost += multiplier;

        int level = (int)((2f - reloadTime) * 10f);
        level+=2;
        tm.SetText("Level " + level.ToString());
        tm.gameObject.SetActive(true);
    }
}



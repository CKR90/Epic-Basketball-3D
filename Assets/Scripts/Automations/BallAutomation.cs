using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
using DG.Tweening;

public class BallAutomation : MonoBehaviour
{
    
    public Material numberDefaultMaterial;
    public GameObject Feedback;

    private bool basketCheck1 = false;
    private bool basketCheck2 = false;
    private bool basketCheck3 = false;

    private bool basketCheckFinal1 = false;
    private bool basketCheckFinal2 = false;
    private bool basketCheckFinal3 = false;

    private bool isAddedScore = false;
    private Throw th;
    private Rigidbody rb;

    public ParticleSystem ps;
    [HideInInspector] public bool ps_enabled = false;
    [HideInInspector] public float ps_timer = 0f;
    [HideInInspector] public bool combo = false;
    [HideInInspector] public GesturesEvent gestures;
    private GameObject hoop;
    
    private bool isBasket = false;
    private bool floorFirstHit = false;

    private bool basketCheckBot1 = false;
    private bool basketCheckBot2 = false;
    private bool basketCheckBot3 = false;

    private bool psInitialize = false;
    private void Start()
    {
        th = transform.parent.gameObject.GetComponent<Throw>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        BasketCheck();
        WhenBasketToBotHoop_ShakeCamera();
        ParticleSystemController();
    }
    private void BasketCheck()
    {
        if(basketCheck1 && basketCheck2 && basketCheck3 && !isAddedScore && !th.ApplyFinalHoop && th.score == th.HoopTransforms.Count - 1)
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);

            isBasket = true;
            if (!th.FinishGame && !th.OpponentController.GetComponent<ThrowBot>().FinishGame)
            {
                hoop.transform.Find("HoopColorPanel").GetComponent<HoopColorPanel>().AnimateColor();
                th.Confetti.Play();
                th.AddPoint();
                th.BasketEffect.Play();
                GetComponent<HoopAudio>().PlayBasket();
                GetComponent<HoopAudio>().PlayWin();
                th.Next_UI.GetComponent<WinEvent>().WinnerBallHoopAudio = GetComponent<HoopAudio>();
                th.Next_UI.SetActive(true);
                th.FinishGame = true;
                th.isWin = true;
                th.OpponentController.GetComponent<ThrowBot>().FinishGame = true;
                isAddedScore = true;
                SetBasketEvents();
            }
        }
        else if (basketCheck1 && basketCheck2 && basketCheck3 && !isAddedScore)
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);

            isBasket = true;
            th.AddPoint();
            isAddedScore = true;
            GetComponent<HoopAudio>().PlayBasket();
            th.BasketEffect.Play();
            SetBasketEvents();
            hoop.transform.Find("HoopColorPanel").GetComponent<HoopColorPanel>().AnimateColor();
        }
        else if (basketCheckFinal1 && basketCheckFinal2 && basketCheckFinal3 && !isAddedScore)
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            
            isBasket = true;
            th.BasketEffectFinal.Play();
            GetComponent<HoopAudio>().PlayBasket();
            if(!th.FinishGame)
            {
                th.Confetti.Play();
                GetComponent<HoopAudio>().PlayWin();
                th.Next_UI.GetComponent<WinEvent>().WinnerBallHoopAudio = GetComponent<HoopAudio>();
                th.Next_UI.SetActive(true);
            }
            th.AddPoint();
            th.FinishGame = true;
            th.isWin = true;

            th.OpponentController.GetComponent<ThrowBot>().FinishGame = true;
            isAddedScore = true;
            SetBasketEvents();
        }
    }
    private void WhenBasketToBotHoop_ShakeCamera()
    {
        if(basketCheckBot1 && basketCheckBot2 && basketCheckBot3)
        {
            basketCheckBot1 = false;
            basketCheckBot2 = false;
            basketCheckBot3 = false;

            Camera.main.transform.DOShakeRotation(.5f, 3f);
            th.BasketOpponentHoop.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (th.FinishGame) return;
        if (other.gameObject.name == "Death Area")
        {
            if (!th.FinishGame) Destroy(gameObject);
            else
            {
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        if (th.score < th.HoopTransforms.Count)
        {
            if (other.gameObject.name == "BasketCheckPlayer1") basketCheck1 = true;
            if (other.gameObject.name == "BasketCheckPlayer2") basketCheck2 = true;
            if (other.gameObject.name == "BasketCheckPlayer3")
            {
                basketCheck3 = true;
                hoop = other.transform.parent.gameObject;
            }
        }
        else
        {
            if (other.gameObject.name == "BasketCheckFinal1") basketCheckFinal1 = true;
            if (other.gameObject.name == "BasketCheckFinal2") basketCheckFinal2 = true;
            if (other.gameObject.name == "BasketCheckFinal3") basketCheckFinal3 = true;
        }

        if (other.gameObject.name == "BasketCheckBot1") basketCheckBot1 = true;
        if (other.gameObject.name == "BasketCheckBot2") basketCheckBot2 = true;
        if (other.gameObject.name == "BasketCheckBot3") basketCheckBot3 = true;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Contains("Hoop"))
        {
            ps.Stop();
            ps_enabled = false;
        }
        if (collision.gameObject.tag == "Floor" && !floorFirstHit)
        {
            ps.Stop();
            ps_enabled = false;
            floorFirstHit = true;
            if(!isBasket) th.Reset_Combo();
            if(!th.FinishGame) Destroy(gameObject, 20f);
        }
        if (collision.gameObject.tag == "Ball")
        {
            if (!ps_enabled) return;

            GameObject ball = collision.gameObject;
            if (!ball.name.Contains("Bot")) return;

            BallAutomationBot bb = ball.GetComponent<BallAutomationBot>();
            if (bb == null) return;

            Rigidbody brb = ball.GetComponent<Rigidbody>();
            if (brb == null || brb.isKinematic) return;

            bb.ExplodeEvent();
        }
    }
    private void SetBasketEvents()
    {
        GameObject empty = new GameObject("FeedBack Folder");
        GameObject f = Instantiate(Feedback);
        f.GetComponent<FeedbackAutomation>().Set_Feedback(3.3f - th.feedbackSize);

        if (th.combo > 2)
        {
            f.GetComponent<FeedbackAutomation>().Set_Combo(th.combo - 1);
            th.WinPoint += th.combo;
            th.comboCount++;

            int money = PlayerPrefs.GetInt("Money");
            int add = th.combo * 5 * th.MoneyMultiplier;
            money += add;
            if (gestures != null) gestures.AddMoney(add);
            PlayerPrefs.SetInt("Money", money);
        }
        else
        {
            th.WinPoint++;
            int money = PlayerPrefs.GetInt("Money");
            int add = 5 * th.MoneyMultiplier;
            money += add;
            if (gestures != null) gestures.AddMoney(add);
            PlayerPrefs.SetInt("Money", money);
        }
    }
    private void ParticleSystemController()
    {
        if (!psInitialize)
        {
            if (rb.isKinematic && (th.combo > 2 || th.ApplyFireBall))
            {
                ps.Play();
                ps_enabled = true;
                ps_timer = 0f;
            }
        }
        if (ps.isPlaying && th.FinishGame) ps.Stop();
    }
}

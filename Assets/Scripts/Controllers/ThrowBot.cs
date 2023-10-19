using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ThrowBot : MonoBehaviour
{
    #region Input
    [Header("Score Text Can Be Null")]
    public Text ScoreText;
    public Text BotDeveloperText;
    [Range(0f, 5f)]
    public float timeToRebuild = .3f;
    public GameObject OpponentController;
    public GameObject UpgradeCanvas;
    public List<GameObject> Balls = new List<GameObject>();
    public GameObject Hoop;
    public GameObject OpponentHoop;
    public GameObject FinalHoop;
    public ParticleSystem Fuse;
    public ParticleSystem BasketEffect;
    public ParticleSystem BasketEffectFinal;
    [HideInInspector] public List<Transform> HoopTransforms = new List<Transform>();
    public GameObject RandomThrowPointsFolder_BotHoop;
    public GameObject RandomThrowPointsFolder_FinalHoop;
    public Material BotBallMaterial;
    public Material PlayerBallMaterial;
    public List<Texture> Skins;
    private GameObject CopyBall;
    #endregion

    [HideInInspector] public int score = 0;
    [HideInInspector] public bool FinishGame = false;
    [HideInInspector] public List<Transform> RandomThrowPoints_BotHoop = new List<Transform>();
    [HideInInspector] public List<Transform> RandomThrowPoints_FinalHoop = new List<Transform>();

    private int state = 0;
    private bool WaitToBuild = false;
    private bool ChangeHoopPosition = false;
    private bool flyHoop = false;

    private bool timerLock = false;
    private float timerCount = 0f;
    private float waitingTime = 0f;
    private Transform ThrowPoint;
    [HideInInspector] public bool TapToPlay = false;
    [HideInInspector] public int ThrowIndex = 0;
    [HideInInspector] public int Win1_Lose2 = 0;

    private bool InstantiateNow = true;
    [HideInInspector] public bool MakeItHeavy = false;
    [HideInInspector] public bool ChangeBall = false;
    [HideInInspector] public float ChangeBallStopTime = 0f;
    [HideInInspector] public GameObject BlockBall;
    private float changeBallTimer = 0f;
    private Throw th;


    void Start()
    {
        th = OpponentController.GetComponent<Throw>();
        List<Transform> list = GameObject.Find("BallControllerForPlayer").GetComponent<Throw>().HoopTransforms;
        HoopTransforms = new List<Transform>();

        foreach (Transform t in list)
        {
            HoopTransforms.Add(Instantiate(t));
            Vector3 v = HoopTransforms[HoopTransforms.Count - 1].position;
            v.x *= -1f;
            HoopTransforms[HoopTransforms.Count - 1].position = v;

            v = HoopTransforms[HoopTransforms.Count - 1].localEulerAngles;
            v.y *= -1f;
            HoopTransforms[HoopTransforms.Count - 1].localEulerAngles = v;
        }

        Hoop.transform.position = HoopTransforms[score].position;
        Hoop.transform.rotation = HoopTransforms[score].rotation;

        if (ScoreText != null) ScoreText.text = score.ToString();
        //InstantiateBall();

        //----------------------------------------------------------
        RandomThrowPoints_BotHoop = RandomThrowPointsFolder_BotHoop.transform.GetComponentsInChildren<Transform>().ToList();
        RandomThrowPoints_BotHoop.Remove(RandomThrowPointsFolder_BotHoop.transform);
        //---------------------------------------------------------
        RandomThrowPoints_FinalHoop = RandomThrowPointsFolder_FinalHoop.transform.GetComponentsInChildren<Transform>().ToList();
        RandomThrowPoints_FinalHoop.Remove(RandomThrowPointsFolder_FinalHoop.transform);
    }
    void Update()
    {
        if (UpgradeCanvas != null) return;
        else if (InstantiateNow)
        {
            InstantiateNow = false;
            InstantiateBall();
        }

        if (!TapToPlay)
        {
            Ball_Animate();
            return;
        }
        if (FinishGame) return;

        switch (state)
        {
            case 0:
                Ball_Animate();
                break;
            case 1:
                WaitTimer();
                break;
            case 2:
                ChooseThrowTransform();
                break;
            case 3:
                if(!th.SpaceAnimations) ThrowBall();
                break;
        }

        HoopController();
        ChangeBallController();
    }

    private void InstantiateBall()
    {
        int index = 0;
        if (ChangeBall && Balls.Count > 1) index = Random.Range(1, Balls.Count);
        CopyBall = Instantiate(Balls[index], transform.position - Vector3.up * 3f, Balls[index].transform.rotation, transform);
        CopyBall.GetComponent<Rigidbody>().isKinematic = true;


        if(index == 0)
        {
            if (!PlayerPrefs.HasKey("BotSkin")) PlayerPrefs.SetInt("BotSkin", 0);
            int skinIndex = PlayerPrefs.GetInt("BotSkin");
            int levelCount = PlayerPrefs.GetInt("CompletedRaceLevelCount");
            if (levelCount % 5 == 0)
            {
                levelCount++;
                skinIndex = (skinIndex + 1) % Skins.Count;
                

                PlayerPrefs.SetInt("CompletedRaceLevelCount", levelCount);
                PlayerPrefs.SetInt("BotSkin", skinIndex);
            }
            if (PlayerBallMaterial.GetTexture("_MainTex") == Skins[skinIndex])
            {
                skinIndex = (skinIndex + 1) % Skins.Count;
                PlayerPrefs.SetInt("BotSkin", skinIndex);
            }
            BotBallMaterial.SetTexture("_MainTex", Skins[skinIndex]);
        }

        if (MakeItHeavy)
        {
            CopyBall.GetComponent<BallAutomationBot>().MakeItHeavy = true;
            CopyBall.GetComponent<HoopAudio>().Bounce.clip = CopyBall.GetComponent<HoopAudio>().HeavyBall.clip;
        }

    }
    private void WaitTimer()
    {
        if (!timerLock)
        {
            timerLock = true;
            waitingTime = Random.Range(0.2f, 0.3f);
            timerCount = 0f;
        }

        timerCount += Time.deltaTime;
        if (timerCount >= waitingTime)
        {
            state = 2;
            timerLock = false;
        }
    }
    private void ChooseThrowTransform()
    {
        if (score < HoopTransforms.Count)
        {
            ThrowIndex = Random.Range(0, RandomThrowPoints_BotHoop.Count);
            if (ThrowIndex >= RandomThrowPoints_BotHoop.Count)
            {
                //Karþý Potaya Yolla
                state = 0; //Þimdilik
            }
            else
            {
                ThrowPoint = RandomThrowPoints_BotHoop[ThrowIndex];
                if (BlockBall != null)
                {
                    BlockBall.GetComponent<BlockHand>().point = ThrowPoint.position;
                }
                state = 3;
            }
        }
        else
        {
            ThrowIndex = Random.Range(0, RandomThrowPoints_FinalHoop.Count - 1);
            ThrowPoint = RandomThrowPoints_FinalHoop[ThrowIndex];
            state = 3;
        }
    }
    public void ThrowBall(Transform targetOverride = null, Vector3? torqueOverride = null)
    {
        if (state != 3) return;
        if (!WaitToBuild || th.SpaceAnimations)
        {

            WaitToBuild = true;
            Vector3 target = ThrowPoint.position + ThrowPoint.forward * .2f;

            if (targetOverride != null) target = targetOverride.position;

            Vector3 velocity = ThrowingVelocityToReachTheTarget.CalculateVelocity(CopyBall.transform.position, target, Vector3.zero, false);

            CopyBall.GetComponent<Rigidbody>().isKinematic = false;
            CopyBall.GetComponent<Rigidbody>().velocity = velocity;

            if(torqueOverride == null)
            {
                float x = Random.Range(-8f, 8f);
                float y = Random.Range(-8f, 8f);
                float z = Random.Range(-8f, 8f);
                CopyBall.GetComponent<Rigidbody>().AddTorque(new Vector3(x, y, z));
            }
            else
            {
                CopyBall.GetComponent<Rigidbody>().AddTorque(torqueOverride.GetValueOrDefault());
            }

            if (BlockBall != null) BlockBall.GetComponent<BlockHand>().targetBall = CopyBall.transform;
            StartCoroutine(Rebuild(timeToRebuild));
        }
    }
    IEnumerator Rebuild(float time)
    {
        yield return new WaitForSeconds(time);
        InstantiateBall();
        WaitToBuild = false;
        state = 0;
    }
    public void AddPoint()
    {
        score = (score + 1);
        ChangeHoopPosition = true;

        if (ScoreText != null) ScoreText.text = score.ToString();
    }
    private void HoopController()
    {
        if (!ChangeHoopPosition) return;

        if (score < HoopTransforms.Count)
        {
            Hoop.transform.position = Vector3.Lerp(Hoop.transform.position, HoopTransforms[score].position, Time.deltaTime * 5f);
            Hoop.transform.rotation = Quaternion.Lerp(Hoop.transform.rotation, HoopTransforms[score].rotation, Time.deltaTime * 5f);
            Hoop.transform.localScale = Vector3.Lerp(Hoop.transform.localScale, HoopTransforms[score].localScale, Time.deltaTime * 5f);

            if (Vector3.Distance(Hoop.transform.position, HoopTransforms[score].position) < .1f) ChangeHoopPosition = false;
        }
        else if (score == HoopTransforms.Count && !flyHoop)
        {
            flyHoop = true;
            Transform[] t = Hoop.GetComponentsInChildren<Transform>();
            foreach (var v in t)
            {
                if (v.gameObject.name == "ClickTrigger")
                {
                    v.gameObject.layer = 0;
                }
            }
            Hoop.GetComponent<HoopAutomation>().Start_Automation = true;
            Fuse.Play();
        }
    }
    private void Ball_Animate()
    {
        CopyBall.transform.position = Vector3.Lerp(CopyBall.transform.position, transform.position, Time.deltaTime * 60f);

        if (Vector3.Distance(transform.position, CopyBall.transform.position) < .03f)
        {
            state = 1;
        }
    }
    public void PlayAfterTap()
    {
        TapToPlay = true;
    }
    private void ChangeBallController()
    {
        if (ChangeBall && ChangeBallStopTime > 0f)
        {
            changeBallTimer += Time.deltaTime;

            if (changeBallTimer >= ChangeBallStopTime)
            {
                ChangeBall = false;
            }
        }
    }

}

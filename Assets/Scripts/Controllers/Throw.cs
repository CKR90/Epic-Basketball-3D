using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Throw : MonoBehaviour
{
    #region Variables
    public bool SpaceAnimations = false;
    public bool ApplyFinalHoop = true;
    public bool playerHoopCanMove = false;
    public Controls ControlMethod = Controls.Slide;
    [Range(0f, 3f)]
    public float HorizontalHelperLength = 1.2f;
    [Range(0f, 1f)]
    public float TargetHorizontalFocus = 1f;
    [Range(0.3f, 3f)]
    public float feedbackSize = 2f;
    public float BallScale = 1f;



    [Header("Score Text Can Be Null")]
    public Text ScoreText;
    public bool isClickBotHoopWhenFinal = true;

    [Range(0f, 5f)]
    public float timeToRebuild = 2f;
    public GameObject OpponentController;
    public GameObject Ball;
    public GameObject Hoop;
    public GameObject OpponentHoop;
    public GameObject FinalHoop;
    public GameObject ArrowPointer;
    public Material ArrowFinalMaterial;
    public ParticleSystem Confetti;
    public ParticleSystem Fuse;
    public ParticleSystem BasketEffect;
    public ParticleSystem BasketEffectFinal;
    public GameObject Chain;
    public List<Transform> HoopTransforms = new List<Transform>();
    public GameObject Next_UI;
    public GameObject Restart_UI;
    public GameObject Gestures_UI;
    public GameObject Gestures_UI_TapFirst;
    public GameObject Gestures_UI_Tap;
    public GameObject UpgradeCanvas;
    public RectTransform GesturesMaskCircleLeft;
    public RectTransform GesturesMaskCircleRight;
    public Color DefaultBallMaterialColor;
    public List<Material> CourtMaterials;

    public AudioSource BasketOpponentHoop;

    private GameObject CopyBall;
    private RectTransform progressRect;

    [HideInInspector] public bool FinishGame = false;
    [HideInInspector] public bool isWin = false;
    [HideInInspector] public bool TapToPlay = false;
    private Rigidbody rb;
    private bool throwLock = false;
    [HideInInspector] public int score = 0;
    private bool ChangeHoopPosition = false;
    private bool flyHoop = false;
    private bool BallAnimate = false;
    private Vector3 arrowLocalPos;
    private Vector3 arrowLocalEuler;
    private bool playConfetti = true;

    private Vector3 firstMousePos;
    private Vector3 currentMousePos;
    private bool MouseIsDown = false;
    [HideInInspector] public Transform correctPosition;
    private Vector3 correctPositionOrigin;
    private Transform helperPosition;
    [HideInInspector] public Vector3 targetPos = Vector3.zero, focusPos = Vector3.zero;
    [HideInInspector] public float horizontalfocusOld = 0f;
    [HideInInspector] public bool isBasket = false;
    [HideInInspector] public int combo = 0;
    [HideInInspector] public int comboCount = 0;
    [HideInInspector] public int WinPoint = 0;

    private bool InstantiateNow = true;
    [HideInInspector] public GesturesEvent gestures;
    [HideInInspector] public bool enableDoubleBall = false;
    [HideInInspector] public bool ApplyRebuild = false;
    [HideInInspector] public bool ApplyFireBall = false;
    [HideInInspector] public int MoneyMultiplier = 1;
    private int nextLevelIndex = 0;
    private float HoopLockTime = Mathf.Infinity;
    private float DoubleBallTime = .2f;


    public AudioSource Voice;
    public List<AudioClip> Voices;
    private int oldClipIndex = -1;


    private bool loadLevel = false;
    private bool startCoroutine = false;
    #endregion
    #region Start and Update
    void Start()
    {
        PlayerPrefs.SetFloat("TimeToRebuild", .3f);
        Check_DebugDatabase();
        InitializeLevel();
        SetFirstArrowPosition();
        SetFirstHoopPosition();
        SetFirstCorrectPosition();
    }
    void Update()
    {
        Reset_R_Button();
        if (UpgradeCanvas != null) return;
        else if (InstantiateNow)
        {
            InstantiateNow = false;
            InstantiateBall();
        }

        ApplyRebuildTime();
        CheckConfetti();
        UnlockControls();
        if (CheckFinishGame()) return;
        CheckControls();
        HoopController();
        Ball_Animate();
    }
    #endregion
    #region Start Methods
    private void Check_DebugDatabase()
    {
        GameObject debug = GameObject.Find("DebugCanvas");
        if (debug == null) return;

        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();

        if (PlayerPrefs.HasKey("EditPlayerHoopTransforms_" + buildIndex))
        {
            string text = PlayerPrefs.GetString("EditPlayerHoopTransforms_" + buildIndex);

            List<string> list = text.Split('*').ToList();

            for(int i = 0; i < list.Count; i++)
            {
                if(!string.IsNullOrEmpty(list[i]))
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

                    HoopTransforms[i].position = pos;
                    HoopTransforms[i].localEulerAngles = euler;
                }
            }
            Hoop.transform.position = HoopTransforms[0].position;
            Hoop.transform.localEulerAngles = HoopTransforms[0].localEulerAngles;
        }
        if (PlayerPrefs.HasKey("EditFinalHoopTransform_" + buildIndex))
        {
            string text = PlayerPrefs.GetString("EditFinalHoopTransform_" + buildIndex);
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
        if (PlayerPrefs.HasKey("EditCameraTransform_" + buildIndex))
        {
            string text = PlayerPrefs.GetString("EditCameraTransform_" + buildIndex);
            List<string> data = text.Split('_').ToList();

            Vector3 pos = Camera.main.transform.position;
            Vector3 rot = Camera.main.transform.localEulerAngles;

            pos.y = float.Parse(data[0]);
            rot.x = float.Parse(data[1]);

            Camera.main.transform.position = pos;
            Camera.main.transform.localEulerAngles = rot;
        }
        if (PlayerPrefs.HasKey("EditCourtTransform_" + buildIndex))
        {
            GameObject g = GameObject.Find("Court");
            string text = PlayerPrefs.GetString("EditCourtTransform_" + buildIndex);
            List<string> data = text.Split('_').ToList();

            Vector3 pos = Vector3.zero;
            Vector3 euler = Vector3.zero;
            pos.x = float.Parse(data[0]);
            pos.y = float.Parse(data[1]);
            pos.z = float.Parse(data[2]);
            euler.x = float.Parse(data[3]);
            euler.y = float.Parse(data[4]);
            euler.z = float.Parse(data[5]);

            g.transform.position = pos;
            g.transform.localEulerAngles = euler;
        }
        if (PlayerPrefs.HasKey("EditCourtMaterialName_" + buildIndex))
        {
            string text = PlayerPrefs.GetString("EditCourtMaterialName_" + buildIndex);
            Material m = CourtMaterials.Find(x => x.name == text);

            GameObject crt = GameObject.Find("Backdrop");
            crt.GetComponent<MeshRenderer>().sharedMaterial = m;
        }
    }
    private void InitializeLevel()
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("OldMoney"));
        PlayerPrefs.SetInt("Cups", PlayerPrefs.GetInt("OldCups"));
        timeToRebuild = PlayerPrefs.GetFloat("TimeToRebuild");
        if (!OpponentController.activeSelf)
        {
            Vector3 p = transform.position;
            p.x = 0f;
            transform.position = p;
        }
    }
    private void SetFirstArrowPosition()
    {
        arrowLocalPos = ArrowPointer.transform.localPosition;
        arrowLocalEuler = ArrowPointer.transform.localEulerAngles;
    }
    private void SetFirstHoopPosition()
    {
        Hoop.transform.position = HoopTransforms[score].position;
        Hoop.transform.rotation = HoopTransforms[score].rotation;
    }
    private void SetFirstCorrectPosition()
    {
        correctPosition = Hoop.transform.Find("CorrectPosition");
        correctPositionOrigin = correctPosition.localPosition;
        Calculate_CorrectPosition();
        horizontalfocusOld = TargetHorizontalFocus;
    }
    #endregion
    #region Update Methods
    private void ApplyRebuildTime()
    {
        if (ApplyRebuild)
        {
            ApplyRebuild = false;
            timeToRebuild = PlayerPrefs.GetFloat("TimeToRebuild");
        }
    }
    private void CheckConfetti()
    {
        if (playConfetti && isWin)
        {
            playConfetti = false;
            Confetti.Play();
        }
    }
    private void UnlockControls()
    {
        if (!TapToPlay)
        {
            Ball_Animate();
            if (Input.GetMouseButton(0) && ControlMethod == Controls.Tap && SceneManager.GetActiveScene().buildIndex == 1 && PlayerPrefs.GetInt("Loop") == 0)
            {
                Vector3 center = (Camera.main.WorldToScreenPoint(GesturesMaskCircleLeft.transform.position) + Camera.main.WorldToScreenPoint(GesturesMaskCircleRight.transform.position)) / 2f;
                float radius = Vector3.Distance(Camera.main.WorldToScreenPoint(GesturesMaskCircleLeft.transform.position), Camera.main.WorldToScreenPoint(GesturesMaskCircleRight.transform.position)) / 2f;
                Vector3 tap = Input.mousePosition;
                float distance = Vector3.Distance(tap, center);
                if (distance < radius)
                {
                    TapToPlay = true;
                    OpponentController.GetComponent<ThrowBot>().TapToPlay = true;
                    Gestures_UI.SetActive(false);
                    Gestures_UI_Tap.SetActive(false);
                    Gestures_UI_TapFirst.SetActive(false);
                }
                else return;
            }
            else if (Input.GetMouseButton(0))
            {
                TapToPlay = true;
                OpponentController.GetComponent<ThrowBot>().TapToPlay = true;
                Gestures_UI.SetActive(false);
                Gestures_UI_Tap.SetActive(false);
                Gestures_UI_TapFirst.SetActive(false);
            }
            else return;
        }
    }
    private bool CheckFinishGame()
    {
        if (FinishGame)
        {
            if (!startCoroutine)
            {
                startCoroutine = true;
                LoadLevelAsync();
            }
            return true;
        }
        return false;
    }
    private void CheckControls()
    {
        if (ControlMethod == Controls.Slide || ControlMethod == Controls.Both)
        {
            MouseSlideOrTap();
        }
        else if (ControlMethod == Controls.Tap)
        {
            if (Input.GetMouseButtonDown(0)) MouseDown();
        }
    }
    private void HoopController()
    {
        if (!ChangeHoopPosition || (Chain != null && Chain.activeSelf && score < HoopTransforms.Count)) return;

        if (HoopLockTime < DoubleBallTime + .1f)
        {
            HoopLockTime += Time.deltaTime;
            return;
        }

        if (score < HoopTransforms.Count)
        {
            Hoop.transform.position = Vector3.Lerp(Hoop.transform.position, HoopTransforms[score].position, Time.deltaTime * 5f);
            Hoop.transform.rotation = Quaternion.Lerp(Hoop.transform.rotation, HoopTransforms[score].rotation, Time.deltaTime * 8f);
            Hoop.transform.localScale = Vector3.Lerp(Hoop.transform.localScale, HoopTransforms[score].localScale, Time.deltaTime * 5f);

            float dist = Vector3.Distance(Hoop.transform.position, HoopTransforms[score].position);
            float ang = Mathf.Abs(Hoop.transform.localEulerAngles.y - HoopTransforms[score].localEulerAngles.y);
            if (dist < .1f && ang < .1f)
            {
                ChangeHoopPosition = false;
            }
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
            Destroy(Chain);
            Hoop.GetComponent<HoopAutomation>().Start_Automation = true;
            Fuse.Play();
        }
    }
    private void Ball_Animate()
    {
        if (!BallAnimate) return;
        CopyBall.transform.position = Vector3.Lerp(CopyBall.transform.position, transform.position, Time.deltaTime * 60f);

        if (Vector3.Distance(transform.position, CopyBall.transform.position) < .03f)
        {
            BallAnimate = false;
            throwLock = false;
        }

    }
    #endregion
    #region PlayTime Controllers
    private void MouseSlideOrTap()
    {
        if (Input.GetMouseButton(0) && !MouseIsDown)
        {
            MouseIsDown = true;
            firstMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && MouseIsDown)
        {
            currentMousePos = Input.mousePosition;
        }
        else if (!Input.GetMouseButton(0) && MouseIsDown)
        {
            MouseIsDown = false;
            if (Vector3.Distance(firstMousePos, currentMousePos) > Screen.width / 10f)
            {
                SlideCalculator();
            }
            else if (ControlMethod == Controls.Both)
            {
                MouseDown();
            }
        }
    }
    private void SlideCalculator()
    {
        int layermask = 0;

        if (score < HoopTransforms.Count) layermask = (1 << 6) | (1 << 8);
        else if (score >= HoopTransforms.Count && isClickBotHoopWhenFinal) layermask = (1 << 7) | (1 << 8);
        else layermask = (1 << 7);

        Ray ray = Camera.main.ScreenPointToRay(currentMousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layermask))
        {
            string tag = hit.collider.gameObject.tag;
            if (tag == "Hoop Player" || tag == "Hoop Bot" || tag == "Hoop Final")
            {
                helperPosition = hit.collider.transform.parent.Find("HelperPosition");
                correctPosition = hit.collider.transform.parent.Find("CorrectPosition");
            }
            else return;
        }
        else return;

        Vector3 helper = Camera.main.WorldToScreenPoint(helperPosition.position);
        Vector3 correct = Camera.main.WorldToScreenPoint(correctPosition.position);
        Vector3 offset = Camera.main.WorldToScreenPoint(CopyBall.transform.position) - firstMousePos;
        firstMousePos += offset;
        currentMousePos += offset;

        float y2, y1, x2, x1, m1;
        float y4, y3, x4, x3, m2;



        x2 = correct.x;
        x1 = helper.x;
        y2 = correct.y;
        y1 = helper.y;
        x3 = firstMousePos.x;
        x4 = currentMousePos.x;
        y3 = firstMousePos.y;
        y4 = currentMousePos.y;

        m1 = (y2 - y1) / (x2 - x1);
        m2 = (y4 - y3) / (x4 - x3);

        float dx = (m1 * x1 - m2 * x3 - y1 + y3) / (m1 - m2);
        float dy = m1 * dx - m1 * x1 + y1;

        Vector3 click = new Vector3(dx, dy, 0f);
        MouseDown_Slider(click, false);

        float toCorrect = Vector3.Distance(correctPosition.position, targetPos);

        if (toCorrect < HorizontalHelperLength)
        {
            focusPos = targetPos + (correctPosition.position - targetPos).normalized * toCorrect * TargetHorizontalFocus;
            click = focusPos;
            MouseDown_Slider(Camera.main.WorldToScreenPoint(click));
        }
        try
        {
            MouseDown_Slider(click);
        }
        catch { }
    }
    public void MouseDown(Transform targetOverride = null, Vector3? torqueOverride = null)
    {
        if (throwLock) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layermask = 0;

        if (score < HoopTransforms.Count) layermask = (1 << 6) | (1 << 8);
        else if (score >= HoopTransforms.Count && isClickBotHoopWhenFinal) layermask = (1 << 7) | (1 << 8);
        else layermask = (1 << 7);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            Vector3 point = hit.point + hit.normal * .2f;
            Vector3 velocity = Vector3.zero;
            string parentName = hit.collider.gameObject.transform.parent.name;

            if(targetOverride != null)
            {
                velocity = ThrowingVelocityToReachTheTarget.CalculateVelocity(CopyBall.transform.position, targetOverride.position, Vector3.zero, false);
                if (combo > 2 && !ApplyFireBall) Run_FireBallSound();
            }
            else if (parentName == "Hoop Player" || parentName == "Hoop Final")
            {
                velocity = ThrowingVelocityToReachTheTarget.CalculateVelocity(CopyBall.transform.position, point, Vector3.zero, false);
                if (combo > 2 && !ApplyFireBall) Run_FireBallSound();
            }
            else
            {
                velocity = ThrowingVelocityToReachTheTarget.CalculateVelocity(CopyBall.transform.position, point, Vector3.zero, true);
            }
            if (ApplyFireBall) Run_FireBallSound();
            rb.isKinematic = false;
            rb.velocity = velocity;
            
            if (enableDoubleBall)
            {
                StartCoroutine(ThrowNewBall(DoubleBallTime, CopyBall, velocity));
                HoopLockTime = 0f;
            }

            if(torqueOverride == null)
            {
                float x = Random.Range(-8f, 8f);
                float y = Random.Range(-8f, 8f);
                float z = Random.Range(-8f, 8f);
                rb.AddTorque(new Vector3(x, y, z));
            }
            else
            {
                rb.AddTorque(torqueOverride.GetValueOrDefault());
            }

            throwLock = true;
            StartCoroutine(Rebuild(timeToRebuild));
        }
    }
    private void MouseDown_Slider(Vector3 clickPos, bool Throw = true)
    {
        if (throwLock || float.IsNaN(clickPos.x) || float.IsNaN(clickPos.y) || float.IsInfinity(clickPos.x) || float.IsInfinity(clickPos.y)) return;
        clickPos.z = 0f;
        RaycastHit hit;
        Ray ray;
        try
        {
            ray = Camera.main.ScreenPointToRay(clickPos);
        }
        catch { return; }

        int layermask = 0;

        if (score < HoopTransforms.Count) layermask = (1 << 6) | (1 << 8);
        else if (score >= HoopTransforms.Count && isClickBotHoopWhenFinal) layermask = (1 << 7) | (1 << 8);
        else layermask = (1 << 7);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            Vector3 point = hit.point + hit.normal * .2f * BallScale;
            if (!Throw)
            {
                targetPos = hit.point;
            }
            else
            {
                focusPos = hit.point;
                Vector3 velocity = Vector3.zero;
                string parentName = hit.collider.gameObject.transform.parent.name;
                HoopVelocity hv = hit.collider.gameObject.transform.parent.GetComponent<HoopVelocity>();
                Vector3 hoopPlayerVelocity = Vector3.zero;
                if (hv != null) hoopPlayerVelocity = hv.velocity;
                if (parentName == "Hoop Player" || parentName == "Hoop Final")
                {
                    velocity = ThrowingVelocityToReachTheTarget.CalculateVelocity(CopyBall.transform.position, point, hoopPlayerVelocity, false);
                }
                else
                {
                    velocity = ThrowingVelocityToReachTheTarget.CalculateVelocity(CopyBall.transform.position, point, hoopPlayerVelocity, true);
                }
                if (combo > 2 || ApplyFireBall) Run_FireBallSound();
                rb.isKinematic = false;
                rb.velocity = velocity;

                if (enableDoubleBall)
                {
                    StartCoroutine(ThrowNewBall(DoubleBallTime, CopyBall, velocity));
                    HoopLockTime = 0f;
                }

                float x = Random.Range(-8f, 8f);
                float y = Random.Range(-8f, 8f);
                float z = Random.Range(-8f, 8f);


                rb.AddTorque(new Vector3(x, y, z));
                throwLock = true;
                StartCoroutine(Rebuild(timeToRebuild));
            }
        }
    }
    IEnumerator Rebuild(float time)
    {
        yield return new WaitForSeconds(time);
        InstantiateBall();
    }
    private void InstantiateBall()
    {
        if (TargetHorizontalFocus >= 1f) TargetHorizontalFocus = 1f;
        else TargetHorizontalFocus += .15f;
        TargetHorizontalFocus = Mathf.Clamp(TargetHorizontalFocus, horizontalfocusOld, 1f);

        CopyBall = Instantiate(Ball, transform.position - Vector3.up * 3f, Ball.transform.rotation, transform);
        CopyBall.transform.localScale = CopyBall.transform.localScale * BallScale;
        CopyBall.GetComponent<BallAutomation>().gestures = gestures;
        rb = CopyBall.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        BallAnimate = true;
    }
    public void AddPoint()
    {
        combo++;
        score++;
        ChangeHoopPosition = true;
        TargetHorizontalFocus = horizontalfocusOld;

        if (score == HoopTransforms.Count)
        {
            ArrowPointer.transform.SetParent(FinalHoop.transform);
            ArrowPointer.transform.Find("Line002").gameObject.GetComponent<MeshRenderer>().material = ArrowFinalMaterial;
            ArrowPointer.transform.localPosition = arrowLocalPos;
            ArrowPointer.transform.localEulerAngles = arrowLocalEuler;
        }

        if (combo > 2)
        {
            int index = Random.Range(0, 100) % Voices.Count;
            if (index == oldClipIndex) index = (index + 1) % Voices.Count;
            oldClipIndex = index;
            Voice.clip = Voices[index];
            Voice.Play();
        }
    }
    public void Calculate_CorrectPosition()
    {
        if (Hoop == null) return;

        Vector3 angle = Hoop.transform.localEulerAngles;
        if (angle.y < 0f) angle.y += 360f;
        angle.y -= 180f;
        float offset = -(angle.y / 180f);
        offset += Hoop.transform.position.x / 30f;

        correctPosition = Hoop.transform.Find("CorrectPosition");
        float distanceCorrction = Vector3.Distance(Hoop.transform.position, transform.position);
        distanceCorrction = .17f - Mat.map(distanceCorrction, 0f, 15f, 0f, .17f);

        Vector3 pos = correctPosition.localPosition;
        pos.x = offset * .8f;
        pos.y = correctPositionOrigin.y + Mathf.Abs(offset * 1.2f) + distanceCorrction - .05f;



        correctPosition.localPosition = pos;
    }
    private void Run_FireBallSound()
    {
        CopyBall.GetComponent<HoopAudio>().Fireball.Play();
    }
    private IEnumerator ThrowNewBall(float delayTime, GameObject ball, Vector3 velocity)
    {
        yield return new WaitForSeconds(delayTime);

        CopyBall = Instantiate(ball, transform.position, Ball.transform.rotation, transform);
        CopyBall.transform.localScale = CopyBall.transform.localScale * BallScale;
        Rigidbody rb = CopyBall.GetComponent<Rigidbody>();
        rb.velocity = velocity;

        float x = Random.Range(-8f, 8f);
        float y = Random.Range(-8f, 8f);
        float z = Random.Range(-8f, 8f);
        rb.AddTorque(new Vector3(x, y, z));
    }
    #endregion
    #region Level Controllers
    public void Button_PlayAgain()
    {
        loadLevel = true;
    }
    private void Get_NextIndex()
    {
        nextLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (isWin) nextLevelIndex++;
        nextLevelIndex = nextLevelIndex % SceneManager.sceneCountInBuildSettings;
        if (nextLevelIndex == 0) nextLevelIndex = 7;
    }
    private void SavePrefs()
    {
        PlayerPrefs.SetInt("Level", nextLevelIndex);
        int score = Next_UI.GetComponent<WinEvent>().targetScore;
        int ballIndex = Next_UI.GetComponent<WinEvent>().unlockBallIndex;
        int selectedBallIndex = Next_UI.GetComponent<WinEvent>().selectedBallIndex;



        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Unlock Ball", ballIndex);

        if(Next_UI.GetComponent<WinEvent>().newUnlockBallIndex >= 0)
        {
            string indices = PlayerPrefs.GetString("UnlockedBallIndices");
            indices += Next_UI.GetComponent<WinEvent>().newUnlockBallIndex.ToString();
            PlayerPrefs.SetString("UnlockedBallIndices", indices);
        }
        PlayerPrefs.SetInt("SelectedBallIndex", selectedBallIndex);
    }
    public void LoadLevel()
    {
        SavePrefs();
        loadLevel = true;
    }
    public void Reset_Combo()
    {
        combo = 0;
    }
    public void LoadLevelAsync()
    {
        Get_NextIndex();
        StartCoroutine(LoadLevelAsyncCoroutine());
    }
    private void Reset_R_Button()
    {
        if (Input.GetKeyUp(KeyCode.R)) SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator LoadLevelAsyncCoroutine()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(nextLevelIndex);
        async.allowSceneActivation = false;

        while (!loadLevel)
            yield return null;

        async.allowSceneActivation = true;

        yield return async;
    }
    #endregion

}
public enum Controls
{
    Tap,
    Slide,
    Both
}
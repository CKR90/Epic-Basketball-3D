using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ProgressBar : MonoBehaviour
{
    public bool isDoubleBar = true;
    public GameObject PlayerController;
    public GameObject BotController;
    public GameObject Seperator;
    public Transform SeperatorFolder;
    public RectTransform orangeBall;
    public RectTransform greenBall;
    public RectTransform orangeProgress;
    public RectTransform greenProgress;
    public RectMask2D mask;


    private Throw th;
    private ThrowBot tb;
    private RectTransform myRect;
    private float width = 0f;
    private float widthScale = 0f;

    private float totalSeperator = 0;
    private int scorePlayer = 0;
    private int scoreBot = 0;


    private void Awake()
    {
        if (isDoubleBar && !BotController.activeSelf) Destroy(gameObject);
        if (!isDoubleBar && BotController.activeSelf) Destroy(gameObject);

        myRect = GetComponent<RectTransform>();
        width = myRect.sizeDelta.x;
        widthScale = myRect.sizeDelta.x * myRect.localScale.x;
    }
    private void Start()
    {
        th = PlayerController.GetComponent<Throw>();
        if(isDoubleBar) tb = BotController.GetComponent<ThrowBot>();

        totalSeperator = th.HoopTransforms.Count;
        if (th.ApplyFinalHoop) totalSeperator++;

        for(int i = 0; i < totalSeperator - 1; i++)
        {
            GameObject sep = Instantiate(Seperator, SeperatorFolder.transform);
            RectTransform rt = sep.GetComponent<RectTransform>();
            Vector3 ap = rt.anchoredPosition;
            ap.x = width * ((i + 1) / totalSeperator);
            rt.anchoredPosition = ap;
        }
        float pad = widthScale - (widthScale * (1f / totalSeperator)) + 5f;

        mask.padding = new Vector4(pad, 0f, 0f, 0f);
    }
    private void FixedUpdate()
    {
        CheckPlayerScore();
        CheckBotScore();
    }
    private void CheckPlayerScore()
    {
        if(scorePlayer != th.score)
        {
            Vector2 pos = orangeBall.anchoredPosition;
            
            if (th.score == totalSeperator) pos.x = width - 50f;
            else pos.x = width * (th.score / totalSeperator) - 25f;
            orangeBall.anchoredPosition = Vector2.Lerp(orangeBall.anchoredPosition, pos, Time.deltaTime * 8f);

            Vector2 size = orangeProgress.sizeDelta;
            size.x = pos.x + 25f;
            orangeProgress.sizeDelta = Vector2.Lerp(orangeProgress.sizeDelta, size, Time.deltaTime * 8f);

            if (Vector2.Distance(orangeBall.anchoredPosition, pos) < .2f) scorePlayer = th.score;
        }
    }
    private void CheckBotScore()
    {
        if (tb == null) return;

        if (scoreBot != tb.score)
        {
            Vector2 pos = greenBall.anchoredPosition;

            if (tb.score == totalSeperator) pos.x = width - 50f;
            else pos.x = width * (tb.score / totalSeperator) - 25f;
            greenBall.anchoredPosition = Vector2.Lerp(greenBall.anchoredPosition, pos, Time.deltaTime * 8f);

            Vector2 size = greenProgress.sizeDelta;
            size.x = pos.x + 25f;
            greenProgress.sizeDelta = Vector2.Lerp(greenProgress.sizeDelta, size, Time.deltaTime * 8f);

            if (Vector2.Distance(greenBall.anchoredPosition, pos) < .2f) scoreBot = tb.score;
        }
    }
}

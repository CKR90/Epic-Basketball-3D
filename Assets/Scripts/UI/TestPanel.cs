using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestPanel : MonoBehaviour
{
    public GameObject panel;
    public Slider length;
    public Slider focus;
    public Slider feed;
    public Text tLength;
    public Text tFocus;
    public Text tFeed;
    private Throw th;
    void Start()
    {
        th = GameObject.Find("BallControllerForPlayer").GetComponent<Throw>();
        length.value = th.HorizontalHelperLength;
        focus.value = th.TargetHorizontalFocus;
        feed.value = th.feedbackSize;
    }

    void FixedUpdate()
    {
        tLength.text = th.HorizontalHelperLength.ToString("0.00");
        tFocus.text = th.TargetHorizontalFocus.ToString("0.00");
        tFeed.text = th.feedbackSize.ToString("0.00");

        
    }
    public void Length()
    {
        th.HorizontalHelperLength = length.value;
    }
    public void Focus()
    {
        th.TargetHorizontalFocus = focus.value;
        th.horizontalfocusOld = focus.value;
    }
    public void Feed()
    {
        th.feedbackSize = feed.value;
    }
    public void ButtonEvent()
    {
        panel.SetActive(!panel.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackAutomation : MonoBehaviour
{
    public GameObject combo;
    public SpriteRenderer numberRenderer;
    public List<Sprite> feedbackSprites;
    public List<Sprite> numberSprites;
    
    public void Set_Feedback(float Size)
    {
        Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * Size - Vector3.up * .2f;
        transform.position = pos;
        transform.forward = Camera.main.transform.forward;
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        s.sprite = feedbackSprites[Random.Range(0, 100) % feedbackSprites.Count];
        Destroy(gameObject, 2f);
    }
    public void Set_Combo(int number)
    {
        if (number > 14) return;
        combo.SetActive(true);
        numberRenderer.sprite = numberSprites[number];
    }
}

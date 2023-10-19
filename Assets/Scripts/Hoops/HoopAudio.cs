using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopAudio : MonoBehaviour
{
    public AudioSource HoopPlane;
    public AudioSource HoopRim;
    public AudioSource Basket;
    public AudioSource Bounce;
    public AudioSource Win;
    public AudioSource Lose;
    public AudioSource Fireball;
    public AudioSource HeavyBall;

    private bool fadeout = false;
    void Start()
    {
        
    }
    void Update()
    {
        if(fadeout)
        {
            float v = Win.volume;
            v = Mathf.MoveTowards(v, 0f, Time.deltaTime);
            Win.volume = v;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "Model" && collision.collider.GetType() == typeof(BoxCollider)) HoopPlane.Play();
        if (collision.collider.gameObject.name == "Model" && collision.collider.GetType() == typeof(MeshCollider)) HoopRim.Play();
        if (collision.collider.gameObject.tag == "Floor" && Bounce != null) Bounce.Play();
    }

    public void PlayBasket()
    {
        Basket.Play();
    }
    public void PlayWin()
    {
        Win.Play();
    }
    public void PlayLose()
    {
        Lose.Play();
    }

    public void FadeOutWinSound()
    {
        fadeout = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHand : MonoBehaviour
{
    [Range(.5f, 5f)] public float BlockSpeed = 1.5f;
    public GameObject particle;
    public ParticleSystem ClapParticle;
    public AudioSource Clap;
    [HideInInspector] public float DestroyTime = 0f;
    [HideInInspector] public Vector3 point;
    [HideInInspector] public Transform targetBall;

    private bool destroyDetect = false;
    private Transform hoop;
    private Vector3 bot;
    private Vector3 pointOld;
    private Vector3 throwDir;
    private bool hit = false;
    private float angle = 0f;
    void Start()
    {
        hoop = GameObject.Find("Hoop Bot").transform;
        bot = GameObject.Find("BallControllerForBot").transform.position;
        transform.SetParent(hoop);
        transform.forward = -hoop.transform.forward;
        transform.localPosition = new Vector3(0f, 5.2f, 2.5f);
        point = transform.position;
        pointOld = point;
    }
    void Update()
    {
        ApplyPosition();
        ApplyHit();
    }
    private void ApplyPosition()
    {
        if (pointOld != point)
        {
            if (!destroyDetect && DestroyTime > 0f)
            {
                destroyDetect = true;
                Destroy(gameObject, DestroyTime);
            }

            bot.y = point.y;
            throwDir = (bot - point).normalized;
            transform.forward = -throwDir;
            point = point + throwDir + Vector3.up * .2f;
            pointOld = point;
        }
        transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime * 5f);
    }
    private void ApplyHit()
    {
        if (targetBall == null)
        {
            return;
        }
        
        float dist = Vector3.Distance(transform.position, targetBall.position);
         
        if (!hit && dist < 1.5f)
        {
            hit = true;
        }
        else if(hit)
        {
            angle += Time.deltaTime * 350f * BlockSpeed;
            Vector3 a = transform.localEulerAngles;
            if (angle >= 180f)
            {
                a.x = 0f;
                hit = false;
                targetBall = null;
                angle = 0f;
            }
            else
            {
                a.x = -Mat.sin(angle) * 50f;
            }
            transform.localEulerAngles = a;
        }
    }

    private void OnDestroy()
    {
        particle.transform.SetParent(null);
        particle.GetComponent<ParticleSystem>().Play();
        Destroy(particle, 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ball")
        {
            Clap.Play();
            ClapParticle.Play();
        }
    }
}

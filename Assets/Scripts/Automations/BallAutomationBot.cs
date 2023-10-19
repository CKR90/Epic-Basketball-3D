using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAutomationBot : MonoBehaviour
{
    private bool basketCheck1 = false;
    private bool basketCheck2 = false;
    private bool basketCheck3 = false;

    private bool basketCheckFinal1 = false;
    private bool basketCheckFinal2 = false;
    private bool basketCheckFinal3 = false;

    private bool isAddedScore = false;
    private ThrowBot tb;

    public bool MakeItHeavy = false;
    public ParticleSystem particle;
    private Rigidbody rb;

    private void Start()
    {
        tb = transform.parent.gameObject.GetComponent<ThrowBot>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        BasketCheck();

        if(MakeItHeavy && !rb.isKinematic)
        {
            rb.AddForce(-Vector3.up * Time.deltaTime * 250f, ForceMode.Acceleration);
        }
    }
    private void BasketCheck()
    {
        if (basketCheck1 && basketCheck2 && basketCheck3 && !isAddedScore && !tb.OpponentController.GetComponent<Throw>().ApplyFinalHoop && tb.score == tb.HoopTransforms.Count - 1)
        {
            if (!tb.FinishGame && !tb.OpponentController.GetComponent<Throw>().FinishGame)
            {
                tb.BasketEffectFinal.Play();
                GetComponent<HoopAudio>().PlayBasket();
                GetComponent<HoopAudio>().PlayLose();
                tb.OpponentController.GetComponent<Throw>().Restart_UI.SetActive(true);

                tb.AddPoint();
                tb.FinishGame = true;
                tb.OpponentController.GetComponent<Throw>().FinishGame = true;
                isAddedScore = true;
            }
        }
        else if (basketCheck1 && basketCheck2 && basketCheck3 && !isAddedScore)
        {
            tb.BasketEffect.Play();
            tb.AddPoint();
            GetComponent<HoopAudio>().PlayBasket();            
            isAddedScore = true;
        }
        else if (basketCheckFinal1 && basketCheckFinal2 && basketCheckFinal3 && !isAddedScore)
        {
            tb.BasketEffectFinal.Play();
            GetComponent<HoopAudio>().PlayBasket();
            if (!tb.FinishGame)
            {
                GetComponent<HoopAudio>().PlayLose();
                tb.OpponentController.GetComponent<Throw>().Restart_UI.SetActive(true);
            }
            tb.AddPoint();
            tb.FinishGame = true;
            tb.OpponentController.GetComponent<Throw>().FinishGame = true;
            isAddedScore = true;
        }

    }
    public void ExplodeEvent()
    {
        if(particle != null)
        {
            particle.transform.SetParent(null);
            particle.Play();
            particle.gameObject.GetComponent<AudioSource>().Play();
            Destroy(particle, 3f);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Death Area")
        {
            if (!tb.FinishGame) Destroy(gameObject);
            else
            {
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        if(tb.score < tb.OpponentController.GetComponent<Throw>().HoopTransforms.Count)
        {
            if (other.gameObject.name == "BasketCheckBot1") basketCheck1 = true;
            if (other.gameObject.name == "BasketCheckBot2") basketCheck2 = true;
            if (other.gameObject.name == "BasketCheckBot3") basketCheck3 = true;
        }
        else
        {
            if (other.gameObject.name == "BasketCheckFinal1") basketCheckFinal1 = true;
            if (other.gameObject.name == "BasketCheckFinal2") basketCheckFinal2 = true;
            if (other.gameObject.name == "BasketCheckFinal3") basketCheckFinal3 = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!tb.FinishGame) Destroy(gameObject, 20f);
    }
}

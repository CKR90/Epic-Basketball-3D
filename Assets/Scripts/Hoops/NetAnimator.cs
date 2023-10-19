using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetAnimator : MonoBehaviour
{
    Animator animator;
    bool wait = false;
    float timer = 0f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(wait)
        {
            timer += Time.deltaTime;
            if(timer > .5f)
            {
                animator.SetBool("Basket", false);
                wait = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wait || animator == null) return;
        animator.SetBool("Basket", true);
        timer = 0f;
        wait = true;
    }
}

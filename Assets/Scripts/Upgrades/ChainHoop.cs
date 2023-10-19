using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHoop : MonoBehaviour
{
    public GameObject particle;
    private Throw th;
    [HideInInspector] public float destroyTime = 0f;
    private bool applyDestroy = false; 
    void Start()
    {
        th = GameObject.Find("BallControllerForPlayer").GetComponent<Throw>();
    }

    
    void Update()
    {
        if(!applyDestroy && th.TapToPlay && destroyTime > 0f)
        {
            applyDestroy = true;
            Destroy(gameObject, destroyTime);
        }
    }

    private void OnDestroy()
    {
        particle.transform.SetParent(null);
        Destroy(particle, 3f);
    }
}

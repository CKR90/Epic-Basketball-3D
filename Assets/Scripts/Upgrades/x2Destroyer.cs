using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class x2Destroyer : MonoBehaviour
{
    [HideInInspector] public Throw th;

    void FixedUpdate()
    {
        if(th != null && th.FinishGame)
        {
            Destroy(gameObject);
        }
    }
}

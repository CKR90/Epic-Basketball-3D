using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetCreator : MonoBehaviour
{
    public GameObject net;

    Vector3 localPos = new Vector3(0f, 4.98f, 2.024f);
    Vector3 localRot = new Vector3(0f, 357.6471f, 0f);

    GameObject player, bot, final;
    void Start()
    {

        player = GameObject.Find("Hoop Player");
        bot = GameObject.Find("Hoop Bot");
        final = GameObject.Find("Hoop Final");

        if(player != null)
        {
            GameObject p = Instantiate(net, player.transform);
            p.transform.position = player.transform.position;
            p.transform.localPosition = localPos;
            p.transform.localEulerAngles = localRot;
            p.transform.SetParent(player.transform.Find("HoopPivot"));
        }
        

        if(bot != null /*&& !GameObject.FindObjectOfType<Throw>().SpaceAnimations*/)
        {
            GameObject b = Instantiate(net, bot.transform);
            b.transform.localPosition = localPos;
            b.transform.localEulerAngles = localRot;
            b.transform.SetParent(bot.transform.Find("HoopPivot"));
        }
        

        if(final != null)
        {
            GameObject f = Instantiate(net, final.transform);
            f.transform.localPosition = localPos;
            f.transform.localEulerAngles = localRot;
            f.transform.SetParent(final.transform.Find("HoopPivot"));
        }
        
    }


}

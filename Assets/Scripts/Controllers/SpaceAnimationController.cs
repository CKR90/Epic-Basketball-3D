using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceAnimationController : MonoBehaviour
{
    public Throw th;
    public ThrowBot tb;

    public int animIndex = 0;

    public List<AnimData> animations = new List<AnimData>();

    private bool spaceLock = false;
    private bool runAnim = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (!th.SpaceAnimations) return;
        if(CheckSpace() && !spaceLock)
        {
            //spaceLock = true;
            RunAnim();
        }

    }

    private void RunAnim()
    {
        if (animations.Count <= 0) return;

        animIndex = animIndex % animations.Count;

        if(animations[animIndex].camPos != null)
        {
            Camera.main.transform.position = animations[animIndex].camPos.position;
            Camera.main.transform.rotation = animations[animIndex].camPos.rotation;
        }
        if(animations[animIndex].firstThrow == firstThrow.player)
        {
            throwPlayer();
            Invoke("throwBot", animations[animIndex].timeOffset);
        }
        else
        {
            Invoke("throwPlayer", animations[animIndex].timeOffset);
            throwBot();
        }
        Invoke("Counter", animations[animIndex].timeOffset);
    }
    private void throwPlayer()
    {
        if(animations[animIndex].playerTarget != null)
            th.MouseDown(animations[animIndex].playerTarget, animations[animIndex].playerTorque);
    }
    private void throwBot()
    {
        if (animations[animIndex].botTarget != null)
            tb.ThrowBall(animations[animIndex].botTarget, animations[animIndex].botTorque);
    }
    private void Counter()
    {
        animIndex = (animIndex + 1) % animations.Count;
    }




    private bool CheckSpace()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    [System.Serializable]
    public class AnimData
    {
        public Transform playerTarget;
        public Transform botTarget;
        public Vector3 playerTorque;
        public Vector3 botTorque;
        public Transform camPos;
        public firstThrow firstThrow;
        public float timeOffset;
    }
    public enum firstThrow
    {
        player = 0,
        bot = 1
    }
}

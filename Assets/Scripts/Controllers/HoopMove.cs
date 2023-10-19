using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HoopMove : MonoBehaviour
{
    public List<MoveData> moves;
    void Start()
    {
        foreach(var v in moves)
        {
            GoToEnd(v);
        }
    }

    private void GoToEnd(MoveData d)
    {

        d.Hoop.DOMove(d.End.position, d.moveTime).SetEase(Ease.InOutSine).OnComplete(() => GoToStart(d));
    }
    private void GoToStart(MoveData d)
    {
        d.Hoop.DOMove(d.Start.position, d.moveTime).SetEase(Ease.InOutSine).OnComplete(() => GoToEnd(d));
    }

    [System.Serializable] public class MoveData
    {
        public Transform Hoop;
        public Transform Start;
        public Transform End;
        [Min(1f)]public float moveTime = 10f;
    }
}

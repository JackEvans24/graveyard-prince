using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostieWiggle : MonoBehaviour
{
    [SerializeField] private Vector2 wiggleRoom;
    [SerializeField] private Vector2 wiggleSpeed;
    [SerializeField] private Ease easing;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveX(transform.localPosition.x + wiggleRoom.x, wiggleSpeed.x).SetEase(easing).SetLoops(-1, LoopType.Yoyo);
        transform.DOLocalMoveY(transform.localPosition.y + wiggleRoom.y, wiggleSpeed.y).SetEase(easing).SetLoops(-1, LoopType.Yoyo);
    }
}

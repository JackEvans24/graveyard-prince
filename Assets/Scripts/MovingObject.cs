using DG.Tweening;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Vector2 moveDistance;
    [SerializeField] private float moveDuration;
    [SerializeField] private float delay = 0;
    [SerializeField] private Ease ease;

    private void Start()
    {
        var target = this.transform.position + this.moveDistance.ToVector3();
        this.transform.DOMove(target, this.moveDuration).SetDelay(delay, false).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + moveDistance.ToVector3());
    }
}

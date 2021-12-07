using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 moveDistance;
    [SerializeField] private float speed;
    [SerializeField] private float targetThreshold;

    private Rigidbody2D rb;

    private Vector2 origin, target;
    private bool moveToTarget = true;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();

        this.origin = this.transform.position;
        this.target = this.origin + this.moveDistance;
    }

    private void FixedUpdate()
    {
        if (moveToTarget && Vector2.Distance(target, transform.position) < targetThreshold)
            moveToTarget = !moveToTarget;
        else if (!moveToTarget && Vector2.Distance(origin, transform.position) < targetThreshold)
            moveToTarget = !moveToTarget;

        rb.velocity = moveDistance * (moveToTarget ? 1 : -1) * speed / 100;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            collision.collider.transform.SetParent(transform);
        else if (collision.collider.CompareTag("Princess"))
            collision.collider.transform.parent.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            collision.collider.transform.SetParent(null);
        else if (collision.collider.CompareTag("Princess"))
            collision.collider.transform.parent.SetParent(null);
    }
}

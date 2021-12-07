using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Vector2 checkDirection;
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask layers;

    [NonSerialized] public bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        this.isGrounded = Physics2D.Raycast(transform.position, checkDirection, checkDistance, layers);
    }

    private void OnDrawGizmosSelected()
    {
        var offset = this.checkDirection * this.checkDistance;
        Gizmos.DrawLine(transform.position, transform.position + offset.ToVector3());
    }
}

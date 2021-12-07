using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroll : MonoBehaviour
{
    [SerializeField] private Vector2 boundaries;
    [SerializeField] private float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.right * speed / 1000;

        if (this.speed > 0 && this.transform.position.x > this.boundaries.y)
            this.transform.position = new Vector2(this.boundaries.x, this.transform.position.y);
        else if (this.speed < 0 && this.transform.position.x < this.boundaries.x)
            this.transform.position = new Vector2(this.boundaries.y, this.transform.position.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(boundaries.x, 10), new Vector2(boundaries.x, -10));
        Gizmos.DrawLine(new Vector2(boundaries.y, 10), new Vector2(boundaries.y, -10));
    }
}

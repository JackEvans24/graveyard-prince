using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PrincessController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider2D collider2d;
    [SerializeField] private AudioSource thud;

    [Header("Lifting")]
    [SerializeField] private Vector2 offset;

    private Rigidbody2D rb, parentRb;
    private SpriteRenderer sprite;
    private BloodSplat splat;

    private Transform player;
    private bool dead;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.sprite = GetComponent<SpriteRenderer>();
        this.splat = GetComponentInChildren<BloodSplat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.player != null)
            this.transform.position = this.player.position + this.offset.ToVector3();
        else if (this.transform.parent != null)
        {
            if (parentRb == null)
                parentRb = this.transform.parent.GetComponent<Rigidbody2D>();

            this.rb.velocity = parentRb.velocity;
        }
        else if (parentRb != null)
        {
            parentRb = null;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        thud.Play();

        if (collision.collider.CompareTag("Hazard"))
            StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        if (dead)
            yield break;
        dead = true;

        this.sprite.enabled = false;
        yield return this.splat.Play();

        GameController.ResetLevel();
    }

    public void PickUp(PlayerController player)
    {
        this.player = player.transform;
        this.SetSpriteAndCollisions(false);
    }

    public void Release(Vector2 velocity)
    {
        this.rb.velocity = velocity;

        this.player = null;
        this.SetSpriteAndCollisions(true);
    }

    private void SetSpriteAndCollisions(bool active)
    {
        this.sprite.enabled = active;
        this.collider2d.enabled = active;
    }
}

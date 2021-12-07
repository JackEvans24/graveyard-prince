using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public bool HasPrincess { get => this.activePrincess != null; }

    [Header("References")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GroundCheck[] groundChecks;

    [Header("Movement")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float ladenSpeed = 6;
    [SerializeField] private float jumpSpeed = 8;
    [SerializeField] private float ladenJumpSpeed = 8;
    [SerializeField] private float fallSpeed = 1;
    [SerializeField] private float maxJumpDuration = 0.2f;

    [Header("Throwing")]
    [SerializeField] private Vector2 throwForce;
    [SerializeField] private Vector2 upwardThrowForce;
    [SerializeField] private float throwCooldown = 0.1f;

    private Animator anim;
    private Rigidbody2D rb, parentRb;
    private BloodSplat splat;

    private float horizontal;
    private bool facingLeft, isLadenJump, dead;

    private GraveController nearbyGrave;
    private AltarController nearbyAltar;

    private PrincessController nearbyPrincess;
    private PrincessController activePrincess;

    private float currentJumpDuration, currentThrowCooldown;

    private void Awake()
    {
        this.anim = GetComponent<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        this.splat = GetComponentInChildren<BloodSplat>();

        this.ResetJump();
        this.currentThrowCooldown = this.throwCooldown + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentJumpDuration < this.maxJumpDuration)
            this.currentJumpDuration += Time.deltaTime;
        if (this.currentThrowCooldown < this.throwCooldown)
            this.currentThrowCooldown += Time.deltaTime;

        this.SetDirection();

        if (Input.GetButtonDown("Action"))
            this.DetermineAction();
        else if (Input.GetButtonDown("Throw"))
            this.ThrowPrincess();
    }

    private void FixedUpdate()
    {
        // Get our current speed (changes depending on whether we have a princess)
        var activeSpeed = this.activePrincess == null && this.currentThrowCooldown >= this.throwCooldown ? this.speed : this.ladenSpeed;
        
        // Get our vertical velocity
        var vertical = this.rb.velocity.y;
        if (this.currentJumpDuration < this.maxJumpDuration)
            vertical = this.isLadenJump ? this.ladenJumpSpeed : this.jumpSpeed;
        else if (!this.groundChecks.Any(g => g.isGrounded))
            vertical -= fallSpeed / 100;

        // Check whether we're on a moving platform
        var parentVelocity = Vector2.zero;
        if (this.transform.parent != null)
        {
            if (this.parentRb == null)
                this.parentRb = this.transform.parent.GetComponent<Rigidbody2D>();

            parentVelocity = this.parentRb.velocity;
        }
        else if (this.parentRb != null)
        {
            this.parentRb = null;
        }

        // Set velocity
        var activeVelocity = new Vector2(this.horizontal * activeSpeed, vertical);
        this.rb.velocity = activeVelocity + parentVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.AssignIfTagMatches(ref this.nearbyGrave, "Grave");
        collision.AssignIfTagMatches(ref this.nearbyAltar, "Altar");
        collision.AssignIfTagMatches(ref this.nearbyPrincess, "Princess");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.UnassignIfTagMatches(ref this.nearbyGrave, "Grave");
        collision.UnassignIfTagMatches(ref this.nearbyAltar, "Altar");
        collision.UnassignIfTagMatches(ref this.nearbyPrincess, "Princess");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Hazard"))
            StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        if (dead)
            yield break;
        dead = true;

        this.sprite.enabled = false;
        this.rb.velocity = Vector2.zero;

        yield return this.splat.Play();

        GameController.ResetLevel();
    }

    private void ResetJump()
    {
        this.currentJumpDuration = this.maxJumpDuration + 1;
        this.isLadenJump = false;
    }

    private void SetDirection()
    {
        // Set horizontal speed
        this.horizontal = Input.GetAxis("Horizontal");

        // Check if grounded, reset variables
        var grounded = this.groundChecks.Any(check => check.isGrounded);
        var holdingJump = Input.GetButton("Jump");
        if (grounded)
        {
            if (holdingJump)
            {
                this.currentJumpDuration = 0;
                this.isLadenJump = this.activePrincess != null;
            }
            else if (currentJumpDuration <= maxJumpDuration)
                this.ResetJump();
        }
        else if (!holdingJump)
            this.ResetJump();

        // Set facingLeft
        if ((this.facingLeft && this.horizontal > 0) || (!this.facingLeft && this.horizontal < 0))
            this.Flip();
    }

    private void Flip()
    {
        this.facingLeft = !this.facingLeft;
        this.sprite.transform.Rotate(0f, 180f, 0f);
    }

    private void DetermineAction()
    {
        if (this.nearbyGrave != null && this.TryOpenGrave())
            return;

        if (this.activePrincess != null)
        {
            if (this.nearbyAltar != null)
                this.Sacrifice();
            else
                this.DropPrincess();
        }        
        else if (this.nearbyPrincess != null)
            this.LiftPrincess();
    }

    private bool TryOpenGrave()
    {
        var result = false;

        if (this.nearbyGrave == null)
            return result;

        result = this.nearbyGrave.TryGetPrincess(out var princessObj);
        if (!result)
            return result;

        this.nearbyPrincess = princessObj.GetComponent<PrincessController>();
        this.LiftPrincess();

        return result;
    }

    private void Sacrifice()
    {
        StartCoroutine(this.nearbyAltar.Sacrifice(this.activePrincess));

        this.activePrincess = null;
        this.SetAnimationVariables();

        this.sprite.enabled = false;
        this.rb.velocity = Vector2.zero;
        this.enabled = false;
    }

    private void LiftPrincess()
    {
        if (nearbyPrincess == null)
            return;

        this.nearbyPrincess.PickUp(this);
        this.activePrincess = this.nearbyPrincess;

        if (!this.groundChecks.Any(check => check.isGrounded))
            this.isLadenJump = true;

        this.SetAnimationVariables();
    }

    private void DropPrincess()
    {
        if (this.activePrincess == null)
            return;

        this.activePrincess.Release(this.rb.velocity);
        this.activePrincess = null;

        this.SetAnimationVariables();
    }

    private void ThrowPrincess()
    {
        if (this.activePrincess == null)
            return;
        else if (this.currentThrowCooldown <= this.throwCooldown)
            return;

        var throwVelocity = this.GetThrowVelocity();

        this.activePrincess.Release(throwVelocity);
        this.activePrincess = null;

        this.currentThrowCooldown = 0;

        this.SetAnimationVariables();
    }

    private Vector2 GetThrowVelocity()
    {
        var throwCoefficient = 1 + Mathf.Abs(this.horizontal);
        throwCoefficient = this.facingLeft ? throwCoefficient * -1 : throwCoefficient;

        Vector2 throwVelocity = Vector2.zero;
        if (Input.GetAxis("Vertical") > 0)
            throwVelocity = new Vector2(this.upwardThrowForce.x * throwCoefficient, this.upwardThrowForce.y);
        else
            throwVelocity = new Vector2(this.throwForce.x * throwCoefficient, this.throwForce.y);

        return throwVelocity;
    }

    private void SetAnimationVariables()
    {
        this.anim.SetBool("HasPrincess", this.HasPrincess);
    }
}

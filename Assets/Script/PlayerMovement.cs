using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // 
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private TrailRenderer trailRenderer;
    
    // 
    private int wholeNumber = 16;
    private float decimalNumber = 4.54f;
    private string text = "blabla";
    private bool boolean = false;
    private float dirX = 0f;
    private float dirY = 0f;
    
    // move & jump speed
    private float moveSpeed = 7f;
    private float jumpSpeed = 14f;
    
    // ground check
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    
    // doubleJump;
    private bool doubleJump;
    
    // dash
    private float dashingVelocity = 14f;
    private float dashingTime = .5f;

    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash=true;
    // 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var dashInput = Input.GetButtonDown("Dash");
        var jumpDownInput = Input.GetButtonDown("Jump");
        var jumpInput = Input.GetButton("Jump");
        // getAxisRaw: make the velocity 0 immediately 
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            trailRenderer.emitting = true;
            dashingDir = dashingDir = new Vector2(dirX, dirY);
            if (dashingDir == Vector2.zero)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }
            // add stopping dash
            StartCoroutine(StopDashing());
        }
        
        anim.SetBool("isDashing", isDashing);
        if (isDashing)
        {
            rb.velocity = dashingDir.normalized * dashingVelocity;
            return;
        }
        if (IsGrounded())
        {
            canDash = true;
        }
        // update double jump status
        if (IsGrounded() && !jumpInput)
        {
            doubleJump = false;
        }
        if (jumpDownInput)
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(0, jumpSpeed);
                doubleJump = !doubleJump;
            }
        }
        UpdateAnim();
    }

    // update the running condition
    void UpdateAnim()
    {
        if (dirX > 0f)
        {
            anim.SetBool("running", true);
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            anim.SetBool("running", true);
            sprite.flipX = true;
        }
        else
        {
            anim.SetBool("running", false);
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false;
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
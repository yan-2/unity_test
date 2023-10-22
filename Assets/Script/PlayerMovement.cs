using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private int wholeNumber = 16;

    private float decimalNumber = 4.54f;

    private string text = "blabla";

    private bool boolean = false;
    private bool doubleJump;
    private float dirX=0f;
    
    // for ground check
    [SerializeField] private LayerMask jumpableGround;
    private float moveSpeed = 7f;
     private float jumpSpeed = 14f;
     private BoxCollider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        // getAxisRaw: make the velocity 0 immediately 
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed,rb.velocity.y);
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(0, jumpSpeed);
                doubleJump = !doubleJump;
            }
        }
        UpdateAnim();
        
    }

    void UpdateAnim()
    {
        if (dirX > 0f)
        {
            anim.SetBool("running",true);
            sprite.flipX = false;
        }
        else if(dirX<0f)
        {
            anim.SetBool("running",true);
            sprite.flipX = true;
        }
        else
        {
            anim.SetBool("running",false); 
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Properties   
    public float moveSpeed = 10f;
    public float jumpForce = 16f;
    public SpriteRenderer playerSprite;

    float horizontalMove, relativePos;
    bool jumpAbility;
    Rigidbody2D body;
    CircleCollider2D feetPos;
    Animator animator;
    GameObject firePoint;   

    private bool facingRight = true;

    // Methods
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        feetPos = GetComponent<CircleCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        firePoint = transform.Find("1_fire_point").gameObject;
        animator = GetComponent<Animator>();
    }

    void Update()
    {        
        UpdateSpriteOrientation();
        UpdateAnimation();
    }

    // MOVEMENT PHYSICS  
    private void FixedUpdate()
    {
        horizontalMove = Input.GetAxisRaw("D-PadH");
        // MOVEMENT
        body.velocity = new Vector2(horizontalMove * moveSpeed, body.velocity.y);

        HandleJumpInput();
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("BtnA")) 
        {
            if (jumpAbility == true)
            {
                body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpAbility = false;
            }
        }
    }

    private void UpdateSpriteOrientation()
    {
        if ((horizontalMove < 0 || body.velocity.x < 0) && facingRight == true)
        {
            facingRight = false;
            transform.Rotate(0, -180, 0);
            firePoint.transform.localRotation = transform.rotation;
        }
        else if ((horizontalMove > 0 || body.velocity.x > 0) && facingRight == false)
        {
            facingRight = true;
            transform.Rotate(0, 180, 0);
            firePoint.transform.localRotation = transform.rotation;
        }
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("horizontalMove", horizontalMove);
        animator.SetFloat("xVelocity", body.velocity.x);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform") jumpAbility = true;        
    }
}
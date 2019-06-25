using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Properties   
    public float moveSpeed = 10f;
    public float jumpForce = 16f;

    float horizontalMove;
    bool jumpAbility;

    Rigidbody2D body;
    CircleCollider2D feetPos;
    public SpriteRenderer playerSprite;
    GameObject firePoint;
    float relativePos;

    private bool facingRight = true;

    // Methods
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        feetPos = GetComponent<CircleCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        firePoint = transform.Find("1_fire_point").gameObject;
    }
    
    void Update()
    {               
        UpdateSpriteOrientation();
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
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
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
        if (horizontalMove < 0 || body.velocity.x < 0)
        {
            playerSprite.flipX = true;
            firePoint.transform.localPosition = this.transform.right * relativePos;
        }
        else if (horizontalMove > 0 || body.velocity.x > 0)
        {
            playerSprite.flipX = false;
            firePoint.GetComponent<SpriteRenderer>().flipX = false;
            firePoint.transform.localPosition = this.transform.right * relativePos;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            jumpAbility = true;
        }
    }
}
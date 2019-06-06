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
    SpriteRenderer playerSprite;

    // Methods
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        feetPos = GetComponent<CircleCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// GET AXIS AND FLIP SPRITE
    /// </summary>
    void Update()
    {
        // MOVEMENT
        horizontalMove = Input.GetAxisRaw("D-PadH");

        // FLIP SPRITE
        //if (horizontalMove < 0 || body.velocity.x < 0)
        //{
        //    playerSprite.flipX = true;
        //}
        //else if (horizontalMove > 0 || body.velocity.x > 0)
        //{
        //    playerSprite.flipX = false;
        //}
    }

    // MOVEMENT PHYSICS  
    private void FixedUpdate()
    {

        // MOVEMENT
        body.velocity = new Vector2(horizontalMove * moveSpeed, body.velocity.y);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            jumpAbility = true;
        }
    }
}
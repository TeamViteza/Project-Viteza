using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerMovement : MonoBehaviour
{

    // Properties
    SpriteRenderer spriteFlipper;

    Rigidbody2D body;
    CircleCollider2D feetPos;

    float moveSpeed = 10f;
    float horizontalMove;
    float jumpForce = 8f;
    bool jumpAbility;

    // Methods
    void Start()
    {
        spriteFlipper = GetComponent<SpriteRenderer>();

        body = GetComponent<Rigidbody2D>();
        feetPos = GetComponent<CircleCollider2D>();
    }
    /// <summary>
    /// GET AXIS AND FLIP SPRITE
    /// </summary>
    void Update()
    {
        // MOVEMENT
        horizontalMove = Input.GetAxisRaw("D-PadH");

        // FLIP SPRITE
        if (horizontalMove < 0 || body.velocity.x < 0)
        {
            spriteFlipper.flipX = true;
        }
        else if (horizontalMove > 0 || body.velocity.x > 0)
        {
            spriteFlipper.flipX = false;
        }
    }
    /// <summary>
    /// MOVEMENT PHYSICS
    /// </summary>
    private void FixedUpdate()
    {
        // MOVEMENT
        body.velocity = new Vector2(horizontalMove * moveSpeed, body.velocity.y);

        // JUMP
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (jumpAbility == true)
            {
                body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpAbility = false;
            }
        }
    }
    /// <summary>
    /// CHECK IF TOUCHING PLATFORM
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            jumpAbility = true;
        }
    }
}

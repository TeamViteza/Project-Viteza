using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerMovement : MonoBehaviour
{

    // Properties
    SpriteRenderer spriteFlipper;

    Rigidbody2D body;
    CircleCollider2D feetPos;

    float moveSpeed = 8f;
    float horizontalMove;
    float jumpForce = 4f;
    bool jumpAbility;

    // Methods
    void Start()
    {
        spriteFlipper = GetComponent<SpriteRenderer>();

        body = GetComponent<Rigidbody2D>();
        feetPos = GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        // MOVEMENT
        horizontalMove = Input.GetAxisRaw("D-PadH");

        if (horizontalMove < 0 || body.velocity.x < 0)
        {
            spriteFlipper.flipX = true;
        }
        else if (horizontalMove > 0 || body.velocity.x > 0)
        {
            spriteFlipper.flipX = false;
        }
    }
    private void FixedUpdate()
    {
        // MOVEMENT
        body.AddForce(new Vector2(horizontalMove * moveSpeed, 0));

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            body.AddForce(new Vector2(-moveSpeed, 0));
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            body.AddForce(new Vector2(moveSpeed, 0));
        }

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            jumpAbility = true;
        }
    }
}

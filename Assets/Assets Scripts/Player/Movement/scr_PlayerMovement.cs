using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerMovement : MonoBehaviour
{

    // Properties
    // public
    public Rigidbody2D body;

    public float jumpForce = 2f;

    public float walkSpeed;
    public float runSpeed;

    // private
    private bool jumbAbility;

    private float horizontalMovement;

    private float maxWalk;
    private float maxRun;

    private float moveSpeed;
    private float maxSpeed;

    // Methods
    void Start()
    {
        maxWalk = 2f;
        maxRun = 6;
    }


    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        body.velocity += new Vector2(horizontalMovement * moveSpeed, 0);

    }
    private void FixedUpdate()
    {
        SpeedManagement();

        if (Input.GetKey(KeyCode.Space) && jumbAbility)
        {
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumbAbility = false;
        }
    }
    void SpeedManagement()
    {
        if (body.velocity.x < maxWalk && body.velocity.x > -maxWalk)
        {
            moveSpeed = walkSpeed;
            maxSpeed = maxWalk;
        }
        else if (body.velocity.x < maxRun && body.velocity.x > -maxRun)
        {
            moveSpeed = runSpeed;
            maxSpeed = maxRun;
        }
        else if (body.velocity.x > maxSpeed && body.velocity.x < -maxRun)
        {
            moveSpeed = 0;
            body.velocity = new Vector2(maxSpeed, body.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {

            jumbAbility = true;
        }
    }
}

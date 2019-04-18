using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerMovement : MonoBehaviour
{

    // Properties
    // public
    public Rigidbody2D body;
    public CapsuleCollider2D feetPos;

    public float jumpForce = 2f;

    public float walkSpeed = 0.01f;
    public float runSpeed = 0.05f;
    public float sprintSpeed = 1f;

    // private
    private bool jumbAbility;

    private float horizontalMovement;

    private float maxWalk;
    private float maxRun;
    private float maxSprint;

    private float moveSpeed;
    private float maxSpeed;

    // Methods
    void Start()
    {
        maxWalk = 5f;
        maxRun = 15f;
        maxSprint = 30f;

        //body = GetComponent<Rigidbody2D>();
        //feetPos = GetComponent<CapsuleCollider2D>();
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
        //else if (body.velocity.x < maxSprint && body.velocity.x > -maxSprint)
        //{
        //    moveSpeed = sprintSpeed;
        //    maxSpeed = maxSprint;
        //}
        else if (body.velocity.x > maxSpeed && body.velocity.x < -maxRun)
        {
            moveSpeed = 0;
            body.velocity = new Vector2(maxSpeed, 0);
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

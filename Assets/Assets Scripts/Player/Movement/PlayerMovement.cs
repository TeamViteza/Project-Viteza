using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Properties   
    public float moveSpeed = 10f;
    public float jumpForce = 16f;
    public SpriteRenderer playerSprite;
    public float OrientationH; // Katt's horizontal orientation. (Is she facing left or right?)

    public float horizontalMove;
    //float horizontalMove, relativePos;
    bool jumpAbility;
    Rigidbody2D body;
    CircleCollider2D feetPos;
    Animator animator;
    GameObject firePoint;
    SMovement sMovement;

    public bool FacingRight = true;

    // Methods
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        feetPos = GetComponent<CircleCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        firePoint = transform.Find("1_fire_point").gameObject;
        animator = GetComponent<Animator>();
        sMovement = GetComponent<SMovement>();
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
        if ((horizontalMove < 0 || body.velocity.x < 0) && FacingRight == true)
        {            
            //transform.Rotate(0, -180, 0);
            //sMovement.DefaultRotation = transform.rotation; // Set the default rotation of our S Movement. (This allows us to keep Katt facing whichever direction she's in.)
            //OrientationH = Mathf.Clamp(-180, -180, -180);
            OrientationH = -180;
            sMovement.Invoke("UpdateOrientation", 0);
            firePoint.transform.localRotation = transform.rotation; // Hmm, maybe I should move this to S Movement.
            FacingRight = false;
        }
        else if ((horizontalMove > 0 || body.velocity.x > 0) && FacingRight == false)
        {            
            //transform.Rotate(0, 180, 0);
            //sMovement.DefaultRotation = transform.rotation; // Set the default rotation of our S Movement. (This allows us to keep Katt facing whichever direction she's in.)
            OrientationH = 180;
            sMovement.Invoke("UpdateOrientation", 0);
            firePoint.transform.localRotation = transform.rotation;
            FacingRight = true;
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;
        animator.SetFloat("horizontalMove", horizontalMove);
        animator.SetFloat("xVelocity", body.velocity.x);
        animator.SetFloat("yVelocity", body.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform") jumpAbility = true;
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMovement : MonoBehaviour
{  // http://info.sonicretro.org/SPG:Solid_Tiles Used for reference here.
    #region  Variables   
    // Public
    public float moveSpeed = 10f;
    public float jumpForce = 16f;    

    // Private
    float horizontalMove;
    float orientationH; // Katt's horizontal orientation. (Is she facing left or right?) 
    float xPos, yPos; // The X and Y co-ordinates of Katt's center.
    float xSpeed, ySpeed, gSpeed; // Katt's horizontal, vertical and ground speed.
    float slope; // The current slope factor in use.
    float angle; // Katt's angle on the ground.   
    float groundRayDistance, groundRayOffsetY;
    Vector2 groundRayDirection; // The direction the ground ray will point in.    
    bool facingRight, jumpCapable, airborne; // For keeping track of whether or not Katt is in mid-air.

    // Constants
    const float acc = 0.046875f;
    const float dec = 0.5f;
    const float frc = 0.046875f;
    const float top = 6;
    const float jmp = 6.5f;
    const float slp = 0.125f;
    const float slpRollUp = 0.078125f;
    const float slpRollDown = 0.046875f;
    const float fall = 2.5f;

    // Components
    SpriteRenderer sprite;
    Vector3 spriteCenter;
    Rigidbody2D body;
    Quaternion DefaultRotation; // Katt's default rotation, she will revert to this whenever she is airborne.
    CircleCollider2D testFeetCollider; // From the old script. I'll likely get rid of it soon and use the sensors instead.
    GameObject blasterGO; // Katt's blaster, used to fire projectiles.
    WeaponFire blasterSC; // Katt's blaster script. Likely won't need the GO in future. We'll see.
    Animator animator;


    // Children    
    Sensor[] sensors = new Sensor[4]; // Increase to 6 when E and F are added.
    #endregion

    // Methods
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        testFeetCollider = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        spriteCenter = sprite.bounds.center;
        facingRight = true;
        blasterGO = transform.Find("1_blaster").gameObject;
        blasterSC = blasterGO.GetComponent<WeaponFire>();

        animator = GetComponent<Animator>();        
        
        DefaultRotation = transform.rotation;

        GroundRayInitialisation();
        SensorInitialisation();
    }

    void Update() // Used to update game elements not related to physics.
    {
        Debug.Log("Facing right: " + facingRight);
        UpdateSpriteOrientation();
        UpdateAnimation();
    }

    void FixedUpdate() // Used to update physics-related game elements.
    {
        HandleMovement();        
        GroundRayUpdate();
        SensorAirborneCheck();        
        RevertRotation();       
    }

    #region MOVEMENT METHODS
    private void HandleMovement()
    {
        horizontalMove = Input.GetAxisRaw("D-PadH");        
        body.velocity = new Vector2(horizontalMove * moveSpeed, body.velocity.y);
        HandleJumpInput();
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("BtnA"))
        {
            if (jumpCapable == true)
            {
                body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                jumpCapable = false;
            }
        }
    }
    #endregion

    #region ROTATION & ORIENTATION METHODS
    private void RevertRotation() // Upon becoming airborne, we want Katt to be upright. (No longer slanted at an angle if she was jumping off of a slope.)
    {
        if (airborne == true)
        {                                               
            int orientationValue = 0; // This value will remain at 0 if Katt's facing the right side of the screen.
            if (!facingRight) orientationValue = -180; // If Katt's facing the left of the screen, change this value to -180 so that she can face the appropriate direction upon reverting her rotation.

            Quaternion revertedRotation = new Quaternion(DefaultRotation.eulerAngles.x, orientationValue, DefaultRotation.eulerAngles.z, 0); // Determine the quaternion Katt's rotation should revert to.            

            transform.rotation = revertedRotation; // Revert Katt's rotation.
            //blasterGO.transform.localRotation = transform.rotation;
        }
    }   
    private void UpdateSpriteOrientation()
    {
        if ((horizontalMove < 0 || body.velocity.x < 0) && facingRight == true)
        {
            orientationH = -180;            
            transform.Rotate(0, orientationH, 0);            
            facingRight = false;
            //blasterGO.transform.localRotation = transform.rotation; 
            blasterSC.ToggleOrientation();
        }
        else if ((horizontalMove > 0 || body.velocity.x > 0) && facingRight == false)
        {
            orientationH = 180;            
            transform.Rotate(0, orientationH, 0);           
            facingRight = true;
            //blasterGO.transform.localRotation = transform.rotation;
            blasterSC.ToggleOrientation();
        }

        //blasterGO.transform.localRotation = transform.rotation;
    }
    #endregion    

    #region SENSOR METHODS
    private void SensorAirborneCheck() // Check whether Katt's currently airborne.
    {
        if (sensors[0].Activated || sensors[1].Activated) airborne = false;
        else if (!sensors[0].Activated && !sensors[1].Activated) airborne = true;
    }
    #endregion

    #region GROUND RAYCAST METHODS
    public RaycastHit2D CheckGroundRaycast()
    {
        Vector2 groundRayStartPos = new Vector2(transform.position.x, transform.position.y + groundRayOffsetY);
        return Physics2D.Raycast(groundRayStartPos, groundRayDirection, groundRayDistance, LayerMask.GetMask("Platform"));
    }
    private void GroundRayUpdate()
    {
        if (airborne) return;
        RaycastHit2D hit = CheckGroundRaycast();

        if (hit.collider)
        {
            //Debug.Log("Hit this object: " + hit.collider.name);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + groundRayOffsetY), hit.normal, Color.yellow);
            transform.rotation = Quaternion.FromToRotation(-transform.up, hit.normal) * transform.rotation;            
        }
    }
    #endregion

    #region ANIMATION METHODS
    private void UpdateAnimation()
    {
        if (animator == null) return;
        animator.SetFloat("horizontalMove", horizontalMove);
        animator.SetFloat("xVelocity", body.velocity.x);
        animator.SetFloat("yVelocity", body.velocity.y);
    }
    #endregion

    #region COLLISION METHODS
    private void OnCollisionEnter2D(Collision2D collision) // Might replace this using sensors.
    {
        if (collision.gameObject.tag == "Platform") jumpCapable = true;
    }
    #endregion

    #region INITIALISATION METHODS
    private void SensorInitialisation()
    {   // Get access to Katt's sensors.     
        GameObject sensorParent = transform.Find("0_sensors").gameObject;

        for (int i = 0; i < sensors.Length; i++)
        {
            sensors[i] = sensorParent.transform.GetChild(i).GetComponent<Sensor>();
        }        

        //Debug.Log("Distance from center to right: " + sprite.bounds.extents.x);
        //Debug.Log("Distance from center to left: " + -sprite.bounds.extents.x);
        //Debug.Log("Distance from center to top: " + sprite.bounds.extents.y);
        //Debug.Log("Distance from center to bottom: " + -sprite.bounds.extents.y);
    }
    private void GroundRayInitialisation()
    {   // Ground-detecting raycast setup.
        groundRayDistance = 1; // The distance the raycast should travel. 
        groundRayOffsetY = -1.1f; // The Y offset from Katt's transform. The ray will be fired from Katt's transform plus this Y offset value. 
        groundRayDirection = new Vector2(0, 1); // The ray will fire downwards from Katt's center.       
    }    
    #endregion
}
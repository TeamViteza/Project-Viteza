using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMovement : MonoBehaviour
{  // http://info.sonicretro.org/SPG:Solid_Tiles Used for reference here.
    #region  Variables   
    // Public
    public Quaternion DefaultRotation; // Katt's default rotation, she will revert to this whenever she is airborne. This'll be public until I merge a lot of P Movement's functions into S Movement.

    // Private
    float xPos, yPos; // The X and Y co-ordinates of Katt's center.
    float xSpeed, ySpeed, gSpeed; // Katt's horizontal, vertical and ground speed.
    float slope; // The current slope factor in use.
    float angle; // Katt's angle on the ground.   
    float groundRayDistance, groundRayOffsetY;
    Vector2 groundRayDirection; // The direction the ground ray will point in.    
    bool airborne; // For keeping track of whether or not Katt is in mid-air.

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
    PlayerMovement pMovement; // I'll have this component temporarily, until I merge the two scripts.

    // Children    
    Sensor[] sensors = new Sensor[4]; // Increase to 6 when E and F are added.
    #endregion

    // Methods
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        spriteCenter = sprite.bounds.center;
        pMovement = GetComponent<PlayerMovement>();
        DefaultRotation = transform.rotation;

        GroundRayInitialisation();
        SensorInitialisation();
    }

    void FixedUpdate()
    {       
        GroundRayUpdate();
        SensorAirborneCheck();
        RevertRotation();       
    }

    #region ROTATION & ORIENTATION METHODS
    private void RevertRotation() // Upon becoming airborne, we want Katt to be upright. (No longer slanted at an angle if she was jumping off of a slope.)
    {
        if (airborne == true)
        {                                    
            Quaternion revertedRotation = new Quaternion(); // The rotation Katt will revert to upon becoming airborne.
            int orientationValue = 0; // This value will remain at 0 if Katt's facing the right side of the screen.

            if (!pMovement.FacingRight) orientationValue = -180; // If Katt's facing the left of the screen, change this value to -180 so that she can face the appropriate direction upon reverting her rotation.

            revertedRotation = new Quaternion(DefaultRotation.eulerAngles.x, orientationValue, DefaultRotation.eulerAngles.z, 0); // Determine the quaternion Katt's rotation should revert to.            

            transform.rotation = revertedRotation; // Revert Katt's rotation.
        }
    }
    private void UpdateOrientation() // Ensure Katt's facing in the direction she's moving.
    {
        transform.Rotate(0, pMovement.OrientationH, 0);
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
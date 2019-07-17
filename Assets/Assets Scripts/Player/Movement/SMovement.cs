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
    bool grounded; // Is Katt in mid-air? If not, this bool will be true.

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
        //Debug.Log("Grounded: " + grounded);
        GroundRayUpdate();
        SensorPlatformCheck();
        RevertRotation();       
    }

    private void RevertRotation()
    {
        if (grounded == false)
        {
            //transform.rotation = DefaultRotation;
            Debug.Log("Reverting Rotation.");
            transform.Rotate(DefaultRotation.eulerAngles.x, pMovement.OrientationH, DefaultRotation.eulerAngles.z); // More tinkering to be done here.
            UpdateOrientation();
        }
    }

    private void UpdateOrientation()
    {
        //Debug.Log("Updating Orientation to Y " + pMovement.OrientationH);
        transform.Rotate(0, pMovement.OrientationH, 0);
        //DefaultRotation = transform.rotation;
    }

    #region SENSOR METHODS
    private void SensorPlatformCheck()
    {
        if(sensors[0].Activated || sensors[1].Activated) grounded = true;       
        else grounded = false;
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
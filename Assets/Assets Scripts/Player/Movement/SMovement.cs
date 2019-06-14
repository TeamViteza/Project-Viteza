using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMovement : MonoBehaviour
{  // http://info.sonicretro.org/SPG:Solid_Tiles Used for reference here.

    // Variables
    float xPos, yPos; // The X and Y co-ordinates of Katt's center.
    float xSpeed, ySpeed, gSpeed; // Katt's horizontal, vertical and ground speed.
    float slope; // The current slope factor in use.
    float angle; // Katt's angle on the ground.

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

    // Children    
    GameObject[] sensors = new GameObject[4]; // Increase to 6 when E and F are added.

    // Methods
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        sprite = GetComponent<SpriteRenderer>();
        spriteCenter = sprite.bounds.center;

        // Get access to Katt's sensors.
        GameObject sensorParent = transform.Find("0_sensors").gameObject;

        for (int i = 0; i < sensors.Length; i++)
        {
            sensors[i] = sensorParent.transform.GetChild(i).gameObject;
        }

        //foreach(GameObject go in sensors)
        //{
        //    Debug.Log(go.name);
        //}



        Debug.Log("Distance from center to right: " + sprite.bounds.extents.x);
        Debug.Log("Distance from center to left: " + -sprite.bounds.extents.x);
        Debug.Log("Distance from center to top: " + sprite.bounds.extents.y);
        Debug.Log("Distance from center to bottom: " + -sprite.bounds.extents.y);
    }

    void Update()
    {
        if (sensors[0].GetComponent<Sensor>().Activated)
        {
            body.gravityScale = 0.0f;
        }
        
    }
}
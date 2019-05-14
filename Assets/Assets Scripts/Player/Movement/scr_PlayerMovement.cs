using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerMovement : MonoBehaviour
{

    // Properties
    Rigidbody2D body;

    float moveSpeed = 8f;

    float horizontalMove;

    // Methods
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("D-PadH");

        body.AddForce(new Vector2(horizontalMove * moveSpeed, 0));

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            body.AddForce(new Vector2(-moveSpeed, 0));
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            body.AddForce(new Vector2(moveSpeed, 0));
        }
    }
    private void FixedUpdate()
    {
        Debug.Log(gameObject.transform.position.ToString());
        Debug.Log(horizontalMove.ToString());
        Debug.Log(moveSpeed.ToString());
    }
}

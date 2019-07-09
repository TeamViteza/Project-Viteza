using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour {

    Rigidbody2D body;
    CircleCollider2D feetPos;

    float jumpForce = 5f;
    bool doubleJump = false;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        feetPos = GetComponent<CircleCollider2D>();
	}
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (body.velocity.y <= 0)
            {
                if (doubleJump == true)
                {
                    body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }
            
        }
	}
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            doubleJump = true;
        }
    }
}

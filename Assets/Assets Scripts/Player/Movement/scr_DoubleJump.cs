using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_DoubleJump : MonoBehaviour {

    Rigidbody2D body;
    BoxCollider2D detectBox;

    float jumpForce = 2f;
    bool jumpAbility;
    bool doubleJump = false;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        detectBox = GetComponent<BoxCollider2D>();
	}
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space) && jumpAbility == true)
        {
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpAbility = false;
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

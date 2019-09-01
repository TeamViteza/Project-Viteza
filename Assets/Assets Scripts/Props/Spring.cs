using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {
    public int springForce = 15;
    bool pressed;

	void Start () {
        pressed = false;
	}
		
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerBody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerBody.AddForce(new Vector2(0, springForce), ForceMode2D.Impulse);          
        }
    }
}

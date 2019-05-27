using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 200f;
    public Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb.velocity = transform.right * speed;
	}
	
	// Update is called once per frame
}

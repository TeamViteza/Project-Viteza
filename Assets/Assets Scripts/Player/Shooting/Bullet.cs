using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 200f;
    public Rigidbody2D rb;
    public GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<SpriteRenderer>().flipX == true)
        {
            rb.velocity = -transform.right * speed;
        }
        else
        {
            rb.velocity = transform.right * speed;
        }
       
	}
	
	// Update is called once per frame
}

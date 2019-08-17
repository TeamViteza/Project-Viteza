using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {   
    public float speed = 200f;
    public int damage = 100;
    public Rigidbody2D rb;
	
	void Start () {
        //Debug.Log("Rotation Y: " + transform.rotation.y + "\nLocal Rotation Y: " + transform.localRotation.y);

        if(transform.rotation.y == 0) rb.velocity = new Vector2(speed, 0); // Gotta ensure here that the bullet is always firing in the proper direction.
        else rb.velocity = new Vector2(-speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Sensor") return; // We don't want our bullets colliding with Katt's Sensors, since they are trigger collisions.

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(100);
        }
        Destroy(gameObject); // This bullet will disappear upon hitting a collision.
    }   
}
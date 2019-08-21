using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {   
    public float speed = 200f;
    public int damage = 100;
    public Rigidbody2D rb;
	
	void Start () {        
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Sensor") return; // We don't want our projectiles colliding with Katt's Sensors, since they are trigger collisions.

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(100);
        }
        Destroy(gameObject); // This projectile will disappear upon hitting a collision.
    }   
}
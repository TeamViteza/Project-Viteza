using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector2 flipDirection;
    public float speed = 200f;
    public int damage = 100;
    public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        flipDirection = new Vector2();

        rb.velocity = new Vector2(speed * transform.parent.transform.position.x, 0);

        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(100);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
}

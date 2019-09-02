using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour {

    Rigidbody2D parentBody; // The body of this object's parent, which will be an icicle hazard.
	
	void Start () {
        parentBody = GetComponentInParent<Rigidbody2D>();        
	}
		
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            parentBody.gravityScale = 1;
        }
    }
}

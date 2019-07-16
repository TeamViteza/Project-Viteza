using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

    public bool Activated;

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.tag == "Platform")
        //{
        //    Debug.Log("Sensor Activated");
        //    Activated = true;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Platform")
        //{
        //    Debug.Log("Sensor De-activated");
        //    Activated = false;
        //}
    }
}
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
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Hazard")
        {            
            Activated = true;
            //Debug.Log(gameObject.name.ToUpper() + " Activated");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Hazard")
        {            
            Activated = false;
            //Debug.Log(gameObject.name.ToUpper() + " De-activated");
        }
    }
}
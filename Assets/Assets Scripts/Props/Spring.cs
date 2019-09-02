using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {
    public int springForce = 15;
    bool pressed;

    [EventRef]
    public string spring = "event:/Master/SFX/jump/spring";
    EventInstance springevent;

    void Start () {
        pressed = false;
        springevent = RuntimeManager.CreateInstance(spring);
    }
		
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {


            RuntimeManager.PlayOneShot(spring);
            Rigidbody2D playerBody = collision.gameObject.GetComponent<Rigidbody2D>();
            playerBody.AddForce(new Vector2(0, springForce), ForceMode2D.Impulse);          
        }
    }
}

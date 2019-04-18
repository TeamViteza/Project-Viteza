using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    private int lives = 3;
    public Text XH;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        XH.text = "x" + lives;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lives--;
        }
	}
}

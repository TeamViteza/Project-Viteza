using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CameraFollowPlayer : MonoBehaviour {

    public GameObject target;

	// Update is called once per frame
	void Update ()
    {
        this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, this.transform.position.z);
	}
}

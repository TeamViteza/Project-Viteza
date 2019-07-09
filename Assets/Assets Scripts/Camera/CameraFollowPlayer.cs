using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    GameObject target; // The game camera will follow this object.

    void Start()
    {
        FindKatt();
    }

    void Update ()
    {
        this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, this.transform.position.z);
	}

    void FindKatt()
    {
        GameObject[] gameObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].name.Contains("katt"))
            {
                target = gameObjects[i];
                break;
            }
        }
    }
}
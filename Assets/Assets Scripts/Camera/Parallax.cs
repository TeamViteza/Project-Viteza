using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{   // Script derived from this Parallax tutorial: https://www.youtube.com/watch?v=zit45k6CUMk
    private float lengthH, lengthV, startPosH, startPosV;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPosH = transform.position.x;
        lengthH = GetComponent<SpriteRenderer>().bounds.size.x;

        startPosV = transform.position.y;
        lengthV = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPosH + dist, transform.position.y, transform.position.z);

        if (temp > startPosH + lengthH) startPosH += lengthH;
        else if (temp < startPosH - lengthH) startPosH -= lengthH;

        //float tempV = (cam.transform.position.y * (1 - parallaxEffect));
        //float distV = (cam.transform.position.y * parallaxEffect);
        //// Vertical testing.
        //if (!(tempV >= startPosV + lengthV) || !(tempV <= startPosV - lengthV))
        //{            
        //    transform.position = new Vector3(transform.position.x, startPosV + dist, transform.position.z);
        //}

        //if (tempV >= startPosV + lengthV) startPosV += lengthV;
        //else if (tempV < startPosV - lengthV) startPosV -= lengthV;
    }
}
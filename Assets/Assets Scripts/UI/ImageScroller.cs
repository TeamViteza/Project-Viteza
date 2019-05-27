using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroller : MonoBehaviour {

    Renderer sRenderer;  
    public float scrollSpeed = 0.5f;
	
	void Start () {
        sRenderer = GetComponent<Renderer>();
	}
		
	void Update () {
        Vector2 offset = new Vector2(-Time.time * scrollSpeed, Time.time * scrollSpeed);
        sRenderer.material.mainTextureOffset = offset;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour {	

    private void OnCollisionEnter2D(Collision2D collision)
    {        
            Destroy(transform.gameObject); // The icicle will shatter upon the player touching it, hurting the player in the process.        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour {

    public Transform firePoint;
    public GameObject bulletPrefab;
    public PlayerMovement player;
    // Update is called once per frame

     void Start()
    {
        player = transform.parent.GetComponent<PlayerMovement>();
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Shoot();
        }
        
	}

    void Shoot ()
    {
        //if (player.playerSprite.flipX == true)
        //{
        //    firePoint.
            
        //}

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, firePoint);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{    
    public GameObject bulletPrefab;       

    void Update()
    {       
        if (Input.GetButtonDown("BtnX"))
        {      
            Shoot();
        }
    }

    void Shoot()
    {       
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
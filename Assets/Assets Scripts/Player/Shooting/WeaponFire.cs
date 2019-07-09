using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{    
    public GameObject bulletPrefab;

    Animator playerAnimator;

    void Start()
    {
        playerAnimator = transform.GetComponentInParent<Animator>();       
    }

    void Update()
    {
        if (Input.GetButtonDown("BtnX")) Shoot();
        else if (playerAnimator !=null && playerAnimator.GetBool("firing") == true) UpdateShootAnimation(false);
    }

    void Shoot()
    {       
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        UpdateShootAnimation(true);
    }

    void UpdateShootAnimation(bool firing)
    {
        if (playerAnimator == null) return;
        playerAnimator.SetBool("firing", firing);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;

public class Blaster : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletOriginOffsetY = 0.05f; // I recommend this value currently.

    [EventRef]
    public string footsteps = "event:/Master/SFX/blaster/blaster";

    SpriteRenderer blasterSprite;
    SMovement playerMovement;
    Animator playerAnimator; 
    Vector3 firePosition;
    
    float bSpriteExtentsX, bSpriteExtentsY;

    void Start()
    {
        blasterSprite = transform.GetComponent<SpriteRenderer>();
        playerMovement = transform.GetComponentInParent<SMovement>();
        playerAnimator = transform.GetComponentInParent<Animator>();          

        bSpriteExtentsX = blasterSprite.bounds.extents.x; // We obtain these values so that the projectile appears from an appropriate location in regards to the blaster sprite.
        bSpriteExtentsY = blasterSprite.bounds.extents.y;        
    }

    void Update()
    {
        if (Input.GetButtonDown("BtnX")) Shoot();
        else if (playerAnimator != null && playerAnimator.GetBool("firing") == true) UpdateShootAnimation(false);      
    }

    void Shoot()
    {        
        switch (playerMovement.FacingRight) // The projectile's origin point is determined by which side of the screen Katt is facing.
        {
            case true:
                Instantiate(bulletPrefab, new Vector3(transform.position.x + bSpriteExtentsX, (transform.position.y + bSpriteExtentsY) - bulletOriginOffsetY, 0), transform.rotation);
                break;

            case false:
                Instantiate(bulletPrefab, new Vector3(transform.position.x - bSpriteExtentsX, (transform.position.y + bSpriteExtentsY) - bulletOriginOffsetY, 0), transform.rotation);
                break;
        }

        UpdateShootAnimation(true);
    }

    void UpdateShootAnimation(bool firing)
    {
        if (playerAnimator == null) return;
        playerAnimator.SetBool("firing", firing);
    }    
}
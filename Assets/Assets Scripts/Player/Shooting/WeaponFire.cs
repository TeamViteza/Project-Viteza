using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletOriginOffsetY = 0.05f; // I recommend this value currently.

    SpriteRenderer blasterSprite;
    Animator playerAnimator;
    Vector3 firePosition;

    float bSpriteExtentsX, bSpriteExtentsY;

    void Start()
    {
        blasterSprite = transform.GetComponent<SpriteRenderer>();
        playerAnimator = transform.GetComponentInParent<Animator>();        

        bSpriteExtentsX = blasterSprite.bounds.extents.x;
        bSpriteExtentsY = blasterSprite.bounds.extents.y;        
    }

    void Update()
    {
        if (Input.GetButtonDown("BtnX")) Shoot();
        else if (playerAnimator != null && playerAnimator.GetBool("firing") == true) UpdateShootAnimation(false);
    }

    void Shoot()
    {       
        Instantiate(bulletPrefab, new Vector3(transform.position.x + bSpriteExtentsX, (transform.position.y + bSpriteExtentsY) - bulletOriginOffsetY, 0), transform.rotation);            
        UpdateShootAnimation(true);
    }

    void UpdateShootAnimation(bool firing)
    {
        if (playerAnimator == null) return;
        playerAnimator.SetBool("firing", firing);
    }

    public void ToggleOrientation()
    {
        blasterSprite.flipX = !blasterSprite.flipX;
    }
}
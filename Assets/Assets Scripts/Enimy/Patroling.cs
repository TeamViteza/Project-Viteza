using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroling : MonoBehaviour {
    public float speed;
    public float distance;

    private bool moving = true;

    public Transform onGround;

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D ray2D = Physics2D.Raycast(onGround.position, Vector2.down, 2f);
        if (ray2D.collider == false)
        {
            if (moving == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                moving = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                moving = true;
            }
        }
    }
}

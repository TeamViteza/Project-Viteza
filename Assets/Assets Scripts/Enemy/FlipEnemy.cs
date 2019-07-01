using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlipEnemy : MonoBehaviour {

    public AIPath flip;

    // Update is called once per frame
    void Update () {
        if (flip.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(0.11f, 0.11f, 0.11f);
        }
        else if (flip.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-0.11f, 0.11f, 0.11f);
        }
	}
}

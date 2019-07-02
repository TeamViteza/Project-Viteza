using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    public float speed;
    public Transform[] moveTo;
    private int randomSpot;
    private float wait;
    public float startWait;

    // Use this for initialization
    void Start()
    {
        wait = startWait;

        randomSpot = Random.Range(0, moveTo.Length);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveTo[randomSpot].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveTo[randomSpot].position) < 0.2f)
        {
            if (wait <= 0)
            {
                randomSpot = Random.Range(0, moveTo.Length);
                wait = startWait;
            }
            else
            {
                wait -= Time.deltaTime;
            }
        }
    }
}

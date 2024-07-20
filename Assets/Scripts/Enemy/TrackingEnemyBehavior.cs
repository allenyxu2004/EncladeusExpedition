using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionRadius = Mathf.Infinity;

    Transform player;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < detectionRadius)
        {
            RotateEnemy();
            FollowPlayer();
        }
    }

    void RotateEnemy()
    {
        transform.LookAt(player);
    }

    void FollowPlayer()
    {
        // move toward the player smoothly
        // moveSpeed should be adjustable through the inspector
        transform.position =
            Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}

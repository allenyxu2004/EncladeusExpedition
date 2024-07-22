using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyBehavior : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 5f;
    public float detectionRadius = Mathf.Infinity;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
        }

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < detectionRadius)
        {
            RotateEnemy();
            FollowPlayer();
        }
    }

    void RotateEnemy()
    {
        transform.LookAt(target.transform);
    }

    void FollowPlayer()
    {
        // move toward the player smoothly
        // moveSpeed should be adjustable through the inspector
        transform.position =
            Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }


}

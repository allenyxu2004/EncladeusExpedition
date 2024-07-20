using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float detectionRadius = Mathf.Infinity;
    public AudioClip enemySFX;

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


    private void FixedUpdate()
    {
        if (Vector3.Distance(player.position, transform.position) < detectionRadius)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed, ForceMode.Force);
        }

    }

}

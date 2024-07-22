using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 5f;
    public float detectionRadius = Mathf.Infinity;
    public AudioClip enemySFX;

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


    private void FixedUpdate()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < detectionRadius)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed, ForceMode.Force);
        }

    }

}

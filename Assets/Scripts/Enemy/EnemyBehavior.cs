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


    private void Update()
    {
        transform.LookAt(target.transform);
        float distance = Vector3.Distance(transform.position, target.transform.position);

       Vector3 direction = target.transform.position - transform.position;
       transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

    }

}

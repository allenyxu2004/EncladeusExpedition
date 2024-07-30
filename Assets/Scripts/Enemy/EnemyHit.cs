using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public int atkDamage = 10;
    public float delayDuration = 1;
    float delayCount = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (delayCount > 0)
        {
            delayCount -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && delayCount <= 0)
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(atkDamage);
            delayCount = delayDuration;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && delayCount <= 0)
        {
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(atkDamage);
            delayCount = delayDuration;
        }
    }
}

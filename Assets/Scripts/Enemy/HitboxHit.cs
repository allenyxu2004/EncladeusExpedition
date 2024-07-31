using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxHit : MonoBehaviour
{
    public EnemyHealth enemyHealth;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            enemyHealth.TakeDamage(20);
        }
    }

}

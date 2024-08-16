using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHit : MonoBehaviour
{
    public int waterDamage = 200;
    public AudioSource waterSFX;
    public AudioSource damageTake;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            waterSFX.Play();
            damageTake.mute = true;
            playerHealth.TakeDamage(waterDamage);

            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCenter : MonoBehaviour
{
    public int healEffect = 10;
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
        // not currently working :(
        if (collision.gameObject.CompareTag("Player") && delayCount <= 0)
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.HealPlayer(healEffect);
            delayCount = delayDuration;
        }
    }
}

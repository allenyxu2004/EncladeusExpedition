using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCenter : MonoBehaviour
{
    public float healEffect = 10.0f;

    void Start()
    {

    }

    void Update()
    {
    
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Healing...");
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.HealPlayer(healEffect * Time.deltaTime);
        }
    }   
}

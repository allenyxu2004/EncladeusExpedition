using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("BoostPad: trigger entered");
        if (other.gameObject.CompareTag("Player"))
        {
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            playerHealth.BoostShip();

            Debug.Log("BoostPad: boost attempted");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("BoostPad: trigger exited");
        if (other.gameObject.CompareTag("Player"))
        {
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            playerHealth.SlowShip();

            Debug.Log("BoostPad: slow down attempted");
        }
    }
}

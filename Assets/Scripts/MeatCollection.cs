using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatCollection : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            playerHealth.addMeat();
            Debug.Log("Meat: Meat collected");
            Destroy(gameObject, 0.1f);
        }
    }
}

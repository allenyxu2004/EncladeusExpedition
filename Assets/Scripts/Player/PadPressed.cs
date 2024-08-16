using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadPressed : MonoBehaviour
{
    public static bool padPressed;
    // Start is called before the first frame update
    void Start()
    {
        padPressed = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Started Ship");
        if (other.gameObject.CompareTag("Player"))
        {
            padPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited Ship");
        if (other.gameObject.CompareTag("Player"))
        {
            padPressed = false;
        }
    }
}

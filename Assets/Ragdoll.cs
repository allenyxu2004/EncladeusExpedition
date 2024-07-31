using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{

    Rigidbody[] rigidbodies;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

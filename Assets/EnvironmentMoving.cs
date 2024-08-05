using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMoving : MonoBehaviour
{
    public float environmentSpeed = 1.0f;

    void Start()
    {
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + Time.deltaTime * environmentSpeed, transform.position.y, transform.position.z);
    }
}

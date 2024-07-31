using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime;
    }
}

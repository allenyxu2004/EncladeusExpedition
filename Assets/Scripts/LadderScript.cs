using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{
    // Start is called before the first frame update
    bool inside = false;
    public float speedDown = 3.2f;
    CharacterController controller;
    public Transform player;



    void Start()
    {
        controller = GetComponent<CharacterController>();

        inside = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (inside == true && Input.GetKey("w"))
        {
            player.transform.position += Vector3.up * speedDown * Time.deltaTime; 
        }

        if (inside == true && Input.GetKey("s"))
        {
            player.transform.position += Vector3.down * speedDown * Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            controller.enabled = false;
            inside = true;
            Debug.Log("Entered");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            Debug.Log("Exited");

            controller.enabled = true;
            inside = false;
        }
    }
}

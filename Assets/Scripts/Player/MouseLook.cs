    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook2 : MonoBehaviour
{
    // Start is called before the first frame update
    Transform playerBody;
    public float mouseSensitivity = 200;
    float pitch = 0;
    void Start()
    {
        playerBody = transform.parent.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Universial mouse controls with sens applied
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player body (YAW)
        playerBody.Rotate(Vector3.up * moveX);

        // Need to invert the pitch to match mouse movement
        pitch -= moveY;

        // Clamp so we don't look too far
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Local rotation bc we don't want to move the player in pitch
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}

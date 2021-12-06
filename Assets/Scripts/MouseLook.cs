using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 10f;
    public Transform playerBody;
    private float xRotation = 0f;
    void Start()
    {
        // hide and lock the cursor in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        // vertical mouse movement rotates the camera around the x-axis (i.e. up and down)
        xRotation -= mouseY;
        
        // clamp the rotation so that we don't over-rotate and look behind the player
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        // rotate the camera using local rotation. 
        // Quaternion is used for rotation in Unity. we only want to rotate the X axis in this case.
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // horizontal mouse movement rotates the player body
        playerBody.Rotate(Vector3.up * mouseX);

    }
}

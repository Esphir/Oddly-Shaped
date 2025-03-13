using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookScript : MonoBehaviour
{
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    public Transform playerBody;

    private float xRotation = 0f;
    private bool canLook = true; // Determines if the player can look around

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor in the center of the screen
    }

    void Update()
    {
        if (canLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

            xRotation -= mouseY; // Invert Y-axis movement properly
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void EnableLooking(bool enable)
    {
        canLook = enable;
    }
}
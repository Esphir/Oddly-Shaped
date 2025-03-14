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

    // Raycast variables
    public float raycastDistance = 3f; // How far the ray should detect
    public LayerMask interactableLayer; // Set to the layers you want the ray to detect (e.g., "Desk" objects)

    private GameObject currentLookedObject; // Current object the player is looking at

    // Door-related variables
    public Transform door; // Reference to the door object
    public float doorOpenAngle = 90f; // The angle the door should rotate to when opened
    public float doorSwingSpeed = 2f; // Speed of the door's swinging animation

    private bool isDoorOpen = false; // Keep track if the door is open or not

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor in the center of the screen
    }

    void Update()
    {
        if (canLook)
        {
            LookAround(); // Handle mouse look and raycasting
            CheckForInteractableObject(); // Check if the player is looking at a desk
        }
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        xRotation -= mouseY; // Invert Y-axis movement properly
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void CheckForInteractableObject()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward); // Ray starts from the camera, going forward

        if (Physics.Raycast(ray, out hit, raycastDistance, interactableLayer))
        {
            // If the ray hits an object tagged with "Desk"
            if (hit.collider.CompareTag("Desk"))
            {
                currentLookedObject = hit.collider.gameObject;
                if (Input.GetMouseButton(0)) // Left-click held down
                {
                    OpenDoor(); // If left-click is held, swing open the door
                }
            }
            else
            {
                currentLookedObject = null;
            }
        }
        else
        {
            currentLookedObject = null;
        }
    }

    private void OpenDoor()
    {
        if (door != null && !isDoorOpen)
        {
            // Rotate the door open
            float step = doorSwingSpeed * Time.deltaTime;
            door.rotation = Quaternion.RotateTowards(door.rotation, Quaternion.Euler(0f, doorOpenAngle, 0f), step);

            // If the door has opened enough, we consider it fully opened
            if (Quaternion.Angle(door.rotation, Quaternion.Euler(0f, doorOpenAngle, 0f)) < 1f)
            {
                isDoorOpen = true;
            }
        }
    }

    // Method to toggle if the player can look around
    public void EnableLooking(bool enable)
    {
        canLook = enable;
    }
}

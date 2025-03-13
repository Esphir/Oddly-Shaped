using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform holdPosition;
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    public float holdDistance = 0.7f;
    public float raycastDistance = 3f; // The distance of the raycast to detect items

    private Rigidbody rb;
    private GameObject heldObject;
    private float rotationX = 0f;

    private string equippedLens = "None";
    private Dictionary<string, string> lensWallMapping = new Dictionary<string, string>()
    {
        {"RedLens", "RedWall"},
        {"BlueLens", "BlueWall"},
        {"GreenLens", "GreenWall"}
    };

    // Reference to the UI Image that overlays the color
    public Image lensOverlayImage;

    // Colors for each lens
    private Color redLensColor = new Color(1f, 0f, 0f, 0.5f); // Red with transparency
    private Color blueLensColor = new Color(0f, 0f, 1f, 0.5f); // Blue with transparency
    private Color greenLensColor = new Color(0f, 1f, 0f, 0.5f); // Green with transparency
    private Color noneLensColor = new Color(0f, 0f, 0f, 0f); // Transparent (no lens effect)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        UpdateLensOverlay("None");
    }

    void Update()
    {
        MovePlayer();
        LookAround();
        UpdateHeldObjectPosition();

        // Try picking up an item if the player is looking at one
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                PickUpObject();
            else
                DropObject();
        }

        // Equip lenses
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipLens("RedLens");
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipLens("BlueLens");
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipLens("GreenLens");
        if (Input.GetKeyDown(KeyCode.F)) EquipLens("None");
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    void PickUpObject()
    {
        RaycastHit hit;

        // Cast a ray from the camera's position in the forward direction
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("PickUp")) // Check if the object has the "PickUp" tag
            {
                heldObject = hit.collider.gameObject;
                heldObject.transform.SetParent(null);
                heldObject.transform.localRotation = Quaternion.identity;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;
                UpdateHeldObjectPosition();
                Debug.Log("Picked up: " + heldObject.name);
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.transform.SetParent(null);
            heldObject = null;
        }
    }

    void UpdateHeldObjectPosition()
    {
        if (heldObject != null)
        {
            heldObject.transform.position = Vector3.Lerp(
                heldObject.transform.position,
                playerCamera.position + playerCamera.forward,
                Time.deltaTime * 10f
            );
        }
    }

    void EquipLens(string lensName)
    {
        equippedLens = lensName;
        Debug.Log("Equipped: " + lensName);

        UpdateLensOverlay(lensName);

        // Disable collisions for relevant walls based on the lens
        foreach (var pair in lensWallMapping)
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag(pair.Value);
            bool shouldDisableCollision = pair.Key == equippedLens;

            foreach (GameObject wall in walls)
            {
                Collider wallCollider = wall.GetComponent<Collider>();
                if (wallCollider != null)
                    wallCollider.enabled = !shouldDisableCollision;
                else
                    Debug.LogWarning("Wall " + wall.name + " does not have a collider!");
            }
        }
    }

    void UpdateLensOverlay(string lensName)
    {
        switch (lensName)
        {
            case "RedLens":
                lensOverlayImage.color = redLensColor;
                break;
            case "BlueLens":
                lensOverlayImage.color = blueLensColor;
                break;
            case "GreenLens":
                lensOverlayImage.color = greenLensColor;
                break;
            case "None":
                lensOverlayImage.color = noneLensColor;
                break;
            default:
                lensOverlayImage.color = noneLensColor;
                break;
        }
    }
}

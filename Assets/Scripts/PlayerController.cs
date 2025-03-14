using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float holdDistance = 0.7f;
    public float raycastDistance = 3f; // The distance of the raycast to detect items
    public GameObject CanvasObject;

    private Rigidbody rb;
    public static string EquippedLens { get; private set; } = "None";
    private Dictionary<string, string> lensWallMapping = new Dictionary<string, string>()
    {
        {"RedLens", "RedWall"},
        {"BlueLens", "BlueWall"},
        {"GreenLens", "GreenWall"}
    };


    public Image lensOverlayImage;


    private Color redLensColor = new Color(1f, 0f, 0f, 0.2f); // Red with transparency
    private Color blueLensColor = new Color(0f, 0f, 1f, 0.2f); // Blue with transparency
    private Color greenLensColor = new Color(0f, 1f, 0f, 0.2f); // Green with transparency
    private Color noneLensColor = new Color(0f, 0f, 0f, 0f); // Transparent (no lens effect)

    void Start()
    {
        CanvasObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        UpdateLensOverlay("None");
    }

    void Update()
    {
        MovePlayer();

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

    void EquipLens(string lensName)
    {
        EquippedLens = lensName;
        Debug.Log("Equipped: " + lensName);

        UpdateLensOverlay(lensName);
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

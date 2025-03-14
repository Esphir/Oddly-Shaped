using TMPro; // Import the TextMeshPro namespace
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float holdDistance = 0.7f;
    public float raycastDistance = 3f; // The distance of the raycast to detect items
    public GameObject CanvasObject;

    // Change TextMesh to TextMeshProUGUI
    public TextMeshProUGUI promptText; // Use TextMeshProUGUI instead of TextMesh

    private Rigidbody rb;
    public static string EquippedLens { get; private set; } = "None";
    private Dictionary<string, string> lensWallMapping = new Dictionary<string, string>()
    {
        {"OrangeLens", "RedWall"},
        {"BlueLens", "BlueWall"}
    };

    public Image lensOverlayImage;

    private Color orangeLensColor = new Color(1f, 0.494f, 0.0157f, 0.2f); // Orange (#ff7e04) with transparency
    private Color blueLensColor = new Color(0.043f, 0.145f, 0.976f, 0.2f); // Blue (#0b25f9) with transparency
    private Color noneLensColor = new Color(0f, 0f, 0f, 0f); // Transparent (no lens effect)

    // Keep track of lens abilities as instance variables
    private bool hasOrangeLens = false;
    private bool hasBlueLens = false;

    void Start()
    {
        CanvasObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        UpdateLensOverlay("None");

        // Display prompt text at the start
        ShowPromptText("Why are we here? ... Escape the room");

        // Start a coroutine to hide the prompt after a delay
        StartCoroutine(HidePromptTextAfterDelay(5f)); // Hide after 5 seconds
    }

    void Update()
    {
        MovePlayer();

        // Only equip lens if the player has collected the corresponding crystal
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasOrangeLens) EquipLens("OrangeLens");
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasBlueLens) EquipLens("BlueLens");
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
            case "OrangeLens":
                lensOverlayImage.color = orangeLensColor;
                break;
            case "BlueLens":
                lensOverlayImage.color = blueLensColor;
                break;
            case "None":
                lensOverlayImage.color = noneLensColor;
                break;
            default:
                lensOverlayImage.color = noneLensColor;
                break;
        }
    }

    // Call this method when the player picks up a crystal
    public void UnlockLensAbility(string lensColor)
    {
        if (lensColor == "Orange")
        {
            hasOrangeLens = true;
            Debug.Log("Orange lens ability unlocked!");
        }
        else if (lensColor == "Blue")
        {
            hasBlueLens = true;
            Debug.Log("Blue lens ability unlocked!");
        }
    }

    // Show prompt text on the screen
    private void ShowPromptText(string message)
    {
        if (promptText != null)
        {
            promptText.text = message;
            promptText.gameObject.SetActive(true);  // Ensure the text is visible
        }
    }

    // Hide prompt text after a certain delay
    private IEnumerator HidePromptTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);  // Hide the prompt after the delay
        }
    }
}

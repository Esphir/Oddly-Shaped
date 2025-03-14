using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float throwForce = 500f;
    public float pickUpRange = 5f;
    private float rotationSensitivity = 1f;
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private bool canDrop = true;
    private int LayerNumber;

    MouseLookScript mouseLookScript;

    // Add a public variable to reference the object you want to unhide (the held object)
    public GameObject objectToUnhide;  // This is the object you want to unhide when the flashlight is picked up

    // Add another public variable for the flashlight's parts (to hide them when picked up)
    public GameObject[] flashlightParts;  // The parts of the flashlight on the ground

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("Hold");
        mouseLookScript = player.GetComponent<MouseLookScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    // Check for "PickUp" tag or crystal tags
                    if (hit.transform.CompareTag("PickUp"))
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                    else if (hit.transform.CompareTag("OrangeCrystal"))
                    {
                        CollectCrystal(hit.transform.gameObject, "Orange");
                    }
                    else if (hit.transform.CompareTag("BlueCrystal"))
                    {
                        CollectCrystal(hit.transform.gameObject, "Blue");
                    }
                    else if (hit.transform.CompareTag("Light")) // Check for flashlight tag
                    {
                        PickUpFlashlight(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (canDrop)
                {
                    StopClipping();
                    DropObject();
                }
            }
        }

        if (heldObj != null)
        {
            MoveObject();
            RotateObject();

            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop)
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObj.transform.parent = holdPos.transform;

            // Change layer to "Hold" for the held object
            heldObj.layer = LayerNumber;

            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }

    void PickUpFlashlight(GameObject flashlightObj)
    {
        // Hide the flashlight's parts on the ground (only the flashlight)
        if (flashlightParts != null && flashlightParts.Length > 0)
        {
            foreach (GameObject part in flashlightParts)
            {
                if (part != null) // Ensure it's not null before hiding
                {
                    part.SetActive(false); // Hide the parts on the ground
                }
            }
        }

        // Now, unhide the object in the player's hand (if objectToUnhide is set)
        if (objectToUnhide != null)
        {
            objectToUnhide.SetActive(true); // Unhide the object that is set in the Inspector
        }

        // Optionally pick up the flashlight (doesn't actually pick it up, just hides parts)
        PickUpObject(flashlightObj);
    }

    void CollectCrystal(GameObject crystal, string type)
    {
        // Call PlayerController to unlock the lens ability
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (type == "Orange")
        {
            playerController.UnlockLensAbility("Orange");
        }
        else if (type == "Blue")
        {
            playerController.UnlockLensAbility("Blue");
        }

        // Make crystal disappear
        crystal.SetActive(false);
    }

    void DropObject()
    {
        // Reset layer and physics when dropping the object
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;

        heldObj = null;
    }

    void MoveObject()
    {
        heldObj.transform.position = holdPos.transform.position;
    }

    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            canDrop = false;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;

            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            canDrop = true;
        }
    }

    void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);

        heldObj = null;
    }

    void StopClipping()
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);

        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }
}

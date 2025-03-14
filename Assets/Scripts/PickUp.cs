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

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("Hold");
        mouseLookScript = player.GetComponent<MouseLookScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
            {
                // Debug the raycast hit
                Debug.DrawLine(transform.position, hit.point, Color.red, 2f); // Draw the ray in the scene

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

    void PickUpFlashlight(GameObject flashlightObj)
    {
        // First, call the existing PickUpObject() logic
        PickUpObject(flashlightObj);

        // Now, unhide the object in the player's hand, assuming it was previously hidden
        if (heldObj != null)
        {
            // Check if the held object has been hidden (SetActive(false))
            heldObj.SetActive(true); // Unhide the held object
        }

        // Optionally, you might want to perform other actions related to the flashlight
        // Example: Enable flashlight functionality if required
    }


    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObj.transform.parent = holdPos.transform;
            heldObj.layer = LayerNumber;

            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidReset : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        // Store the initial position and rotation of the object
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Void"))
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        // Reset the object's position and rotation
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // If the object has a Rigidbody, reset its velocity
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            OpenDoor(other.gameObject);
        }
    }

    void OpenDoor(GameObject door)
    {
        // Disable the renderer and collider to make the door "disappear"
        Renderer doorRenderer = door.GetComponent<Renderer>();
        Collider doorCollider = door.GetComponent<Collider>();

        if (doorRenderer != null)
        {
            doorRenderer.enabled = false;
        }

        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        Debug.Log("Door opened!");
    }
}

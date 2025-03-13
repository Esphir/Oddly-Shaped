using UnityEngine;

public class ItemCollisionHandler : MonoBehaviour
{
    private Collider itemCollider;

    void Start()
    {
        itemCollider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        // Get the item's layer name
        string itemLayer = LayerMask.LayerToName(gameObject.layer);
        string wallLayer = LayerMask.LayerToName(other.layer);

        // Check if item and wall layers match
        if (itemLayer == wallLayer)
        {
            // Ignore collision between item and corresponding wall
            Physics.IgnoreCollision(itemCollider, other.GetComponent<Collider>(), true);
            Debug.Log($"Ignoring collision between {gameObject.name} (Layer: {itemLayer}) and {other.name} (Layer: {wallLayer})");
        }
    }
}

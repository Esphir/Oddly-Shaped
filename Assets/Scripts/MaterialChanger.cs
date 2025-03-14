using UnityEngine;

public class LensMaterialChanger : MonoBehaviour
{
    // Assign materials in the Inspector
    public Material redLensMaterial;
    public Material blueLensMaterial;
    public Material greenLensMaterial;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false; // Start as invisible
    }

    void Update()
    {
        UpdateMaterial(PlayerController.EquippedLens);
    }

    void UpdateMaterial(string equippedLens)
    {
        switch (equippedLens)
        {
            case "RedLens":
                rend.material = redLensMaterial;
                rend.enabled = true;
                break;
            case "BlueLens":
                rend.material = blueLensMaterial;
                rend.enabled = true;
                break;
            case "GreenLens":
                rend.material = greenLensMaterial;
                rend.enabled = true;
                break;
            default:
                rend.enabled = false; // Make invisible when no lens is equipped
                break;
        }
    }
}

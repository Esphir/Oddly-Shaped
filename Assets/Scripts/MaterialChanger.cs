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
        rend.enabled = false;
    }

    void Update()
    {
        UpdateMaterial(PlayerController.EquippedLens);
    }

    void UpdateMaterial(string equippedLens)
    {
        switch (equippedLens)
        {
            case "OrangeLens":
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
                rend.enabled = false;
                break;
        }
    }
}

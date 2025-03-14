using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public Material redLensMaterial;
    public Material blueLensMaterial;
    public Material greenLensMaterial;
    public Material defaultMaterial;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateMaterial(PlayerController.EquippedLens);
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
                break;
            case "BlueLens":
                rend.material = blueLensMaterial;
                break;
            case "GreenLens":
                rend.material = greenLensMaterial;
                break;
            default:
                rend.material = defaultMaterial;
                break;
        }
    }
}

using UnityEngine;

public class LensGlass : MonoBehaviour
{
    public string requiredLens; // Set this in the Inspector (RedLens, BlueLens, GreenLens)
    public bool shouldAppearWithLens = false; // If true, the object will appear with the lens; if false, it will disappear

    private Renderer rend;
    //private Collider col;

    void Start()
    {
        rend = GetComponent<Renderer>();
        //col = GetComponent<Collider>();
    }

    void Update()
    {
        if (shouldAppearWithLens)
        {
            // Object should appear when the correct lens is equipped
            if (PlayerController.EquippedLens == requiredLens)
            {
                rend.enabled = true;
                //col.enabled = true;
            }
            else
            {
                rend.enabled = false;
                //col.enabled = false;
            }
        }
        else
        {
            // Object should disappear when the correct lens is equipped
            if (PlayerController.EquippedLens == requiredLens)
            {
                rend.enabled = false;
                //col.enabled = false;
            }
            else
            {
                rend.enabled = true;
                //col.enabled = true;
            }
        }
    }
}

using UnityEngine;

public class LensGlass : MonoBehaviour
{
    public string requiredLens; 
    public bool shouldAppearWithLens = false; 

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

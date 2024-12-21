using UnityEngine;

public class OutlineHover : MonoBehaviour
{
    private Material outlineMaterial;

    void Start()
    {
        
        // Access the second material (index 1) in the MeshRenderer's materials array
        outlineMaterial = GetComponent<Renderer>().materials[1];
        outlineMaterial.SetFloat("OutlineVisible", 1);
    }

    void OnMouseEnter()
    {
        outlineMaterial.SetFloat("OutlineVisible", 1); // Show the outline
    }

    void OnMouseExit()
    {
        outlineMaterial.SetFloat("OutlineVisible", 0); // Hide the outline
    }
}

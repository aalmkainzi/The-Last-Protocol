using UnityEngine;

public class SetMaterial : MonoBehaviour
{
    public Renderer[] legComponents = null;
    [Tooltip("The apply button will automatically apply the selected 'Leg' material to all the components that make up the legs, and the 'body' material to the body.")]
    public Material legMaterial = null;

    public Material bodyMaterial = null;
    public SkinnedMeshRenderer bodyMesh;

    public void ApplyMaterial()
    {
        legComponents = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in legComponents)
        {
            renderer.material = legMaterial;
        }
        Debug.Log("Applied " + legMaterial.name + " to " + legComponents.Length + " meshes");

        bodyMesh.material = bodyMaterial;
    }
}



using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetMaterial))]
public class ApplyMaterialToGroup : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SetMaterial setMaterial = (SetMaterial)target;

        if (GUILayout.Button("Apply Material"))
        {
            Debug.Log("Material Set");
            setMaterial.ApplyMaterial();
        }
    }
}


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TreePlacer))]
public class TreeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Get the target ObjectModifier component
        TreePlacer objModifier = (TreePlacer)target;

        // Show a button in the Inspector to apply changes
        if (GUILayout.Button("Place Trees"))
        {
            objModifier.PlaceTrees(120, 0.5f, 1.5f);

            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }
    }

}
#endif
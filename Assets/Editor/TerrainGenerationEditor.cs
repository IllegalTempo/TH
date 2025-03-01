using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGeneration))]
public class TerrainGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get reference to the TerrainGeneration script
        TerrainGeneration terrainGen = (TerrainGeneration)target;

        // Draw the default inspector properties
        DrawDefaultInspector();

        // Add space before the button
        EditorGUILayout.Space(10);

        // Create a button that calls the Generate function
        
    }
}

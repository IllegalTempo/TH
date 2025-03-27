//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//using UnityEditor;

//[CustomEditor(typeof(GridSystem))]
//public class GridSystemEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        // Get reference to the TerrainGeneration script
//        GridSystem gd = (GridSystem)target;

//        // Draw the default inspector properties
//        DrawDefaultInspector();

//        // Add space before the button
//        EditorGUILayout.Space(10);
//        if(GUILayout.Button("Delete child",GUILayout.Height(20)))
//        {
//            int childCount = gd.transform.childCount;
//            for (int i = childCount - 1; i >= 0; i--)
//            {
//                Transform child = gd.transform.GetChild(i);

//                // If in editor mode, use DestroyImmediate
//                if (Application.isPlaying)
//                {
//                    Destroy(child.gameObject);
//                }
//                else
//                {
//                    DestroyImmediate(child.gameObject);
//                }
//            }
//        }
//        // Create a button that calls the Generate function
//        if (GUILayout.Button("Generate Terrain", GUILayout.Height(30)))
//        {

//            int childCount = gd.transform.childCount;
//            for (int i = childCount - 1; i >= 0; i--)
//            {
//                Transform child = gd.transform.GetChild(i);

//                // If in editor mode, use DestroyImmediate
//                if (Application.isPlaying)
//                {
//                    Destroy(child.gameObject);
//                }
//                else
//                {
//                    DestroyImmediate(child.gameObject);
//                }
//            }

//            // Check if we're in play mode
//            if (Application.isPlaying)
//            {
//                gd.StartGridSystem(true);
//            }
//            else
//            {
//                // If in edit mode, register the action for undo
//                Undo.RecordObject(gd, "Generate Terrain");
//                gd.StartCoroutine(gd.genchunks(false));
//                EditorUtility.SetDirty(gd);
//            }
//        }
//        if (GUILayout.Button("Generate Terrain + Tree + Enemies", GUILayout.Height(30)))
//        {
            


//            // Check if we're in play mode
//            if (Application.isPlaying)
//            {
//                gd.StartCoroutine(gd.genchunks(true));
//            }
//            else
//            {
//                // If in edit mode, register the action for undo
//                Undo.RecordObject(gd, "Generate Terrain");
//                gd.StartCoroutine(gd.genchunks( true));
//                EditorUtility.SetDirty(gd);
//            }
//        }
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GlobalVariableController))]

public class GlobalVariableControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GlobalVariableController controller = target as GlobalVariableController;
        EditorGUILayout.LabelField("Light Intensity");
        GUILayout.Label("");

        if (GUILayout.Button("Confirm"))
        {
            controller.SetWind(controller.WindVector);
            controller.SetLightIntensity(controller.LightIntensity);
            controller.SetGlobalColor(controller.ShadeColor);
            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }
    }
}

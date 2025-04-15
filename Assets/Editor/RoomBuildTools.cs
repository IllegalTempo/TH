using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RoomBuildTools : EditorWindow
{

    private GameObject BaseRoom;
    [MenuItem("Window/RoomBuildTools")]
    private void OnEnable()
    {
        BaseRoom = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Rooms/RoomTools/BaseRoom.prefab");   
    }
    public static void ShowWindow()
    {
        GetWindow<RoomBuildTools>("Room Build Tools");
    }
    private void OnGUI()
    {
        GUILayout.Label("Tools for room building");

        if(GUILayout.Button("Instantiate Base Room"))
        {
            CreateInitialRoom();
        }
        
    }
    private void CreateInitialRoom()
    {
        GameObject room = PrefabUtility.InstantiatePrefab(BaseRoom) as GameObject;
        room.transform.position = Vector3.zero;
        Undo.RegisterCreatedObjectUndo(room, "Create Initial Room");
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Selection.activeGameObject = room;

    }
}

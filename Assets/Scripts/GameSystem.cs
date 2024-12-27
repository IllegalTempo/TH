using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += GameInformation.OnExit;
#endif

        Save.LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnApplicationQuit()
    {
        GameInformation.currentsave.Saving();
    }
}

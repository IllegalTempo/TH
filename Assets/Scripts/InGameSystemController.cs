using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class InGameSystemController : MonoBehaviour
{
    public GameObject NPCPrefab;
    public GameObject[] Destinations;
    private void Awake()
    {
        GameInformation.system = this;
    }
    public void InstantiateHeadlessNPC(Vector3 pos)
    {
        GameObject prefab = Instantiate(NPCPrefab,pos,Quaternion.identity);
        bool sex = Random.Range(0, 2) == 0;
        AiMain ai = prefab.GetComponent<AiMain>();
        float H = Random.value * 0.6f;
        ai.Initialize(sex, Color.HSVToRGB(H, 0.5f, 0.9f), Color.HSVToRGB((H+0.1f)%1, 0.3f, 0.9f), Color.HSVToRGB(H, 0.4f, 1f),Random.value * 7 + 3);
        

    }
    public Vector3 GetRandomDestination()
    {
        return Destinations[(int)(UnityEngine.Random.value * Destinations.Length)].transform.position;
    }
    private void Start()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += GameInformation.OnExit;
#endif

        Save.LoadData();
        for (int i = 0; i < 10;i++)
        {
            
            
            InstantiateHeadlessNPC(GetRandomDestination());

        }
    }

    private void OnApplicationQuit()
    {
        GameInformation.currentsave.Saving();
    }
}

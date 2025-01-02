using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameSystem instance;
    private GameObject PlayerObject;
    public static int saveindex;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
        Cursor.lockState = CursorLockMode.None;
        #if UNITY_EDITOR
        //EditorApplication.playModeStateChanged += GameInformation.OnExit;
        #endif
    }
    public void LoadSceneAction(string scene,bool init)
    {
        StartCoroutine(LoadScene(scene, init));
    }
    private IEnumerator LoadScene(string scene,bool init)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        GameObject spawnpoint = GameObject.Find("SpawnPoint");
        GameInformation.instance.LocalPlayer.SetActive(!scene.Contains("CUTSCENE"));
        if(!init)
        {
            GameInformation.instance.LocalPlayer.transform.position = spawnpoint.transform.position;

        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void SpawnPlayer(Vector3 position)
    {
        PlayerObject = Resources.Load<GameObject>("Assets/Character/Reimu/prefab/Reimu");

        Instantiate(PlayerObject,position,Quaternion.identity);
    }
    private void OnApplicationQuit()
    {
        if (GameInformation.instance.currentsave == null) return;
        GameInformation.instance.currentsave.Saving();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameSystem instance;
    private GameObject PlayerObject;
    public NPCinform npcs = new NPCinform();
    public static int saveindex;
    void Awake()
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
        GetSceneAction = new Dictionary<int, SceneAction>
        {
            {0,HakureiShrine.Marisa_Ask },
            {1,HakureiShrine.Marisa_Ask },
            {2,HakureiShrine.Marisa_Ask_2 },
            {3,HakureiShrine.Marisa_Ask },


        };
    }
    public void LoadSceneAction(string scene,bool init)
    {
        StartCoroutine(LoadScene(scene, init));
    }
    TMP_Text loadingtext;
    private IEnumerator LoadScene(string scene,bool init)
    {
        loadingtext = GameInformation.instance.ui.progressloading;
        if (scene == "StartScreen") { scene = "CUTSCENE_1"; }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;
        float startTime = Time.time;
        GameInformation.instance.ui.LoadingScreenGameObject.SetActive(true);
        while (!asyncLoad.isDone)
        {

            loadingtext.text = ((asyncLoad.progress/0.9f) * 100f) + "%";
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        GameInformation.instance.ui.LoadingScreenGameObject.SetActive(false);

        GameObject spawnpoint = GameObject.Find("SpawnPoint");
        GameInformation.instance.LocalPlayer.SetActive(!scene.Contains("CUTSCENE"));
        if(scene == "InBattle")
        {
            GameInformation.instance.LocalPlayer.GetComponent<PlayerMain>().OnEnterBattle(spawnpoint.transform.position);

        }
        GameInformation.instance.LocalPlayer.transform.position = spawnpoint.transform.position;
        GameInformation.instance.ui.NewPlaceIntro(scene,true);

    }
    public delegate void SceneAction(int missionID);

    public Dictionary<int, SceneAction> GetSceneAction;
    
    public void NextSceneAction()
    {
        int nextaction = Array.IndexOf(GameInformation.instance.currentsave.ActionCheckList, false);
        if (nextaction != -1 && nextaction < GameInformation.instance.currentsave.ActionCheckList.Length)
        {
            GetSceneAction[nextaction](nextaction);

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

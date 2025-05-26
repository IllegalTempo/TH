using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameSystem instance;
    public bool DialogueActive;
[SerializeField]
    private GameObject PlayerObject;
    [SerializeField]
    private GameObject[] Effects;
    [SerializeField]
    private AudioClip[] Sounds;
    [SerializeField]
    public List<Note> CurrentlyOwnedNotes = new List<Note>();
    public NPCinform npcs = new NPCinform();
    public static int saveindex;
    private List<GameObject> allplayerobject = new List<GameObject>();
    public void PlayEffect(int id, Vector3 pos)
    {
        Instantiate(Effects[id], pos, Quaternion.identity);
    }
    public void PlaySound(int id, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(Sounds[id], pos, 1);
    }
    
    public void SpawnPowerDrops(Vector3 CenterPosition)
    {
        GameObject powerinstance = GameInformation.instance.powerDropInstance;

        Vector3 droppos = CenterPosition + Random.insideUnitSphere * 2;
        droppos.y = CenterPosition.y + 2f;
        Instantiate(powerinstance, droppos, Quaternion.identity);
    }
    public void SpawnPointDrops(Vector3 CenterPosition)
    {
        GameObject powerinstance = GameInformation.instance.pointDropInstance;

        Vector3 droppos = CenterPosition + Random.insideUnitSphere * 5;
        droppos.y = CenterPosition.y + 2f;
        Instantiate(powerinstance, droppos, Quaternion.identity);
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
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
            {0,HakureiShrine.Marisa_Ask_0 },
            {1,HakureiShrine.Marisa_Ask_1 },
            {2,HakureiShrine.Marisa_Ask_2 },


        };
    }
    public void LoadSceneAction(string scene, bool init)
    {
        StartCoroutine(LoadScene(scene, init));
    }
    TMP_Text loadingtext;
    private void EnterBattle()
    {
        GameInformation info = GameInformation.instance;
        GameUIManager.instance.EnterBattleState();
        if (info.MainNetwork.IsServer)
        {
            if (info.MainNetwork.server.GetPlayerCount() == 1)
            {
                info.gd.EveryoneReady();
            }
        }
    }
    public void AddRoomNote(Note n)
    {
        GameUIManager.instance.AddRoomNote(n);
        CurrentlyOwnedNotes.Add(n);

    }
    public void AddRandomRoomNote()
    {
        AddRoomNote(GameInformation.instance.AllNotes[UnityEngine.Random.Range(0,GameInformation.instance.AllNotes.Length)]);
    }
    public void SelectNote(Note n)
    {
        if(CurrentlyOwnedNotes.Contains(n))
        {
            CurrentlyOwnedNotes.Remove(n);

        } else
        {
            Debug.LogError($"Select Note ({n.Name}) not in list");
        }
    }
    public void SpawnItem(Vector3 pos, Item item)
    {
    }
    private IEnumerator LoadScene(string scene, bool init)
    {
        loadingtext = GameUIManager.instance.progressloading;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;
        GameUIManager.instance.StartLoading($"Loading Scene: {scene}, Is Init?: {init}");
        while (!asyncLoad.isDone)
        {

            loadingtext.text = ((asyncLoad.progress / 0.9f) * 100f) + "%";
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        GameUIManager.instance.StopLoading();

        GameObject spawnpoint = GameObject.Find("SpawnPoint");
        if (scene == "InBattle")
        {
            EnterBattle();
        }
        Cursor.lockState = CursorLockMode.Locked;
        if (GameInformation.instance.MainNetwork.Connected)
        {
            GameInformation.instance.LocalPlayer.GetComponent<PlayerMain>().SwitchScene(scene, spawnpoint.transform.position);

        }
        GameUIManager.instance.NewPlaceIntro(scene, true);

    }
    public delegate void SceneAction(int missionID);

    public Dictionary<int, SceneAction> GetSceneAction;

    public void NextSceneAction()
    {
        int nextaction = Array.IndexOf(GameInformation.instance.currentsave.ActionCheckList, false);
        if (nextaction != -1 && GetSceneAction.ContainsKey(nextaction))
        {
            GetSceneAction[nextaction](nextaction);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DialogueActive && Input.GetKeyDown(KeyMap.Interact2))
        {
            GameUIManager.instance.DialogueContinue();
        }
    }
    public void RemoveAllPlayerObject()
    {
        foreach (GameObject g in allplayerobject)
        {
            Destroy(g);
        }
    }
    public PlayerMain SpawnPlayer(bool isLocal, int networkid, ulong steamid)
    {
        Debug.Log("Spawning Player");
        PlayerMain p = Instantiate(PlayerObject, Vector3.zero, Quaternion.identity).GetComponent<PlayerMain>();
        p.NetworkID = networkid;
        p.PlayerID = steamid;
        allplayerobject.Add(p.gameObject);
        if (isLocal)
        {
            p.Localisation();
        }
        else
        {
            p.DeLocalisation();

        }
        return p;
    }
    private void OnApplicationQuit()
    {
        if (GameInformation.instance.currentsave == null) return;
        GameInformation.instance.currentsave.Saving();
    }
}

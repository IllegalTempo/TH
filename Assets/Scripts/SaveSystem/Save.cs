using Newtonsoft.Json;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Save
{
    //Save Data
    public PlayerInventory Inventory;
    [JsonIgnore]
    public Vector3 LastLocation;

    public float[] LastLocation_Save = new float[3]; 
    public string LastSceneName;
    public string savename;
    public LastRunSave runsave;
    public bool[] ActionCheckList;
    public PrayerMission[] ActiveMissions = new PrayerMission[9];
    public PrayerMission CurrentMission;
    //[PlaceID] [priority]
    public int seed;

    public int[] FindMissionByID(int Type,int id)
    {
        List<int> result = new List<int>();
        
        for(int i = 0; i < ActiveMissions.Length;i++)
        {
            if (ActiveMissions[i].missionType == Type && ActiveMissions[i].id == id)
            {
                result.Add(i);
            }
        }
        return result.ToArray();
    }





    public Save(string savename)
    {
        //new Player Save
        seed = (int)Time.time;
        Inventory = new PlayerInventory();
        LastLocation = new Vector3(495, 200, -70);
        LastSceneName = "CUTSCENE_1";
        ActionCheckList = new bool[100];
        this.savename = savename;
    }
    public void SaveData()
    {
        LastLocation_Save = new float[3]
        {
            LastLocation.x, LastLocation.y, LastLocation.z
        };
        string json = JsonConvert.SerializeObject(this, Formatting.None);
        ;
        Debug.Log("Saving Data: " + json);
        using (StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + $"SaveData/{savename}.json"))
        {
            writer.Write(json);
        }
    }
    /*
    public void SetSave(PlayerInventory inv, Vector3 LastLocation)
    {
        Inventory = inv;
        this.LastLocation = LastLocation;
    }
    */
    public void UtilizeSave()
    {
        GameInformation.instance.MainNetwork.CreateGameLobby();
        GameSystem.instance.LoadSceneAction(LastSceneName,true);

        GameInformation.instance.inventory = Inventory;
        GameInformation.instance.ui.StartPlayerControl();
        GameObject g = GameSystem.instance.SpawnPlayer(true, 0, SteamClient.SteamId).gameObject;

    }
    public static string CreateNewSave(string savename)
    {
            Save save = new Save(savename);
            save.SaveData();
            return Application.dataPath + Path.AltDirectorySeparatorChar + $"SaveData/{savename}.json";
        
    }
    public static void LoadData(string savename)
    {
        if(!File.Exists(Application.dataPath + Path.AltDirectorySeparatorChar + $"SaveData/{savename}.json"))
        {
            Debug.Log("File Not Exist");
        } else
        {
            string json = string.Empty;

            using (StreamReader reader = new StreamReader(Application.dataPath + Path.AltDirectorySeparatorChar + $"SaveData/{savename}.json"))
            {
                json = reader.ReadToEnd();
            }
            Debug.Log("Reading File: " + json);
            Save save = new Save(savename);
            //JsonUtility.FromJsonOverwrite(json,save);
            JsonConvert.PopulateObject(json,save);
            GameInformation.instance.currentsave = save;
            save.LastLocation = new Vector3(save.LastLocation_Save[0], save.LastLocation_Save[1], save.LastLocation_Save[2]);
        }
        GameInformation.instance.currentsave.UtilizeSave();

    }
    public static void LoadDataPath(string path)
    {
        LoadData(Path.GetFileNameWithoutExtension(path));

    }
    public static Save GetSave(string pathname)
    {
        if (!File.Exists(pathname))
        {
            Debug.Log("File Not Exist");
            return null;
        }
        else
        {
            string json = string.Empty;

            using (StreamReader reader = new StreamReader(pathname))
            {
                json = reader.ReadToEnd();
            }
            Debug.Log("Reading File: " + json);

            return JsonUtility.FromJson<Save>(json);

        }

    }
    public static void InitializeSave(string savename)
    {
        Save save = new Save(savename);
        GameInformation.instance.currentsave = save;
        GameInformation.instance.currentsave.Saving();
    }
    public void Saving()
    {
        LastLocation = GameInformation.instance.LocalPlayer.transform.position;
        
        LastSceneName = SceneManager.GetActiveScene().name;
        if (LastSceneName == "InBattle") { LastSceneName = "InGame"; }
        SaveData();
    }
    public static void OnExit(PlayModeStateChange change)
    {
        if(change == PlayModeStateChange.ExitingPlayMode && GameInformation.instance.currentsave != null)
        {

            GameInformation.instance.currentsave.Saving();
        }
    }
}

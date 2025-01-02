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
    public Vector3 LastLocation;
    public string LastSceneName;
    public string savename;
    public bool[,] ActionCheckList = 
    {
        { false},
    };
    //[PlaceID] [priority]
        








    public int GetBestPriorityAction(int placeid)
    {
        return Array.IndexOf(ActionCheckList[placeid], false);
    }

    public Save(string savename)
    {
        //new Player Save
        Inventory = new PlayerInventory();
        LastLocation = new Vector3(495, 200, -70);
        LastSceneName = "CUTSCENE_1";
        this.savename = savename;
    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(this);

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
        
        GameSystem.instance.LoadSceneAction(LastSceneName,true);
        GameInformation.instance.LocalPlayer.transform.position = LastLocation;
        GameInformation.instance.LocalPlayer.GetComponent<PlayerMain>().inventory = Inventory;
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

            Save save = JsonUtility.FromJson<Save>(json);
            GameInformation.instance.currentsave = save;
        }
        GameInformation.instance.currentsave.UtilizeSave();

    }
    public static void LoadDataPath(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("File Not Exist");
        }
        else
        {
            string json = string.Empty;

            using (StreamReader reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
            }
            Debug.Log("Reading File: " + json);

            Save save = JsonUtility.FromJson<Save>(json);
            GameInformation.instance.currentsave = save;
            save.UtilizeSave();

        }

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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Save
{
    public PlayerInventory Inventory;
    public Vector3 LastLocation;
    public int CutScenes = -1;
    public Save()
    {
        //new Player Save
        Inventory = new PlayerInventory();
        LastLocation = new Vector3(495, 200, -70);
    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(this);

        Debug.Log("Saving Data: " + json);
        using (StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData/PlayerSaveData.json"))
        {
            writer.Write(json);
        }
    }
    public void SetSave(PlayerInventory inv,Vector3 LastLocation)
    {
        Inventory = inv;
        this.LastLocation = LastLocation;
    }
    public void UtilizeSave()
    {
        GameInformation.LocalPlayer.transform.position = LastLocation;
        GameInformation.LocalPlayer.GetComponent<PlayerMain>().inventory = Inventory;
    }
    public static void LoadData()
    {
        if(!File.Exists(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData/PlayerSaveData.json"))
        {
            Debug.Log("File Not Exist, Creating One Instead");
            InitializeSave();
        } else
        {
            string json = string.Empty;

            using (StreamReader reader = new StreamReader(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData/PlayerSaveData.json"))
            {
                json = reader.ReadToEnd();
            }
            Debug.Log("Reading File: " + json);

            Save save = JsonUtility.FromJson<Save>(json);
            GameInformation.currentsave = save;
        }
        GameInformation.currentsave.UtilizeSave();

    }
    public static void InitializeSave()
    {
        Save save = new Save();
        GameInformation.currentsave = save;
        GameInformation.currentsave.Saving();
    }
    public void Saving()
    {
        LastLocation = GameInformation.LocalPlayer.transform.position;
        SaveData();
    }
    public static void OnExit(PlayModeStateChange change)
    {
        if(change == PlayModeStateChange.ExitingPlayMode)
        {

            GameInformation.currentsave.Saving();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class StartScreenScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown SaveDisplays;

    [SerializeField]
    private string selectedsave;
    [SerializeField]
    private Animator SceneObjectAnimator;
    [SerializeField]
    private GameObject Clickables;
    public string[] GetAllSavePath()
    {
        return Directory.GetFiles(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData","*.json").OrderByDescending(d => new FileInfo(d).CreationTime).ToArray();
    }
    private void Start()
    {
        selectedsave = PlayerPrefs.GetString("LastSave");
        DisplayAllSaves();
        

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneObjectAnimator.SetTrigger("ButtonPressed");
            Clickables.SetActive(false);

        }
    }
    public void DisplayAllSaves()
    {
        int selectedindex = 0;
        int i = 0;
        foreach (string path in GetAllSavePath())
        {
            DateTime lastWriteTime = File.GetLastWriteTime(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            if(selectedsave != null)
            {
                if(selectedsave == path)
                {
                    selectedindex = i;
                }
            }
            i++;
            SaveDisplays.options.Add(new TMP_Dropdown.OptionData($"{fileName} - {lastWriteTime.ToString()}"));

        }
        SaveDisplays.value = selectedindex;
        SaveDisplays.RefreshShownValue();
        
    }
    public void OnDropDownValueChanged(int newPosition)
    {
        selectedsave = GetAllSavePath()[newPosition];
    }
    public void OnClickNewSave()
    {
        selectedsave = Save.CreateNewSave($"Save{DateTime.UtcNow.Ticks}");
        SaveDisplays.ClearOptions();
        SaveDisplays.value = 0;
         
        DisplayAllSaves();

    }
    public void OnClickStartGame()
    {
        if (GetAllSavePath().Length == 0)
        {
            selectedsave = Save.CreateNewSave($"Save{DateTime.UtcNow.Ticks}");
        }
        if (selectedsave == null) { selectedsave = GetAllSavePath()[0]; }
        selectedsave = GetAllSavePath()[0];
        PlayerPrefs.SetString("LastSave",selectedsave);
        Debug.Log($"Starting with {selectedsave}");
        Save.LoadDataPath(selectedsave);
        Debug.Log($"Loading Scene: {GameInformation.instance.currentsave.LastSceneName}");
    }
}

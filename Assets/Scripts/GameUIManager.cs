using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text MissionTitle;
    [SerializeField]
    private TMP_Text MissionDescription;
    public void SetMission(string title,string description)
    {
        MissionTitle.text = title;
        MissionDescription.text = description;  
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class mission : MonoBehaviour
{
    public TMP_Text text;
    public PrayerMission referencedMission;
    public Outline outline;
    public void NewMission(PrayerMission mission,int placeindex)
    {
        gameObject.SetActive(true);
        referencedMission = mission;
        mission.placeindex = placeindex;
        
    }

    
    private void Update()
    {
        if(outline.interacted)
        {
            GameInformation.instance.missionsystem.AcceptedMission(referencedMission);
        }
    }
    public void MissionCompleted()
    {
        gameObject.SetActive(false);
    }
}

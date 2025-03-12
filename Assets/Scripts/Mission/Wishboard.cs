using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wishboard : MonoBehaviour
{

    public mission[] PrayerObjects;
    private void Start()
    {
        for (int i = 0; i < PrayerObjects.Length; i++)
        {
            PrayerObjects[i].gameObject.SetActive(false);
        }
        GameInformation.instance.missionsystem = this;
    }
    public void NewPrayer(PrayerMission m)
    {
        int placeindex = Array.IndexOf(PrayerObjects, null);
        PrayerObjects[placeindex].NewMission(m,placeindex);
    }
    public void AcceptedMission(PrayerMission m)
    {
        GameInformation.instance.currentsave.CurrentMission = m;
        //GameUIManager.instance.SetMission(m.name,m.description);
    }
   public void RemovePrayer(PrayerMission m)
    {
        int placeindex = m.placeindex;
        PrayerObjects[placeindex].MissionCompleted();
    }
}

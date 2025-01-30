using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayerMission : MonoBehaviour
{
    //when the int is set to:
    // null -> Not this type of mission
    // 9999 -> Completed
    // any number ->
    public enum MissionType
    {
        Beat = 0,
        Collect = 1,
        Save = 2,
    }
    public int placeindex;
    public int id;
    public int count;
    public int countmax;
    public string description;
    public void IncreaseCount(int increaseamount)
    {
        count += increaseamount;
        if(count >= countmax)
        {
            completed();
        }
    }
    

    public int missionType;
    public PrayerMission(int id,int missiontype)
    {
        this.id = id;
        this.missionType = missiontype;
            switch (missionType)
        {
            case (int)MissionType.Beat:
                description =
$"Beat {NPCinform.GetNpcTranslatedName(id)} {count}/{countmax}";
                break;

            case (int)MissionType.Collect:
                Item item = ItemInform.items[id];
                description =
$"Collect {item.name} {count}/{countmax}";
                break;
            case (int)MissionType.Save:
                description =
$"Find and Rescue {NPCinform.GetNpcTranslatedName(id)} {count}/{countmax}";
                break;

        }
    }
    public void completed()
    {

    }
}

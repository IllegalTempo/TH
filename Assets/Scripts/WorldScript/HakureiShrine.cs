using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HakureiShrine : world
{
    //References
    public NPC Marisa;
    public const int MissionLength = 2;
    public PathCreator Path_LeadToMissionBoard;
    private void OnEnable()
    {
        WorldID = 0;
        GameInformation.instance.CurrentWorld = this;
        GameSystem.instance.NextSceneAction();
    }
    //On Load

    public static void Marisa_Ask(int missionid)
    {
        HakureiShrine thisworld = ((HakureiShrine)GameInformation.instance.CurrentWorld);
        GameInformation.instance.currentsave.ActionCheckList[missionid] = true;
        thisworld.Marisa.NewTranslatedSpeechBubble("MARISA_ASK_" + missionid, 5, true, 0);
    }
    public static void Marisa_Ask_2(int missionid)
    {
        HakureiShrine thisworld = ((HakureiShrine)GameInformation.instance.CurrentWorld);
        GameInformation.instance.currentsave.ActionCheckList[missionid] = true;
        thisworld.Marisa.NewTranslatedSpeechBubble("MARISA_ASK_2" , 5, true, 0);
        thisworld.Marisa.MoveTo(thisworld.Path_LeadToMissionBoard);

    }
}

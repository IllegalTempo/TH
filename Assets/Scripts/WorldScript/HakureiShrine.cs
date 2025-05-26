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

    public static void Marisa_Ask_0(int missionid)
    {
        GameInformation.instance.currentsave.ActionCheckList[missionid] = true;
        GameUIManager.instance.TranslatedDialogue("MARISA_ASK_0","marisa_smart");
    }
    public static void Marisa_Ask_1(int missionid)
    {
        GameInformation.instance.currentsave.ActionCheckList[missionid] = true;
        GameUIManager.instance.TranslatedDialogue("MARISA_ASK_1", "marisa_smart");
    }
    public static void Marisa_Ask_2(int missionid)
    {
        GameInformation.instance.currentsave.ActionCheckList[missionid] = true;
        GameUIManager.instance.TranslatedDialogue("MARISA_ASK_2", "marisa_interested");
    }
}

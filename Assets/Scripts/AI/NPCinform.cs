using System.Collections.Generic;
using System.Data;
using UnityEngine.Localization.Settings;

public class NPCinform
{
    public enum NPCID
    {
        Marisa = 0,
        Rumia = 1,
    }
    public static Dictionary<int,string> GetNPCName = new Dictionary<int, string>()
    {
        {0,"marisa"},
        { 1,"rumia"}
    };
    public static string GetNpcTranslatedName(int id)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString("NPC", GetNPCName[id]);
    }
}


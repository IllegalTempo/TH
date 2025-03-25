using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Item
{
    public string name;
    public int id;
    public int type;
    public string iconpath;
    public int rarity;
    public string description;
    public string instancepath;
    public Item(string name, int id, int type, string iconpath,int rarity)
    {
        this.name = ItemInform.RarityColor[rarity] + LocalizationSettings.StringDatabase.GetLocalizedString("ItemName", name);
        this.description = ItemInform.RarityColor[rarity] + LocalizationSettings.StringDatabase.GetLocalizedString("ItemName", name + "_Description");
        this.instancepath = 
        this.id = id;
        this.type = type;
        this.iconpath = iconpath;
        this.rarity = rarity;
    }
}

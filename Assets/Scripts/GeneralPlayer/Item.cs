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
    public Item(string name, int id, int type, string iconpath,int rarity)
    {
        this.name = ItemInform.RarityColor[rarity] + LocalizationSettings.StringDatabase.GetLocalizedString("Item", name);
       
        this.id = id;
        this.type = type;
        this.iconpath = iconpath;
        this.rarity = rarity;
    }
}

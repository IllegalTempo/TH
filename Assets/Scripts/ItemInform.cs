using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ItemInform
{
    public static string[] RarityColor = new string[]
    {
        "<color=#e8e8e8>",
        "<color=#67f075>",
        "<color=#67b4f0>",
        "<color=#be67f0>",
        "<color=#f067ab>",
        "<color=#f06767>",
        "<color=#f0db67>",
    };
    public enum ItemType
    {
        Weapon = 0,
        Consuming = 1,
        Material = 2,
        Skill = 3,

    }
    public static Item[] items =
    {
        new("Hakurei_Souvenir",0,(int)ItemType.Material,"Items/Icon/000_HakureiUnique",6),
    };
}

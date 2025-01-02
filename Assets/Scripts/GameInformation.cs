using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class GameInformation
{
    public static GameInformation instance;
    public SteamManager MainNetwork;

    //Keys
    public GameObject LocalPlayer;
    public Save currentsave;
    public InGameSystemController system;
    public LayerMask OnlyBuildings = LayerMask.GetMask("Buildings");
    public const int BuildingLayer = 8;
    public Dictionary<int, string> WeaponPrefabPath = new Dictionary<int, string>()
    {
        { (int)Weapon.HAKUREI_FLUTE,"weapon/flute/HAKUREI_FLUTE/flute"},
    };
    public Dictionary<int, string> WeaponPlayerAnimatorPath = new Dictionary<int, string>()
    {
        { (int)WeaponType.flute,"weapon/flute/PlayerAnimator"},
    };
    public readonly string Hakurei_House_Scene = "InGame";
    public readonly string Human_Village = "HumanVillage";

    public enum WeaponType
    {
        flute,
        guitar,
        violin,
        trumpet,
        harp,
        drum,
    }
    public enum PlaceID
    {
        Hakurei_Shrine = 0,
        Human_Village = 1,
    }

    public enum Weapon
    {
        HAKUREI_FLUTE = 0,
        HAKUREI_GUITAR = 1,
        HAKUREI_VIOLIN = 2,
        HAKUREI_TRUMPET = 3,
        HAKUREI_HARP = 4,
        HAKUREI_DRUM = 5,
    }
    public void OnExit(PlayModeStateChange mode)
    {
        Save.OnExit(mode);
    }
}

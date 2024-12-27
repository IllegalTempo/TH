using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class GameInformation
{
    public static SteamManager MainNetwork;

    //Keys
    public static GameObject LocalPlayer;
    public static Save currentsave;
    public static InGameSystemController system;
    public static LayerMask OnlyBuildings = LayerMask.GetMask("Buildings");
    public const int BuildingLayer = 8;
    public static Dictionary<int, string> WeaponPrefabPath = new Dictionary<int, string>()
    {
        { (int)Weapon.HAKUREI_FLUTE,"weapon/flute/HAKUREI_FLUTE/flute"},
    };
    public static Dictionary<int, string> WeaponPlayerAnimatorPath = new Dictionary<int, string>()
    {
        { (int)WeaponType.flute,"weapon/flute/PlayerAnimator"},
    };
    public enum WeaponType
    {
        flute,
        guitar,
        violin,
        trumpet,
        harp,
        drum,
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
    public static void OnExit(PlayModeStateChange mode)
    {
        Save.OnExit(mode);
    }
}

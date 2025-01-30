using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class GameInformation : MonoBehaviour
{
    public static GameInformation instance;
    public SteamManager MainNetwork;

    //Keys
    public GameObject LocalPlayer;
    public Save currentsave;
    public InGameSystemController system;
    public LayerMask OnlyBuildings;
    public LayerMask enemy;
    public LayerMask playerMask;
    public const int BuildingLayer = 8;
    public world CurrentWorld;
    public Wishboard missionsystem;
    public GameUIManager ui;




    public Dictionary<int, string> WeaponPrefabPath = new Dictionary<int, string>()
    {
        { (int)Weapon.HAKUREI_FLUTE,"weapon/flute/HAKUREI_FLUTE/flute"},
    };
    public Dictionary<int, string> WeaponPath = new Dictionary<int, string>()
    {
        { (int)WeaponType.flute,"weapon/flute/"},
    };
    public Dictionary<int, int> WeaponID2TypeID = new Dictionary<int, int>()
    {
        { (int)Weapon.HAKUREI_FLUTE,(int)WeaponType.flute}
    };
    
    public readonly string Hakurei_House_Scene = "InGame";
    public readonly string Human_Village = "HumanVillage";
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
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
    public enum HakureiShrine_Mission
    {
        Marisa_Ask_1 = 0,
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


    //Flute Damages
    public float A1_Weapon_Damage = 10f;
}

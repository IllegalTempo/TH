using Steamworks.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class GameInformation : MonoBehaviour
{
    //System Classes
    public static GameInformation instance;
    public SteamManager MainNetwork;
    public InGameSystemController system;
    public EnemyProjectileReserve PjtlReserve;
    public GameUIManager ui;
    public GridSystem gd;
    public Wishboard missionsystem;
    public PlayerInventory inventory;
    //General
    public GameObject LocalPlayer;
    public Save currentsave;
    public LayerMask OnlyBuildings;
    public LayerMask enemy;
    public LayerMask playerMask;
    public const int BuildingLayer = 8;
    public world CurrentWorld;
    public int State;
    public float ItemPickUpRange = 1f;
    public Lobby CurrentLobby;
    public enum GameState
    {
        StartScreen = 0,
        InGame = 1,
        InBattle = 2,
    }

    
    
    public readonly string Hakurei_House_Scene = "InGame";
    public readonly string Human_Village = "HumanVillage";
    public int GetMaxXP(int level)
    {
        return (int)(Mathf.Pow(1.3f, level) + Mathf.Sin(level*30)) * 10;
    }
    void Awake()
    {
        playerMask = LayerMask.GetMask("Player");
        OnlyBuildings = LayerMask.GetMask("Buildings");
        GameObject.Find("");

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
    public Type GetBoon(int id, out BoonInform info)
    {
        info = BoonInforms[id];
        return PlayerBoons[id];
    }
    public Type[] PlayerBoons =
    {
        typeof(con_OnAttack_Test),
    };
    public BoonInform[] BoonInforms =
    {
        new BoonInform("SpellCard/Alpha:Test","Spellcard #1 Test", 0,0,(int)BoonInform.ContinuouseBoonTypes.PlayerAttackBoon),
    };
    public Dictionary<string, Type> PrefixBoonSetupMatch = new Dictionary<string, Type>
    {
        {"Insane",
             typeof(Insane)
        },
    };

    public Dictionary<string, EnemySpawnMap> CoreEnemySpawnSetupMatch = new Dictionary<string, EnemySpawnMap>
    {
        {"Disgusting",
            new EnemySpawnMap(new Dictionary<float, int>{ {1,0} },5) 
        },
        {"Annoying",
            new EnemySpawnMap(new Dictionary<float, int>{ {1,0} },10)
        },
        {"Inferno",
            new EnemySpawnMap(new Dictionary<float, int>{ {1,0} },20)
        },

    };
    public Sprite[] SpellCardTexture;
    public Dictionary<int, string> Rarity = new()
    {
        {0,"Easy"},
        {1,"Normal" },
        {2,"Hard" },
        {3, "Lunatic" },
        {4, "Maiden" },
        {5, "BlackMarket" },
        {6, "Chronicle" },
        {7, "Matara's" },
    };
    public Dictionary<string, Type> SuffixRoomRewardMatch = new Dictionary<string, Type>
    {
        {"Vault",
            typeof(Vault)

        },
    };
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
    public Dictionary<int, GameObject> EnemyInstances = new Dictionary<int, GameObject>();
    public Dictionary<string, Sprite> EnemyBoonImage = new Dictionary<string, Sprite>();

    public Dictionary<int, string> EnemyInstancesPath = new Dictionary<int, string>()
    {
        { 0,"enemies/fairy/0"}
    };
    public Dictionary<string, string> EnemyBoonImagePath = new Dictionary<string, string>()
    {
        { "Insane","boons/enemy/Insane"}
    };
    public GameObject powerDropInstance;
    public GameObject pointDropInstance;
    public GameObject GetEnemyInstances(int EnemyID)
    {
        if (!EnemyInstances.TryGetValue(EnemyID, out _))
        {
            EnemyInstances.Add(EnemyID, Resources.Load<GameObject>(EnemyInstancesPath[EnemyID]));

        }
        return EnemyInstances[EnemyID];
    }
    public Sprite GetEnemyBoonImage(string boonName)
    {
        if (!EnemyBoonImage.TryGetValue(boonName, out _))
        {
            EnemyBoonImage.Add(boonName, Resources.Load<Sprite>(EnemyBoonImagePath[boonName]));

        }
        return EnemyBoonImage[boonName];
    }
    //class for storing Game assests;
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

}

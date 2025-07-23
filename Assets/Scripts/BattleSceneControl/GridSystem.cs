using Assets.Scripts.BattleSceneControl;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class GridSystem : MonoBehaviour
{

    [Header("Preset GameObject")]
    [SerializeField]
    public GameObject InitialRoom;
    public bool CurrentRoomCompleted = false;
    public int seed;
    public AnimationCurve heightcurve;
    public Transform EnemiesGroup;
    public Room CurrentRoom;
    public bool GDInitialized = false;
    public float roomsize = 200; // 200*200
    //private int GetGridItem(Vector2 res)
    //{
    //    if (res.x < 0 || res.y < 0 || res.x >= grid.GetLength(0) || res.y >= grid.GetLength(0)) return 999;
    //    return grid[(int)res.x, (int)res.y];
    //}
    //public static Vector2 Chunksize = new Vector2(100, 100);
    public int TPC = 50;

    public List<int> ReadySpawnChunkPlayerList;
    //public GameObject Chunk;
    //public GameObject Barrier;

    //public Transform BarrierGroup;
    public string ranPrefix;
    public string ranSuffix;
    public string ranCorefix;
    private void Start()
    {
        GameInformation.instance.gd = this;
        if (GameInformation.instance.MainNetwork.IsServer)
        {
            ReadySpawnChunkPlayerList = new List<int>();
            seed = (int)Time.time;
            PacketSend.Server_Send_TransferBattle(seed);
            ReadySpawnChunkPlayerList.Add(0);
            Random.InitState(seed);
        }



    }
    public void EveryoneReady()
    {
        GameUIManager.instance.NewMessage("Everyone's Ready!");
        CurrentRoomCompleted = true;
        //StartGridSystem(true);
    }
    //private Vector2 NextPos()
    //{

    //    Vector2[] dir = new Vector2[]
    //    {
    //        new Vector2(1,0),
    //        new Vector2(-1,0),
    //        new Vector2(0,1),
    //        new Vector2(0,-1)
    //    };
    //    List<Vector2> EmptyPos = new List<Vector2>();
    //    for (int i = 0; i < 4; i++)
    //    {
    //        Vector2 rs = currentpos + dir[i];
    //        if (GetGridItem(rs) == 0) //if available chunk
    //        {
    //            int avi = 0;
    //            for (int j = 0; j < dir.Length; j++)
    //            {
    //                Vector2 pos = dir[j] + rs;
    //                if (GetGridItem(pos) == 0)
    //                {
    //                    avi++;
    //                }


    //            }
    //            if (avi >= 3)
    //            {
    //                EmptyPos.Add(rs);

    //            }
    //        }
    //    }
    //    if (EmptyPos.Count == 0) return new Vector2(5, 5);
    //    return EmptyPos[UnityEngine.Random.Range(0, EmptyPos.Count)];
    //}
    //private void GenerateBarrierChunks()
    //{
    //    Vector3 brs = new Vector3(Chunksize.x, 100, Chunksize.y);
    //    int[,] Barrierinfo = new int[grid.GetLength(0), grid.GetLength(1)];
    //    for (int x = 0; x < Barrierinfo.GetLength(0); x++)
    //    {
    //        for (int y = 0; y < Barrierinfo.GetLength(1); y++)
    //        {
    //            if (grid[x, y] != 0)
    //            {
    //                Barrierinfo[x + 1, y] = 1;
    //                Barrierinfo[x - 1, y] = 1;
    //                Barrierinfo[x, y + 1] = 1;
    //                Barrierinfo[x, y - 1] = 1;
    //                Barrierinfo[x, y] = 0;

    //            }
    //        }
    //    }
    //    for (int x = 0; x < Barrierinfo.GetLength(0); x++)
    //    {
    //        for (int y = 0; y < Barrierinfo.GetLength(1); y++)
    //        {
    //            if (grid[x, y] != 0)
    //            {
    //                Barrierinfo[x, y] = 0;

    //            }
    //        }
    //    }
    //    for (int x = 0; x < Barrierinfo.GetLength(0); x++)
    //    {
    //        for (int y = 0; y < Barrierinfo.GetLength(1); y++)
    //        {
    //            if (Barrierinfo[x, y] == 1)
    //            {
    //                GameObject chunk = Instantiate(Barrier, ToWorld(new Vector2(x, y)), Quaternion.identity, BarrierGroup);
    //                chunk.transform.localScale = brs;
    //            }

    //        }
    //    }
    //}

    //public Dictionary<Vector2, Room> RoomV2Match = new Dictionary<Vector2, Room>();
    //public Room GetRoombyLocalPos(Vector2 localpos)
    //{
    //    return RoomV2Match[localpos];
    //}
    //public enum ChunkType
    //{
    //    GrassPlane,
    //    SnowyPlane,
    //    SnowyMountain,
    //    Lake,
    //}
    public Vector3 randomWorldPosOnSurface()
    {
        NavMeshHit hit;
        for (int i = 0; i < 30; i++)
        {
            Vector3 dir = Random.insideUnitSphere * roomsize;
            dir.y = 0;
            Vector3 center = new Vector3(0,5,0) + dir;
            if (NavMesh.SamplePosition(dir, out hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }
    //public Vector3 randomLocalPosOnSurface(Vector3 center)
    //{
    //    return randomWorldPosOnSurface(ToWorld(center));
    //}
    private int NumberOfRooms()
    {
        string searchPattern = "*.prefab";
        return Directory.GetFiles(Application.dataPath + "/rooms",searchPattern).Length;
    }
    //public void SpawnRandomChunk(string prefix, string corefix, string suffix)
    //{
    //    SpawnChunk(prefix, corefix, suffix, Random.Range(0,NumberOfRooms()));
    //}
    public void SpawnRandomChunk()
    {
        SpawnChunk(Random.Range(0, NumberOfRooms()));
    }
    public void SpawnChunk(int roomid)
    {
        if (CurrentRoom != null)
        {
            Destroy(CurrentRoom.gameObject);
        }
        string path = $"rooms/{roomid}";
        Debug.Log(path);
        GameObject res = Resources.Load<GameObject>(path);
        GameObject g = Instantiate(res, new Vector3(-roomsize / 2, 0, -roomsize / 2), Quaternion.identity);

        Room r = g.GetComponent<Room>();
        CurrentRoom = r;
        GameInformation.instance.LocalPlayer.transform.position = r.GetRoomSpawnPoint();
        //r.WaveComplete(); //Run for first time, used for first wave
        GameUIManager.instance.StopLoading();
    }
    //public void SpawnChunk(string prefix, string corefix, string suffix, int roomid)
    //{
    //    if(CurrentRoom!=null)
    //    {
    //        Destroy(CurrentRoom.gameObject);
    //    }
    //    string path = $"rooms/{roomid}";
    //    Debug.Log(path);
    //    GameObject res = Resources.Load<GameObject>(path);
    //    GameObject g = Instantiate(res,new Vector3(-roomsize/2,0,-roomsize/2),Quaternion.identity);
        
    //    Room r = g.AddComponent<Room>();
    //    //r.AddRoomReward(suffix);
    //    CurrentRoom = r;
    //    //EnemySpawnMap spawnmap = GameInformation.instance.CoreEnemySpawnSetupMatch[corefix];


    //    //foreach (KeyValuePair<float, int> kvp in spawnmap.Map)
    //    //{
    //    //    float sample = Random.value * (kvp.Key / spawnmap.TotalWeight) * spawnmap.maxcount;
    //    //    for (int i = 0; i < sample; i++)
    //    //    {

    //    //        //Vector3 SpawnPos = ToWorld(pos) + Random.insideUnitSphere * Chunksize.x * (transform.localScale.x / 2);
    //    //        //SpawnPos.y = 50f;

    //    //        Vector3 SpawnPos = randomWorldPosOnSurface();
    //    //        SpawnEnemy(SpawnPos,  kvp.Value, prefix);

    //    //    }
    //    //}
    //    GameUIManager.instance.StopLoading();
    //    //for(int i =0; i< TPC;i++)
    //    //{
    //    //    Instantiate(TreePrefab[UnityEngine.Random.Range(0, TreePrefab.Length)], randomWorldPosOnSurface(ToWorld(pos)), Quaternion.Euler(transform.rotation.eulerAngles.x, UnityEngine.Random.Range(0f, 360f), transform.rotation.eulerAngles.z), TreeGroup);

    //    //}
    //}

    private void SpawnEnemy(Vector3 SpawnPos,  int enemyid, string prefix)
    {

        

    }
    //private GameObject SpawnTerrain(Vector2 pos, ChunkType t, Vector2 size)
    //{
    //    grid[(int)pos.x, (int)pos.y] = 1;
    //    GameObject chunk = Instantiate(Chunk, ToWorld(pos), Quaternion.identity, transform);
    //    TerrainGeneration tg = chunk.GetComponent<TerrainGeneration>();
    //    tg.Generate(seed, t, size, heightcurve);
    //    return chunk;
    //}
    //public Vector3 ToWorld(Vector2 pos)
    //{
    //    return new Vector3(pos.x * Chunksize.x * transform.localScale.x, 0, pos.y * Chunksize.y * transform.localScale.x);
    //}
    
    public void ClientInitGridSystem(int seed)
    {
        this.seed = seed;
        Random.InitState(seed);
        PacketSend.Client_Send_ReadySpawnChunk();
    }
    public void PlayerReady(NetworkPlayer p)
    {
        GameInformation.instance.gd.ReadySpawnChunkPlayerList.Add(p.NetworkID); 
        GameUIManager.instance.NewMessage($"{p.SteamName} is Ready!");
        if (ReadySpawnChunkPlayerList.Count >= GameInformation.instance.MainNetwork.server.GetPlayerCount())
        {
            EveryoneReady();
            PacketSend.Server_Send_EveryoneReady();
        }

    }
    public NavMeshSurface sur;

    private Dictionary<float, int> EnemiesVariance = new Dictionary<float, int>
    {
        { 1, 0 },
    };
    string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=";

    private char GenerateRandomChar()
    {
        // Choose from different character sets
        return characterSet[Random.Range(0, characterSet.Length)];
    }

    //private Vector2 NextPosAll(Vector2 v)
    //{
    //    return new Vector2(v.x + (int)((v.y + 1) / grid.GetLength(0)), (v.y + 1) % grid.GetLength(0));
    //}
    //public void RollRoomArguments()
    //{

    //    ranPrefix = GameInformation.instance.PrefixBoonSetupMatch.Keys.ElementAt(Random.Range(0, GameInformation.instance.PrefixBoonSetupMatch.Count));
    //    ranCorefix = GameInformation.instance.CoreEnemySpawnSetupMatch.Keys.ElementAt(Random.Range(0, GameInformation.instance.CoreEnemySpawnSetupMatch.Count));
    //    ranSuffix = GameInformation.instance.SuffixRoomRewardMatch.Keys.ElementAt(Random.Range(0, GameInformation.instance.SuffixRoomRewardMatch.Count));
    //    StartCoroutine(GameUIManager.instance.SetRoomArgument(ranPrefix, ranCorefix, ranSuffix));
    //}
    public void GenerateNextRoom()
    {
        if(!StartEnteringRooms)
        {
            Destroy(InitialRoom);
            StartEnteringRooms = true;
        }
        if(!GameInformation.instance.MainNetwork.IsServer)
        {
            PacketSend.Client_Send_SpawnChunk(GameInformation.instance.MainNetwork.client.NetworkID);

        } else
        {
            PacketSend.Server_DistributeSpawnChunk(0);
        }
        GameUIManager.instance.StartLoading("Loading Room");
        SpawnNextRoom = false;
        CurrentRoomCompleted = false;
        //GameUIManager.instance.ConfirmClickNextRoom();
        SpawnRandomChunk();

    }
    public bool SpawnNextRoom = false;
    public bool StartEnteringRooms = false;
    //private void Update()
    //{
    //    if ((Input.GetKeyDown(KeyMap.Interact2) || SpawnNextRoom) && CurrentRoomCompleted)
    //    {
    //        GenerateNextRoom();
    //    }
    //}
    //public IEnumerator genchunks(bool spawnenmeies)
    //{
    //    //TreeGroup = new GameObject("Trees").transform;
    //    //TreeGroup.parent = transform;
        
    //    grid = new int[10, 10];
    //    currentpos = new Vector2(5, 5);
    //    //for (int x =1;x < grid.GetLength(0)-1;x++)
    //    //{
    //    //    for(int y=  1; y < grid.GetLength(1)-1;y++)
    //    //    {
    //    //        Vector2 pos = new Vector2(x, y);
    //    //        SpawnChunk(pos, ChunkType.GrassPlane, EnemiesVariance, 50, spawntree, spawnenmeies);
    //    //        //Vector2 Nextpos = NextPosAll(currentpos);
    //    //        //currentpos = Nextpos;
    //    //        //if (currentpos == new Vector2(5, 5))
    //    //        //{
    //    //        //    break;
    //    //        //}
    //    //    }

    //    //SpawnChunk(currentpos, ChunkType.GrassPlane, EnemiesVariance, 50, spawntree, spawnenmeies);

    //    RollRoomArguments();

    //    //GenerateNextRoom();
    //    yield return new WaitForSeconds(1f);
    //}
    public void StartGridSystem(bool SpawnEnemies)
    {
        GameUIManager.instance.StartLoading("Starting Battle Spawning Room");
        GDInitialized = true;
        EnemiesGroup = new GameObject("Enemies").transform;
        EnemiesGroup.parent = transform;
        //GameUIManager.instance.StartRollRoom();
        //RollRoomArguments();
        GenerateNextRoom();

    }
}

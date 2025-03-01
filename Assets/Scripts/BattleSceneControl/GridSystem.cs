using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class GridSystem : MonoBehaviour
{
    public enum GridBoxType
    {
        Air = 0,

    }
    public int seed;
    public int[,] grid = new int[5,5];
    public AnimationCurve heightcurve;
    public Vector2 currentpos = new Vector2(5,5);
    private float RoadLength = 0;
    public enum direction { Up, Down, Right, Left }
    public Transform EnemiesGroup;
    public Transform TreeGroup;
    public GameObject[] EnemiesInstances;
    private int GetGridItem(Vector2 res)
    {
        if (res.x < 0 || res.y < 0 || res.x >= grid.GetLength(0) || res.y >= grid.GetLength(0)) return 999;
        return grid[(int)res.x, (int)res.y];
    }
    public static Vector2 Chunksize = new Vector2(100, 100);
    public GameObject[] TreePrefab;
    public int TPC = 50;
    private Vector2 NextPos()
    {

        Vector2[] dir = new Vector2[]
        {
            new Vector2(1,0),
            new Vector2(-1,0),
            new Vector2(0,1),
            new Vector2(0,-1)
        };
        List<Vector2> EmptyPos = new List<Vector2>();
        for(int i = 0; i < 4; i ++)
        {
            Vector2 rs = currentpos + dir[i];
            if (GetGridItem(rs) == 0) //if available chunk
            {
                int avi = 0;
                for(int j = 0; j < dir.Length;j++)
                {
                    Vector2 pos = dir[j] + rs;
                    Debug.Log(pos);
                        if (GetGridItem(pos) == 0)
                        {
                            avi++;
                        }
                    
                    
                }
                if(avi >= 3)
                {
                    EmptyPos.Add(rs);

                }
            }
        }
        if (EmptyPos.Count == 0) return new Vector2(5, 5);
        return EmptyPos[UnityEngine.Random.Range(0, EmptyPos.Count)];
    }
    private void GenerateBarrierChunks()
    {
        Vector3 brs = new Vector3 (Chunksize.x, 100, Chunksize.y);
        int[,] Barrierinfo = new int[grid.GetLength(0), grid.GetLength(1)];
        for(int x = 0; x < Barrierinfo.GetLength(0);x++)
        {
            for (int y = 0; y < Barrierinfo.GetLength(1); y++)
            {
                if (grid[x,y] != 0)
                {
                    Barrierinfo[x + 1, y] = 1;
                    Barrierinfo[x - 1, y] = 1;
                    Barrierinfo[x, y+1] = 1;
                    Barrierinfo[x, y - 1] = 1;
                    Barrierinfo[x, y] = 0;

                }
            }
        }
        for (int x = 0; x < Barrierinfo.GetLength(0); x++)
        {
            for (int y = 0; y < Barrierinfo.GetLength(1); y++)
            {
                if (grid[x, y] != 0)
                {
                    Barrierinfo[x, y] = 0;

                }
            }
        }
        for (int x = 0; x < Barrierinfo.GetLength(0); x++)
        {
            for(int y = 0;y < Barrierinfo.GetLength(1);y++)
            {
                if (Barrierinfo[x,y] == 1)
                {
                    GameObject chunk = Instantiate(Barrier, ToWorldbar(new Vector2(x, y)), Quaternion.identity, transform);
                    chunk.transform.localScale = brs;
                }

            }
        }
    }
    public GameObject Chunk;
    public GameObject Barrier;

    public enum ChunkType
    {
        GrassPlane,
        SnowyPlane,
        SnowyMountain,
        Lake,
    }
    public static Vector3 randomWorldPosOnSurface(Vector3 center)
    {
        NavMeshHit hit;
        for (int i = 0; i < 30; i++)
        {
            Vector3 dir = center + Random.insideUnitSphere * (GridSystem.Chunksize.x * 5f);
            dir.y = 0;
            if (NavMesh.SamplePosition(dir, out hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return center;
    }
    public static Vector3 randomLocalPosOnSurface(Vector3 center)
    {
        return randomWorldPosOnSurface(ToWorld(center));
    }
    public void SpawnChunk(Vector2 pos,ChunkType t,Dictionary<float,int> EnemiesSpawnWeight,int MaxEnemiesCount,bool spawntree,bool spawnenemies)
    {
        SpawnTerrain(pos, t, Chunksize,spawntree);
        float total = EnemiesSpawnWeight.Keys.Sum();

        if(spawnenemies)
        {
            foreach (KeyValuePair<float, int> kvp in EnemiesSpawnWeight)
            {
                float sample = Random.value * (kvp.Key / total) * MaxEnemiesCount;
                for (int i = 0; i < sample; i++)
                {

                    Vector3 SpawnPos = ToWorld(pos) + Random.insideUnitSphere * Chunksize.x * 5f;
                    SpawnPos.y = 100f;
                    GameObject enemy = Instantiate(EnemiesInstances[kvp.Value], SpawnPos, Quaternion.identity, EnemiesGroup);
                    enemy.GetComponent<AiEnemy>().InChunk = pos;
                }
            }
        }
        //for(int i =0; i< TPC;i++)
        //{
        //    Instantiate(TreePrefab[UnityEngine.Random.Range(0, TreePrefab.Length)], randomWorldPosOnSurface(ToWorld(pos)), Quaternion.Euler(transform.rotation.eulerAngles.x, UnityEngine.Random.Range(0f, 360f), transform.rotation.eulerAngles.z), TreeGroup);

        //}
    }
    private GameObject SpawnTerrain(Vector2 pos, ChunkType t,Vector2 size,bool spawntree)
    {
        grid[(int)pos.x, (int)pos.y] = 1;
        GameObject chunk = Instantiate(Chunk, ToWorld(pos), Quaternion.identity, transform);
        TerrainGeneration tg = chunk.GetComponent<TerrainGeneration>();
        tg.Generate(seed, t, size,spawntree,heightcurve);
        chunk.GetComponent<NavMeshSurface>().BuildNavMesh();
        return chunk;
    }
    private static Vector3 ToWorld(Vector2 pos)
    {
        return new Vector3(pos.x * Chunksize.x * 10,0,pos.y * Chunksize.y * 10);
    }
    private Vector3 ToWorldbar(Vector2 pos)
    {
        return new Vector3(pos.x * Chunksize.x, 0, pos.y * Chunksize.y);
    }
    private void Start()
    {
        UnityEngine.Random.InitState(seed);
        seed = (int)Time.time;
        
        StartCoroutine(genchunks(true,true));
        


    }
    public NavMeshSurface sur;
    private Dictionary<float,int> EnemiesVariance = new Dictionary<float, int> 
    {
        { 1, 0 },
    };
    private Vector2 NextPosAll(Vector2 v)
    {
        return new Vector2(v.x + (int)((v.y + 1) / grid.GetLength(0)), (v.y + 1) % grid.GetLength(0));
    }
    public IEnumerator genchunks(bool spawntree,bool spawnenmeies)
    {
        TreeGroup = new GameObject("Trees").transform;
        TreeGroup.parent = transform;
        EnemiesGroup = new GameObject("Enemies").transform;  
        EnemiesGroup.parent = transform;
        grid = new int[10, 10];
        currentpos = new Vector2(5, 5);
        //for (int x =1;x < grid.GetLength(0)-1;x++)
        //{
        //    for(int y=  1; y < grid.GetLength(1)-1;y++)
        //    {
        //        Vector2 pos = new Vector2(x, y);
        //        SpawnChunk(pos, ChunkType.GrassPlane, EnemiesVariance, 50, spawntree, spawnenmeies);
        //        //Vector2 Nextpos = NextPosAll(currentpos);
        //        //currentpos = Nextpos;
        //        //if (currentpos == new Vector2(5, 5))
        //        //{
        //        //    break;
        //        //}
        //    }

        //SpawnChunk(currentpos, ChunkType.GrassPlane, EnemiesVariance, 50, spawntree, spawnenmeies);


        

            SpawnChunk(currentpos, ChunkType.GrassPlane, EnemiesVariance, 50, spawntree, spawnenmeies);

        GenerateBarrierChunks();
        yield return new WaitForSeconds(1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public enum GridBoxType
    {
        Air = 0,

    }
    public NavMeshSurface navs;
    private int seed;
    public int[,] grid = new int[11,11];
    public int[] currentpos = { 5, 5 };
    public enum direction { Up, Down, Right, Left }
    public int[] NextPos(direction dir)
    {

        int nextx = currentpos[0];
        int nexty = currentpos[1];
        switch (dir)
        {

            case direction.Up:
                if(nexty+1 >= grid.GetLength(1))
                {
                    return null;
                } else
                {
                    return new int[]{ nextx,nexty+1};
                }
            case direction.Down:

                if (nexty-1 < 0)
                {
                    return null;
                }
                else
                {
                    return new int[] { nextx, nexty-1 };
                }
            case direction.Right:
                if (nextx + 1 >= grid.GetLength(0))
                {
                    return null;
                }
                else
                {
                    return new int[] { nextx+1, nexty};
                }
            case direction.Left:

                if (nextx - 1 < 0)
                {
                    return null;
                }
                else
                {
                    return new int[] { nextx-1, nexty};
                }

        }
        return null;

    }
    public GameObject Chunk;
    public void SpawnChunk(int[] pos)
    {
        GameObject chunk = Instantiate(Chunk, new Vector3(pos[0] * TerrainGeneration.xsize, 0, pos[1] * TerrainGeneration.ysize),Quaternion.identity,transform);
        TerrainGeneration tg = chunk.GetComponent<TerrainGeneration>();
        tg.Generate(seed);
    }
    
    private void Start()
    {
        seed = (int)Time.time;
        SpawnChunk(currentpos);
        SpawnChunk(NextPos(direction.Up));
        navs.BuildNavMesh();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRunSave : MonoBehaviour
{
    public int MapSeed;
    public float[] LastLocation = new float[3];
    public List<boon> AllBoons = new List<boon>();

    public LastRunSave(int seed)
    {
        AllBoons.Clear();
        LastLocation = new float[3];
        MapSeed = seed;
    }
}

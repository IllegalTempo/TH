using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class EnemySpawnMap
{
    public Dictionary<float, int> Map;
    public float TotalWeight;
    public int maxcount;
    public EnemySpawnMap(Dictionary<float, int> map, int MaxEnemyCount)
    {
        Map = map;
        maxcount = MaxEnemyCount;
        TotalWeight = map.Keys.Sum();
    }
    public void addEnemySet(float weight, int EnemyID)
    {
        Map.Add(weight, EnemyID);
    }
}

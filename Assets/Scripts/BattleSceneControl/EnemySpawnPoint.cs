using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public int EnemyID;
    public Enemy Spawn(int InWaveIndex)
    {
        GameObject enemy = Instantiate(GameInformation.instance.GetEnemyInstances(EnemyID), transform.position, transform.rotation, GameInformation.instance.gd.EnemiesGroup);

        Enemy e = enemy.GetComponent<Enemy>();
        e.InWaveIndex = InWaveIndex;
        return e;
    }
}

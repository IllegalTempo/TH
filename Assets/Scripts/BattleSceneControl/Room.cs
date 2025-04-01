using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Scripts.BattleSceneControl
{
    [System.Serializable]
    class Wave
    {
        public EnemySpawnPoint[] SpawningObjects;
    }
    public class Room : MonoBehaviour
    {

        public RoomReward[] rewards = new RoomReward[3];
        public int RewardCount = 0;
        public List<Enemy> enemies = new List<Enemy>();
        public Vector3 ChunkPos; //Local
        [SerializeField]
        private Wave[] waves;
        public int wave = 0;
        public int currentRemainingEnemies;
        public void WaveComplete()
        {

            if (wave >= waves.Length)
            {
                RoomCompleted();
            } else
            {
                Debug.Log("Next Wave! | Wave:" + wave);
                Wave currentwave = waves[wave];
                currentRemainingEnemies = currentwave.SpawningObjects.Length;
                for (int i = 0; i < currentwave.SpawningObjects.Length; i++)
                {
                    AddEnemy(currentwave.SpawningObjects[i].Spawn(i));
                }
                wave++;
            }
            
        }
        public void EnemyKilled(Enemy e)
        {
            enemies.Remove(e);

            currentRemainingEnemies--;
            if (currentRemainingEnemies <= 0)
            {
                WaveComplete();
            }
        }

        public void AddRoomReward(string suffixname)
        {
            if (RewardCount >= rewards.Length) return;
            Type t = GameInformation.instance.SuffixRoomRewardMatch[suffixname];
            RoomReward r = (RoomReward)gameObject.AddComponent(t);
            rewards[RewardCount] = r;
            r.init(this);
            RewardCount++;
        }
        public int AddEnemy(Enemy e)
        {
            e.BelongTo = this;
            enemies.Add(e);
            return enemies.Count - 1;
        }
        private void RoomCompleted()
        {
            //GameUIManager.instance.StartRollRoom();
            //GameInformation.instance.gd.RollRoomArguments();
            GameInformation.instance.gd.CurrentRoomCompleted = true;
            for(int i = 0; i < RewardCount;i++)
            {
                rewards[i].OnRoomComplete();
            }
        }
    }
}

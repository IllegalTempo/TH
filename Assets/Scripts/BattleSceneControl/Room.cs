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
    public class Room : MonoBehaviour
    {
        public RoomReward[] rewards = new RoomReward[3];
        public int RewardCount = 0;
        public List<Enemy> enemies = new List<Enemy>();
        public Vector3 ChunkPos; //Local
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
            GameInformation.instance.ui.StartRollRoom();
            GameInformation.instance.gd.RollRoomArguments();
            GameInformation.instance.gd.CurrentRoomCompleted = true;
            for(int i = 0; i < RewardCount;i++)
            {
                rewards[i].OnRoomComplete();
            }
        }
        public void RemoveEnemy(Enemy e)
        {
            enemies.Remove(e);
            if(enemies.Count == 0)
            {
                RoomCompleted();
            }
        }
    }
}

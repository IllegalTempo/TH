using Assets.Scripts.BattleSceneControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
[Serializable]
public class ItemInfo
{
    public int ItemID;
    public float DropWeight;
}
public class Enemy : MonoBehaviour
{
    //values of enemy
    public int InWaveIndex;
    public EnemySpawnControl Spawner;
    public float health;
    public float maxhealth;
    public bool invulnerable;
    public float value; //ability to drop powers and points
    public float DamageMultiplier;

    [SerializeField]
    private Animator animator;
    public ParticleSystem OnHitEffect;
    public Renderer SoulRenderer;
    public int[] UsedProjectileList = new int[0];
    public ItemInfo[] ItemDropTable;
    private float DropTableTotalWeight;
    public EnemyBulletPool[] ProjectilePools;
    public Room BelongTo;
    public int EnemyIDinRoom;
    public long uuid;
    public AiWalkEnemy Ai;
    public EnemyBoonBase[] Boons = new EnemyBoonBase[3];
    public int BoonCount = 0;
    //public void AddBoon(string PrefixName)
    //{
    //    if (BoonCount > 2) return;
    //    Type t = GameInformation.instance.PrefixBoonSetupMatch[PrefixName];
    //    Boons[BoonCount] = (EnemyBoonBase)gameObject.AddComponent(t);
    //    BoonCount++;

    //}
    private void Start()
    {
        uuid = GameInformation.instance.GetUUID();
        GameInformation.instance.AllEnemies.Add(uuid, this);
        if(UsedProjectileList.Length > 0)
        {
            ProjectilePools = new EnemyBulletPool[UsedProjectileList.Length];
            //Initialize Projectile Pool
            for (int i = 0; i < UsedProjectileList.Length; i++)
            {
                ProjectilePools[i] = GameInformation.instance.PjtlReserve.UsePool(UsedProjectileList[i]);
            }
        }
        
    }
    public void SetHealth(float dmg)
    {
        MaterialPropertyBlock pb = new MaterialPropertyBlock();
        float nhealth = health -= dmg;
        pb.SetFloat("_Fill", (nhealth * 0.5f) / maxhealth);
        SoulRenderer.SetPropertyBlock(pb);
        if (nhealth <= 0)
        {
            OnDeath();
        }
        else
        {
            animator.Play("HURT");

        }
    }
    public void Damage(float dmg,Vector3 collidepoint,PlayerMain DamageDealer,int soundid,int effectid)
    {
        GameSystem.instance.PlayEffect(effectid,collidepoint);
        GameSystem.instance.PlaySound(soundid, collidepoint);
        if (!invulnerable)
        {
            if (!DamageDealer.IsLocal) return;
            SetHealth(dmg);
            if (GameInformation.instance.MainNetwork.IsServer)
            {
                PacketSend.Server_DistributeEnemyHealthUpdate(0, uuid, dmg);
            }
            else
            {
                PacketSend.Client_Send_EnemyHealthUpdate(uuid, dmg);
            }
        }
            animator.Play("HURT");



        
        
    }
    private void OnDeath()
    {
        animator.Play("OnDeath");
        BelongTo.EnemyKilled(this);
        int pointdrop = (int)Random.Range(0, value);
        int powerdrop = (int)Random.Range(0, value);

        GameObject pointinstance = GameInformation.instance.pointDropInstance;
        for (int i = 0; i < powerdrop; i++)
        {
            GameSystem.instance.SpawnPowerDrops(transform.position);

        }
        for (int i = 0; i < pointdrop; i++)
        {
            GameSystem.instance.SpawnPointDrops(transform.position);

        }
        Ai.Stopped = true;
    }
    public void OnDisappear()
    {
        //Save currentsave = GameInformation.instance.currentsave;
        //int[] meetmissions = currentsave.FindMissionByID((int)PrayerMission.MissionType.Beat,EnemyID);
        //for(int i = 0; i < meetmissions.Length;i++)
        //{
        //    currentsave.ActiveMissions[i].count++;
        //}
        for (int i = 0; i < UsedProjectileList.Length; i++)
        {
            GameInformation.instance.PjtlReserve.UnUsePool(UsedProjectileList[i]);
        }
        Destroy(gameObject);
    }
}

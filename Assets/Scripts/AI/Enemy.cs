using Assets.Scripts.BattleSceneControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    //values of enemy
    public float health;
    public float maxhealth;
    public bool invulnerable;
    public float value; //ability to drop powers and points

    [SerializeField]
    private Animator animator;
    public ParticleSystem OnHitEffect;
    public Renderer SoulRenderer;
    public int[] UsedProjectileList = new int[0];
    public EnemyBulletPool[] ProjectilePools;
    public Room BelongTo;
    public AiEnemy AiClass;
    public int EnemyIDinRoom;

    public EnemyBoonBase[] Boons = new EnemyBoonBase[3];
    public int BoonCount = 0;
    public void AddBoon(string PrefixName)
    {
        if (BoonCount > 2) return;
        Type t = GameInformation.instance.PrefixBoonSetupMatch[PrefixName];
        Boons[BoonCount] = (EnemyBoonBase)gameObject.AddComponent(t);
        BoonCount++;

    }
    private void Start()
    {
        AiClass = GetComponent<AiEnemy>();
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
    public void Damage(float dmg,Vector3 collidepoint)
    {
        if(!invulnerable)
        {
            float newhealth = health -= dmg;
            MaterialPropertyBlock pb = new MaterialPropertyBlock();

            pb.SetFloat("_Fill", (newhealth*0.5f) / maxhealth);
            SoulRenderer.SetPropertyBlock(pb);

        }
        if (health <= 0)
        {
            OnDeath();
        }else
        {
            animator.Play("HURT");

        }


        OnHitEffect.transform.position = collidepoint;
        OnHitEffect.Play();
    }
    private void OnDeath()
    {
        animator.Play("OnDeath");
        BelongTo.RemoveEnemy(this);
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
        AiClass.StopAction();
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

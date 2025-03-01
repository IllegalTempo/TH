using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float maxhealth;
    public bool invulnerable;
    public int EnemyID;
    [SerializeField]
    private Animator animator;
    public ParticleSystem OnHitEffect;
    public Renderer SoulRenderer;
    public int[] UsedProjectileList;
    public EnemyBulletPool[] ProjectilePools;
    private void Start()
    {
        ProjectilePools = new EnemyBulletPool[UsedProjectileList.Length];
        //Initialize Projectile Pool
        for (int i = 0; i < UsedProjectileList.Length; i++)
        {
           ProjectilePools[i] = GameInformation.instance.PjtlReserve.UsePool(UsedProjectileList[i]);
        }
    }
    public void Damage(float dmg,Vector3 collidepoint)
    {
        if(!invulnerable)
        {
            health -= dmg;
            MaterialPropertyBlock pb = new MaterialPropertyBlock();

            pb.SetFloat("_Fill", health / maxhealth);
            SoulRenderer.SetPropertyBlock(pb);

        }
        if (health <= 0)
        {
            OnDeath();
        }

        
        OnHitEffect.transform.position = collidepoint;
        OnHitEffect.Play();
        animator.Play("HURT");
    }
    public void OnDeath()
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

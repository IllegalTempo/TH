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
    public void Damage(float dmg,Vector3 collidepoint)
    {
        if(!invulnerable)
        {
            health -= dmg;

        }
        OnHitEffect.transform.position = collidepoint;
        OnHitEffect.Play();
        animator.Play("HURT");
    }
    public void OnDeath()
    {
        Save currentsave = GameInformation.instance.currentsave;
        int[] meetmissions = currentsave.FindMissionByID((int)PrayerMission.MissionType.Beat,EnemyID);
        for(int i = 0; i < meetmissions.Length;i++)
        {
            currentsave.ActiveMissions[i].count++;
        }
    }
}

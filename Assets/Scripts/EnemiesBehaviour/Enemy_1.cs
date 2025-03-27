using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : AiWalkEnemy
{
    protected override void Attack()
    {
        EnemyBullet bullet = MainClass.ProjectilePools[0].GetBulletobject();
        bullet.damagemultiplier = MainClass.DamageMultiplier;
        bullet.transform.position = Head.transform.position;
        bullet.transform.rotation = Head.transform.rotation;
        attackcd = 2f;
        Debug.Log("Attack");
    }
}

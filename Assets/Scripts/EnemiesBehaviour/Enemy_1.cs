using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : AiEnemy
{
    protected override void Attack()
    {
        Transform bullet = MainClass.ProjectilePools[0].GetBulletobject().transform;
        
        bullet.position = Head.transform.position;
        bullet.rotation = Head.transform.rotation;
        attackcd = 2f;
        Debug.Log("Attack");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insane : EnemyBoonBase
{
    public override void OnAttack()
    {
        Debug.Log("Insane Attack!");
    }

    public override void OnDeath()
    {
    }

    protected override void ContinEffect()
    {
    }

    protected override void PermEffect()
    {
        ai.speed *= 2;

    }
}

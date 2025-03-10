using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_1_a1 : EnemyBullet
{
    protected override Vector3 BulletMovement()
    {
        return new Vector3(0, 0, 1);
    }
}

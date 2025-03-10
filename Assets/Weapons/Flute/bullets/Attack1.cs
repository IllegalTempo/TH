using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : bulletobject
{
    protected override Vector3 BulletMovement()
    {
        return new Vector3(0, 0, 1);
    }
}

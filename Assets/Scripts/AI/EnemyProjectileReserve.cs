using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileReserve : MonoBehaviour
{
    public GameObject[] Projectiles;

    private Dictionary<int,PoolInform> BulletPoolsInform = new Dictionary<int, PoolInform>();



    private EnemyBulletPool CreateBulletPool(int ProjectileID,int defaultcap,int maxcap)
    {
        BulletPoolsInform.Add(ProjectileID, new PoolInform(new EnemyBulletPool(Projectiles[ProjectileID], defaultcap, maxcap)));
        return BulletPoolsInform[ProjectileID].Pool;
    }
    public EnemyBulletPool UsePool(int ProjectileID)
    {
        if(BulletPoolsInform.ContainsKey(ProjectileID))
        {
            BulletPoolsInform[ProjectileID].Usage++;

        } else
        {
            CreateBulletPool(ProjectileID, 10, 100);
        }
        return BulletPoolsInform[ProjectileID].Pool;

    }
    public void UnUsePool(int ProjectileID)
    {
        if(BulletPoolsInform.ContainsKey(ProjectileID))
        {
            if (BulletPoolsInform[ProjectileID].Usage == 1)
            {
                BulletPoolsInform[ProjectileID] = null;
                BulletPoolsInform.Remove(ProjectileID);
            }
            else
            {
                BulletPoolsInform[ProjectileID].Usage--;

            }
        }
    }
    public EnemyBulletPool GetPool(int ProjectileID)
    {
        return BulletPoolsInform[ProjectileID].Pool;
    }
    private void Start()
    {
        GameInformation.instance.PjtlReserve = this;
    }
}
public class PoolInform
{
    public EnemyBulletPool Pool;
    public int Usage;
    public PoolInform(EnemyBulletPool pool)
    {
        Pool = pool;
    }
}
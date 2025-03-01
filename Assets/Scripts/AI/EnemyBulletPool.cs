using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPool
{
    private GameObject bullet;
    private ObjectPool<EnemyBullet> privateBulletPool;
    public EnemyBulletPool(GameObject bulletprefab, int defaultcapa, int maxcapa)
    {
        bullet = bulletprefab;
        privateBulletPool = new ObjectPool<EnemyBullet>(OnCreateBullet, OnGetBullet, OnPutBackBullet, OnDestroyBullet, true, defaultcapa, maxcapa);

    }
    public EnemyBullet GetBulletobject()
    {
        return privateBulletPool.Get();
    }
    private EnemyBullet OnCreateBullet()
    {
        EnemyBullet b = GameObject.Instantiate(bullet).GetComponent<EnemyBullet>();
        b.gameObject.SetActive(false);
        b.SetPool(privateBulletPool);
        return b;

    }
    private void OnGetBullet(EnemyBullet bullet)
    {
        bullet.EnableBullet();
    }
    private void OnPutBackBullet(EnemyBullet bullet)
    {
        bullet.Uninitialize();
    }
    private void OnDestroyBullet(EnemyBullet bullet)
    {
        GameObject.Destroy(bullet.gameObject);
    }
}

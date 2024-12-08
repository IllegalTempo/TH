using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool
{
    private GameObject bullet;
    private ObjectPool<bulletobject> privateBulletPool;
    public BulletPool(GameObject bulletprefab,int defaultcapa,int maxcapa)
    {

        bullet = bulletprefab;
        privateBulletPool = new ObjectPool<bulletobject>(OnCreateBullet, OnGetBullet, OnPutBackBullet, OnDestroyBullet, true, defaultcapa, maxcapa);
        
    }
    public bulletobject GetBulletobject()
    {
        return privateBulletPool.Get();
    }
    private bulletobject OnCreateBullet()
    {
        bulletobject b = GameObject.Instantiate(bullet).GetComponent<bulletobject>();
        b.gameObject.SetActive(false);
        b.SetPool(privateBulletPool);
        return b;

    }
    private void OnGetBullet(bulletobject bullet)
    {
        bullet.EnableBullet();
    }
    private void OnPutBackBullet(bulletobject bullet)
    {
        bullet.Uninitialize();
    }
    private void OnDestroyBullet(bulletobject bullet)
    {
        GameObject.Destroy(bullet.gameObject);
    }
}

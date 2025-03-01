using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class EnemyBullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float orglife;
    public float life;
    private ObjectPool<EnemyBullet> poolref;

    private void Update()
    {
        transform.Translate(BulletMovement() * speed * Time.deltaTime, Space.Self);

        if (life > 0)
        {
            life -= Time.deltaTime;
        }
        else
        {
            DestroyBullet();
        }
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        PlayerMain enemy = other.gameObject.GetComponent<PlayerMain>();
        if (enemy != null)
        {
            Debug.Log(enemy.gameObject.name);

            enemy.Damage(damage, other.ClosestPoint(transform.position));
            DestroyBullet();
        }
    }
    protected abstract Vector3 BulletMovement();
    public void Uninitialize()
    {
        gameObject.SetActive(false);
    }
    public void EnableBullet()
    {
        gameObject.SetActive(true);
        life = orglife;

    }
    private void DestroyBullet()
    {

        poolref.Release(this);
    }
    public void SetPool(ObjectPool<EnemyBullet> pool)
    {
        poolref = pool;
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneralHitDetection : MonoBehaviour
{
    public float damage;
    private PlayerMain owner;
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        
        if (enemy != null)
        {
            enemy.Damage(damage,other.ClosestPoint(transform.position),owner);
            owner.OnHit(gameObject.name);
        }
    }
    private void Start()
    {
        owner = transform.root.GetComponent<PlayerMain>();
    }
}

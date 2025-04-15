using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneralHitDetection : MonoBehaviour
{
    public float damage;
    public int OnHitSound;
    public int OnHitEffect;
    public string OnHitMethod;

    private PlayerMain owner;

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        
        if (enemy != null)
        {
            enemy.Damage(damage,other.ClosestPoint(transform.position),owner,OnHitSound,OnHitEffect);
            owner.OnHit(gameObject.name);
            if(OnHitMethod != "")
            {
                owner.CurrentWeapon.SendMessage(OnHitMethod);

            }
        }
    }
    private void Start()
    {

        owner = gameObject.transform.root.GetComponent<PlayerMain>();
        gameObject.SetActive(false);
    }
}

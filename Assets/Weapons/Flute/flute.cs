using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Pool;

public class flute : GeneralWeapon
{
    GameObject Attack1NotePrefab;

    int AttackIndex = 1;
    int charge = 0;
    BulletPool Attack1Pool;


    //Weapon Action CDs
    private float Shoot1CD = 1f;
    private float WeaponActionTimeMultiplier = 1f;
    public float WeaponActionSpeed
    {
        get
        {
            return WeaponActionTimeMultiplier;
        }
        set
        {
            WeaponActionTimeMultiplier = value;
            playeranimator.speed = 1/value;
        }
    }
    // Update is called once per frame
    void Update()
    {
        base.BaseUpdate();
        if (Input.GetMouseButtonDown((int)KeyMap.altAttack))
        {
            Aim();
        }
        if (Input.GetMouseButtonUp((int)KeyMap.altAttack))
        {
            UnAim();
        }
        //Inputs
        if (WeaponActionCD <= 0)
        {
            if (Input.GetMouseButtonDown((int)KeyMap.Attack))
            {
                Shoot();
            }
            
        }
        

    }
    public void AddCharge()
    {
        charge += 10;
    }
    private void Aim()
    {
        player.playermovement.AimDownSight();
    }
    private void UnAim()
    {
        player.playermovement.AimUpSight();

    }
    private void Shoot()
    {
        player.OnAttack();
        playeranimator.Play($"Attack{AttackIndex}");
        switch(AttackIndex)
        {
            case 1:
                Shoot1();
                break;
        }
    }
    //Followings are ran by animation:
    private Vector3 CursorExtension = new Vector3(0,0,3);
    public void Shoot1()
    {
        WeaponActionCD += Shoot1CD * WeaponActionTimeMultiplier;
        Invoke("Shoot1Action",(0.47f/0.6f) * WeaponActionTimeMultiplier);
        
    }
    private void Shoot1Action()
    {
        bulletobject b = Attack1Pool.GetBulletobject();
        if (player.playermovement.aiming)
        {

            b.transform.position = Camera.main.transform.position + Camera.main.transform.rotation * CursorExtension;
            b.transform.rotation = Camera.main.transform.rotation;
        }
        else
        {
            b.transform.position = player.transform.position;
            b.transform.rotation = player.transform.rotation;

        }
    }

    
    private void Start()
    {
        base.preStart();
        WeaponActionSpeed = 0.8f;
        Attack1NotePrefab = Resources.Load<GameObject>("weapon/flute/bullets/Attack1");
        Attack1Pool = new BulletPool(Attack1NotePrefab,50,200);
    }
}

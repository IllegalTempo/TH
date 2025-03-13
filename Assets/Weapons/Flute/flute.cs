using Steamworks.ServerList;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class flute : GeneralWeapon
{
    GameObject Attack1NotePrefab;

    int AttackIndex = 1;
    int charge = 0;
    BulletPool Attack1Pool;
    

    //Weapon Action CDs
    private float Shoot1CD = 1f;
    private float BeatCD = 1f;
    private float Spin_BeatCD = 1f;
    public Vector3 UpwardForce_Spin_Beat = new Vector3(0, 30, 0);
    private float WeaponActionTimeMultiplier = 1f;
    private Dictionary<string, object> GetAnimationCDvar;


    //[Header("Attack Colliders")]
    //public Collider Attack1_Weapon_Collider;
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
    private void InitializeAbilityCD()
    {
        GetAnimationCDvar = new Dictionary<string, object>()
        {
            {"Attack1",Shoot1CD},
            {"Attack1_Weapon",BeatCD},
            {"Attack2_Weapon",Spin_BeatCD},


        };
        foreach(AnimationClip c in playeranimator.runtimeAnimatorController.animationClips)
        {
            if (!GetAnimationCDvar.ContainsKey(c.name))
            {

            } else
            {


                GetAnimationCDvar[c.name] = c.length;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        base.BaseUpdate();

        if (!player.IsLocal) return;
        //Inputs
        if (WeaponActionCD <= 0)
        {
            if(Weaponized)
            {
                if (Input.GetMouseButton((int)KeyMap.Attack))
                {
                    if(Input.GetMouseButton((int)KeyMap.altAttack))
                    {
                        Spin_Beat();

                    } else
                    {
                        Beat();

                    }
                }

            } else 
            {

                if (Input.GetMouseButton((int)KeyMap.Attack))
                {
                    Shoot();

                }
            }
            
               
                
            
            
        }
        

    }
    public void AddCharge()
    {
        charge += 10;
    }
    private void Shoot()
    {
        base.OnAttack_Network(0);

        player.OnAttack();
        playeranimator.Play($"Attack{AttackIndex}");
        switch(AttackIndex)
        {
            case 1:
                Shoot1();
                break;
        }
    }


    /// <summary>
    /// Real Attacks ==================================
    /// </summary>
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
    private void Beat()
    {
        base.OnAttack_Network(1);

        player.OnAttack();
        playeranimator.Play("Attack1_Weapon");
        WeaponActionCD = BeatCD * WeaponActionTimeMultiplier;
    }
    private void Spin_Beat()
    {
        base.OnAttack_Network(2);

        player.OnAttack();
        player.rb.AddForce(UpwardForce_Spin_Beat,ForceMode.VelocityChange);
        playeranimator.Play("Attack2_Weapon");
        WeaponActionCD = Spin_BeatCD * WeaponActionTimeMultiplier;
    }
    /// <summary>
    /// END
    /// </summary>





    //Followings are ran by animation:
    private Vector3 CursorExtension = new Vector3(0,0,3);
    public void Shoot1()
    {
        WeaponActionCD = Shoot1CD * WeaponActionTimeMultiplier;
        Invoke("Shoot1Action",(0.47f/0.6f) * WeaponActionTimeMultiplier);
        
    }
    

    
    private void Start()
    {
        base.GetAttackAction = new AttackAction[]{Shoot,Beat,Spin_Beat };
        WeaponTypeID = (int)GameInformation.WeaponType.flute;
        base.preStart();
        InitializeAbilityCD();
        WeaponActionSpeed = 0.8f;
        Attack1NotePrefab = Resources.Load<GameObject>("weapon/flute/bullets/Attack1");
        Attack1Pool = new BulletPool(Attack1NotePrefab,50,200,player);
    }
}

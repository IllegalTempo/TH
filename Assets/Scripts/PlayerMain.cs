using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMain : MonoBehaviour
{
    private float health;

    public GameObject CurrentWeapon;
    public GameObject CurrentWeaponCollider;

    public int CurrentWeaponID;
    public Animator animator;
    public Movement playermovement;
    public PlayerInventory inventory;
    public GameObject soul;
    public Transform hand;
    public Transform HighLightObject;
    private RaycastHit rch;
    public Rigidbody rb;
    public ParticleSystem OnHitEffect;
    private void Update()
    {
        if(HighLightObject != null)
        {
            HighLightObject.gameObject.GetComponent<Outline>().enabled = false;
            HighLightObject = null;
        }
        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out rch,Mathf.Infinity))
        {
            if (rch.transform.gameObject.GetComponent<Outline>() != null)
            {
                HighLightObject = rch.transform;
                HighLightObject.gameObject.GetComponent<Outline>().enabled = true;

            }





        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameInformation.instance.LocalPlayer = gameObject;
        rb = GetComponent<Rigidbody>();
        playermovement = GetComponent<Movement>();
        ChooseWeapon((int)GameInformation.Weapon.HAKUREI_FLUTE);

        gameObject.SetActive(false);
    }
    public void Damage(float damage)
    {
        health -= damage;
        OnDamage();
    }
    public void Heal(float health)
    {
       this.health += health;
        OnHeal();
    }
    public void SelectWeapon(int weaponid)
    {

    
    }
    private void OnDamage()
    {


    }
    private void OnHeal()
    {

    }
    public void OnAttack()
    {

    }
    public void OnHit(string hitinform)
    {
        
    }
    public void ChooseWeapon(int WeaponID)
    {
        int type = GameInformation.instance.WeaponID2TypeID[WeaponID];
        if (CurrentWeapon != null) { Destroy(CurrentWeapon);Destroy(CurrentWeaponCollider); }
        CurrentWeapon = Instantiate(Resources.Load<GameObject>(GameInformation.instance.WeaponPrefabPath[WeaponID]),hand);
        CurrentWeaponCollider = Instantiate(Resources.Load<GameObject>(GameInformation.instance.WeaponPath[type] + "HitDetect"), transform);

        CurrentWeapon.name = CurrentWeapon.name.Replace("(Clone)", "").Trim();
        CurrentWeaponCollider.name = CurrentWeaponCollider.name.Replace("(Clone)", "").Trim();

        animator.Rebind();
        CurrentWeaponID = WeaponID;
    }
}

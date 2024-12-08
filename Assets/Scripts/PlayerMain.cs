using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private float health;

    public GameObject CurrentWeapon;
    public int CurrentWeaponID;
    public Animator animator;
    public Movement playermovement;
    public PlayerInventory inventory;
    private void Start()
    {
        playermovement = GetComponent<Movement>();
        GameInformation.LocalPlayer = gameObject;
        ChooseWeapon((int)GameInformation.Weapon.HAKUREI_FLUTE);
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
    public void ChooseWeapon(int WeaponID)
    {
        if (CurrentWeapon != null) { Destroy(CurrentWeapon); }
        CurrentWeapon = Instantiate(Resources.Load<GameObject>(GameInformation.WeaponPrefabPath[WeaponID]),transform);
        CurrentWeapon.name = CurrentWeapon.name.Replace("(Clone)", "").Trim();
        animator.Rebind();
        CurrentWeaponID = WeaponID;
    }
}

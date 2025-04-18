using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Pool;

public class GeneralWeapon : MonoBehaviour
{
    public PlayerMain player;
    public bool Weaponized;
    protected Animator playeranimator;
    public float WeaponActionCD = 0f;
    public int WeaponTypeID;

    protected void preStart()
    {
        player = transform.root.GetComponent<PlayerMain>();
        playeranimator = player.animator;

        playeranimator.runtimeAnimatorController = Resources.Load<AnimatorController>(GameInformation.instance.WeaponPath[WeaponTypeID] + "PlayerAnimator");

    }
    protected void BaseUpdate()
    {
        
        if (WeaponActionCD > 0)
        {
            WeaponActionCD -= Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyMap.SwitchWeaponized))
        {
            Weaponized = !Weaponized;
            playeranimator.SetBool("Weaponing",Weaponized);
        }
    }
    // Update is called once per frame
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Pool;

public class GeneralWeapon : MonoBehaviour
{
    public PlayerMain player;
    protected Animator playeranimator;
    public float WeaponActionCD = 0f;
    protected void preStart()
    {
        player = transform.parent.GetComponent<PlayerMain>();
        playeranimator = player.animator;

        playeranimator.runtimeAnimatorController = Resources.Load<AnimatorController>(GameInformation.WeaponPlayerAnimatorPath[(int)GameInformation.WeaponType.flute]);

    }
    protected void BaseUpdate()
    {
        if(WeaponActionCD > 0)
        {
            WeaponActionCD -= Time.deltaTime;
        }
    }
    // Update is called once per frame
    
}

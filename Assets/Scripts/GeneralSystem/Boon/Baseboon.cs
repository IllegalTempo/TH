using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Baseboon : MonoBehaviour
{
    protected PlayerMain owner;
    public BoonInform inform;

    private void OnEnable()
    {
        owner = GetComponent<PlayerMain>();
    }
    
    
    public abstract void OnAttack();
    public abstract void OnDamaged();
    public abstract void OnKill();


}

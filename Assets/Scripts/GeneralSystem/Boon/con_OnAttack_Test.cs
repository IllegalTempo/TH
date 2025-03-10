using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class con_OnAttack_Test : Baseboon
{
    public override void OnAttack()
    {
        Debug.Log("OnAttack_Boon_Test!");
        
    }

    public override void OnDamaged()
    {
        throw new NotImplementedException();
    }

    public override void OnKill()
    {
        throw new NotImplementedException();
    }

}


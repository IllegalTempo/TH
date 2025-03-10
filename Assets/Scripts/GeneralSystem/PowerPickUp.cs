using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPickUp : Pickable
{
    public override void OnPickUp()
    {
        GameInformation.instance.LocalPlayer.GetComponent<PlayerMain>().OnPickUpPower();    
    }
}

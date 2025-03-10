using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPickUp : Pickable
{
    public override void OnPickUp()
    {
        GameInformation.instance.LocalPlayer.GetComponent<PlayerMain>().OnPickUpPoint();
    }
}

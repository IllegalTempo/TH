using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HakureiShrineStairInteraction : InteractableCollider
{
    protected override void OnInteract()
    {
            GameSystem.instance.LoadSceneAction("InGame", false);

    }
}

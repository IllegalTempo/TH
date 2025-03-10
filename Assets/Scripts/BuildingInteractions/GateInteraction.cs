using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateInteraction : InteractableCollider
{
    protected override void OnInteract()
    {
        GameSystem.instance.LoadSceneAction("InBattle",false);
    }
}

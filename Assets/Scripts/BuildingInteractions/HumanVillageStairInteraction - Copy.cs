using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanVillageInteraction : InteractableCollider
{
    protected override void OnInteract()
    {
        
            GameSystem.instance.LoadSceneAction("HumanVillage", false);

        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateInteraction : InteractableCollider
{
    protected override void OnInteract()
    {
        if(GameInformation.instance.MainNetwork.IsServer)
        {
            GameSystem.instance.LoadSceneAction("InBattle", false);

        } else
        {
            GameUIManager.instance.NewMessage("You are not the owner of the world!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRoomInteract : InteractableCollider
{
    protected override void OnInteract()
    {
        GameUIManager.instance.StartChoosingNote();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableCollider : MonoBehaviour
{
    private bool inRange = false;
    private void Update()
    {
        if(Input.GetKeyDown(KeyMap.Interact) && inRange)
        {
            OnInteract();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameInformation.instance.LocalPlayer)
        {
            inRange = true;
            GameUIManager.instance.StartInteraction();
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == GameInformation.instance.LocalPlayer)
        {
            inRange = false;
            //
            GameUIManager.instance.EndInteractionTips();
        }
    }
    private void OnDestroy()
    {
        GameUIManager.instance.EndInteractionTips(); 
    }
    protected abstract void OnInteract();

}

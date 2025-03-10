using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public abstract void OnPickUp();
    private float ItemPickUpRange;
    private void OnEnable()
    {
        ItemPickUpRange = GameInformation.instance.ItemPickUpRange;
    }
    public void Update()
    {
        Vector3 localplayerpos = GameInformation.instance.LocalPlayer.transform.position;
        float distance = Vector3.Distance(transform.position, localplayerpos);
        transform.position = Vector3.MoveTowards(transform.position, localplayerpos, Time.deltaTime * 50f / distance);
        if (distance < ItemPickUpRange)
        {
            GotPickUp();
        }

    }
    public void GotPickUp()
    {
        OnPickUp();
        Destroy(gameObject);
    }

}

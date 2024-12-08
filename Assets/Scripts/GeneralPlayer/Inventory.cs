using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //This class Controls InventoryUI! if you want to find real storage inventory class go PlayerInventory.cs
    [SerializeField]
    private GameObject InventoryObject;
    [SerializeField]
    private GameObject soul;
    private bool opening = false;
    private Animator animator;

    private Movement playerMovement;
    private void OnEnable()
    {
        playerMovement = GameObject.Find("Player").GetComponent<Movement>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyMap.ToggleInventory))
        {
            if(opening)
            {
                OnCloseInventory();
            } else
            {
                OnOpenInventory();
            }
        }
    }
    private void OnOpenInventory()
    {
        InventoryObject.SetActive(true);
        soul.SetActive(true);
        opening = true;
        playerMovement.OpenInventory();

    }
    private void OnCloseInventory()
    {
        soul.SetActive(false);
        InventoryObject.SetActive(false);

        opening = false;
        playerMovement.CloseInventory();

    }
    private void DisableInventoryObject()
    {
        InventoryObject.SetActive(false);

    }
}

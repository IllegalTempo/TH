using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //This class Controls InventoryUI! if you want to find real storage inventory class go PlayerInventory.cs
    [SerializeField]
    private GameObject InventoryObject;
    public GameObject soul;
    private bool opening = false;
    public void Initialize(Movement mov,GameObject soul)
    {
        playerMovement = mov;
        this.soul = soul;
    }
    private Movement playerMovement;
    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyMap.ToggleInventory))
        {
            if (opening)
            {
                OnCloseInventory();
            }
            else
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

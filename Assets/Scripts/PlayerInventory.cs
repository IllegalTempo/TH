using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Item[] equiped;
    public List<Item> inv;
    public PlayerInventory()
    {
        equiped = new Item[5];
        inv = new List<Item>();
    }
    public PlayerInventory(Item[] items,List<Item> inv)
    {
        equiped = items;
        this.inv = inv;
    }
}

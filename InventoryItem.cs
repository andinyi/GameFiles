using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public ItemData data {get; private set; }
    public int stackSize {get; private set;}

    public InventoryItem(ItemData input) {
        data = input; 
        stack();
    }

    public void stack() {
        stackSize++;
    }

    public void unstack() {
        stackSize--;
    }
}

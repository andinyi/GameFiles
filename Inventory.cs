using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public static Inventory current;
    [SerializeField]
    private Dictionary<ItemData, InventoryItem> dictMap;
    [SerializeField]
    public List<InventoryItem> inventory {get; private set;}
    public int latest;
    public ItemData latestData;

    private void Awake() {
        current = this;
        inventory = new List<InventoryItem>();
        dictMap = new Dictionary<ItemData, InventoryItem>();
    }

    public void Add(ItemData addIn) {
        if(inventory.Count >= 9) {
            Debug.Log("Inventory Full");
            return;
        }
        if(dictMap.TryGetValue(addIn, out InventoryItem item)) {
            item.stack();
            InventoryStackTrigger(item.stackSize, addIn);
        }
        else 
        {
            InventoryItem newItem = new InventoryItem(addIn);
            inventory.Add(newItem);
            dictMap.Add(addIn, newItem);
            InventoryEventsTrigger();
        }
        Debug.Log("Sucessfully Added");
    }

    public void Remove(ItemData takeOut) {
        if(dictMap.TryGetValue(takeOut, out InventoryItem item)) {
            item.unstack();
        }
        Debug.Log("Sucessfully Removed");
        InventoryEventsTrigger();
    }

    public event System.Action InventoryEvents;
    
    public void InventoryEventsTrigger() {
        if(InventoryEvents != null)
        {
            InventoryEvents();
        }
    }

    public event System.Action InventoryStackEvent;
    public void InventoryStackTrigger(int num, ItemData data) {
        if(InventoryStackEvent != null) {
            InventoryStackEvent();
        }
        latest = num;
        latestData = data;
    }

}

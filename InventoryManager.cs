using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{   
    public GameObject Slot;
    public void Start() {
        Inventory.current.InventoryEvents += InventoryAddedItem;
        Inventory.current.InventoryStackEvent += InventoryStacks;
    }

    private void InventoryAddedItem() {
        Transform inventoryPanel = transform.Find("InventoryHUD");
        
        foreach(Transform slot in inventoryPanel)
        {
            Image image = slot.GetChild(0).GetChild(0).GetComponent<Image>();

            if(!image.enabled) {
                Debug.Log("GETS TO !IMAGE ENABLED LETS GO");
                image.enabled = true;
                image.sprite = Inventory.current.inventory[Inventory.current.inventory.Count - 1].data.icon;

                break;
            }
        }
    }

    private void InventoryStacks() {
        Transform inventoryPanel = transform.Find("InventoryHUD");
        if(Inventory.current.latest > 0) {
            Debug.Log("Stacksize > 1");
            foreach(Transform slot in inventoryPanel)
            {
                Image image = slot.GetChild(0).GetChild(0).GetComponent<Image>();
                if(image.sprite == Inventory.current.latestData.icon) {
                    Debug.Log("You can compare sprites");
                }
            }
        }
    }

    
    /*private void InventoryAddedItem() {
        foreach(InventoryItem item in Inventory.current.inventory) 
        {
            AddInventorySlots(item);
        }
    }

    private void AddInventorySlots(InventoryItem item) {
        GameObject obj = Instantiate(Slot);
        obj.transform.SetParent(transform, false);

        SlotScript slotcode = obj.GetComponent<SlotScript>();
        slotcode.Set(item);
    }*/
}

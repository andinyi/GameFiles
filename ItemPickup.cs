using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData referenceItem;

    public void handlePickup() {
        Inventory.current.Add(referenceItem);
        Destroy(gameObject);
    }
}

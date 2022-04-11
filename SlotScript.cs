using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    public Image sprite;
    public Text txt; 
    public GameObject tmp;
    public TextMesh stackLabel;

    public void Set(InventoryItem item) {
        sprite.sprite = item.data.icon;
        txt.text = item.data.name;
        if(item.stackSize <= 1) {
            tmp.SetActive(false);
            return; 
        }
        //stackLabel.text = item.stackSize.ToString();
    }
}

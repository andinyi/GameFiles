using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject var;
    
    public void openMessage() {
        var.SetActive(true);
    }
    public void closeMessage() {
        var.SetActive(false);
    }
}

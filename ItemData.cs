using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemData")]
public class ItemData : ScriptableObject {
    public GameObject prefab;
    public string description;
    public Sprite icon;
    public string displayname;
    public string id;
}

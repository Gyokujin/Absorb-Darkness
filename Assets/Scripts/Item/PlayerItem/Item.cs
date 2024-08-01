using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Info")]
    public GameObject modelPrefab;
    public Sprite itemIcon;
    public int itemCount;
    public string itemName;
    public string itemInfo;
}
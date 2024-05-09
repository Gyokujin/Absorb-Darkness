using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Info")]
    public GameObject modelPrefab;
    public Sprite itemIcon;
    public string itemName;
}
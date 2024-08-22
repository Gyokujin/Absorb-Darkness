using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Using Item", menuName = "Items/Using Item")]
public class UsingItem : Item
{
    public enum UsingItemType
    {
        EtherFlask
    }

    public UsingItemType usingItemType;
    public string usingAnimation;
}
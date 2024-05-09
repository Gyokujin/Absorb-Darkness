using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Using Item", menuName = "Items/Using Item")]
public class UsingItem : Item
{
    [Header("Action Animations")]
    public string usingAnimation;
}
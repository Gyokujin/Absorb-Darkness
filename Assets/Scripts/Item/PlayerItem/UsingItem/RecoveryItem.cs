using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecoveryItem", menuName = "Items/Using Item/RecoveryItem")]
public class RecoveryItem : UsingItem
{
    public int hpRecoveryAmount;
    public int mpRecoveryAmount;
}
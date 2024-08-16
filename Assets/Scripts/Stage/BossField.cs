using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossField", menuName = "FieldInfo/BossField")]
public class BossField : FieldInfo
{
    public string bossName;
    public AudioClip bossBgm;
    public Item[] dropItems;
}
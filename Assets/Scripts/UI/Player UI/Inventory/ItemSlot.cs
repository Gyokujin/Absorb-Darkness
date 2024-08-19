using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public enum SlotType
    {
        WeaponSlot, UsingItemSlot, InteractItemSlot
    }

    public SlotType slotType;

    [SerializeField]
    private Image slotIcon;
    [SerializeField]
    private Text slotCount;
    [SerializeField]
    private Text slotName;
    [SerializeField]
    private Text slotInfo;
    public int slotIndex;

    public void ItemSlotUpdate(bool isWeapon, Sprite icon, int count, string name, string info, int index)
    {
        slotIcon.sprite = icon;
        slotCount.text = isWeapon ? null : count.ToString();
        slotName.text = name;
        slotInfo.text = info;
        slotIndex = index;
    }
}
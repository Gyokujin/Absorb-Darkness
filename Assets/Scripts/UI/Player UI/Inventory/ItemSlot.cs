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
    private Image slotItemIcon;
    [SerializeField]
    private Text slotItemCount;
    [SerializeField]
    private Text slotItemName;
    [SerializeField]
    private Text slotItemInfo;
    public int slotIndex;

    public void ItemSlotUpdate(bool isWeapon, Sprite icon, int count, string name, string info, int index)
    {
        slotItemIcon.sprite = icon;
        slotItemCount.text = isWeapon ? null : count.ToString();
        slotItemName.text = name;
        slotItemInfo.text = info;
        slotIndex = index;
    }
}
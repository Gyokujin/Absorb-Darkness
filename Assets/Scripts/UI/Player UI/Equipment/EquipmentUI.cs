using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    [Header("Slot Selected")]
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;

    [Header("Equipt Slot")]
    [SerializeField]
    private HandEquipmentSlotUI[] handEquipmentSlotUI;

    public void OpenEquipmentsUI()
    {
        LoadWeaponsOnEquipmentScreen(UIManager.instance.playerInventory);
    }

    public void ResetAllSelectedSlots()
    {
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
    }

    public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
    {
        for (int i = 0; i < handEquipmentSlotUI.Length; i++)
        {
            if (handEquipmentSlotUI[i].leftHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftSlots[0]);
            }
            else if (handEquipmentSlotUI[i].leftHandSlot02)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftSlots[1]);
            }
            else if (handEquipmentSlotUI[i].rightHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightSlots[0]);
            }
            else
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightSlots[1]);
            }
        }
    }
}
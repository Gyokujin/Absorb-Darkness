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
        ResetAllSelectedSlots();
        PlayerInventory playerInventory = UIManager.instance.playerInventory;

        for (int i = 0; i < handEquipmentSlotUI.Length; i++)
        {
            if (handEquipmentSlotUI[i].leftHandSlot01)
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftSlots[0]);
            else if (handEquipmentSlotUI[i].leftHandSlot02)
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftSlots[1]);
            else if (handEquipmentSlotUI[i].rightHandSlot01)
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightSlots[0]);
            else
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightSlots[1]);
        }
    }

    public void ResetAllSelectedSlots()
    {
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
    }

    public void SelectWeapon(HandEquipmentSlotUI selectSlot)
    {
        ResetAllSelectedSlots();

        if (selectSlot.leftHandSlot01)
            leftHandSlot01Selected = true;
        else if (selectSlot.leftHandSlot02)
            leftHandSlot02Selected = true;
        else if (selectSlot.rightHandSlot01)
            rightHandSlot01Selected = true;
        else
            rightHandSlot02Selected = true;

        UIManager.instance.inventoryUI.gameObject.SetActive(true);
        UIManager.instance.inventoryUI.OpenWeaponInventory();
    }

    public void EquipWeapon(WeaponItem weaponItem)
    {
        PlayerInventory playerInventory = UIManager.instance.playerInventory;
        WeaponItem curWeapon;

        if (leftHandSlot01Selected)
        {
            curWeapon = playerInventory.weaponInLeftSlots[0];
            playerInventory.weaponInLeftSlots[0] = weaponItem;
        }
        else if (UIManager.instance.equipmentUI.leftHandSlot02Selected)
        {
            curWeapon = playerInventory.weaponInLeftSlots[1];
            playerInventory.weaponInLeftSlots[1] = weaponItem;
        }
        else if (UIManager.instance.equipmentUI.rightHandSlot01Selected)
        {
            curWeapon = playerInventory.weaponInRightSlots[0];
            playerInventory.weaponInRightSlots[0] = weaponItem;
        }
        else if (UIManager.instance.equipmentUI.rightHandSlot02Selected)
        {
            curWeapon = playerInventory.weaponInRightSlots[1];
            playerInventory.weaponInRightSlots[1] = weaponItem;
        }
        else
            return;

        playerInventory.weaponItems.Remove(weaponItem);
        playerInventory.weaponItems.Add(curWeapon);

        if (playerInventory.curLeftWeaponIndex >= 0)
            playerInventory.curLeftWeapon = playerInventory.weaponInLeftSlots[playerInventory.curLeftWeaponIndex];

        if (playerInventory.curRightWeaponIndex >= 0)
            playerInventory.curRightWeapon = playerInventory.weaponInRightSlots[playerInventory.curRightWeaponIndex];

        playerInventory.LoadWeaponSlot(playerInventory.curLeftWeapon, true);
        playerInventory.LoadWeaponSlot(playerInventory.curRightWeapon, false);

        OpenEquipmentsUI();
        UIManager.instance.inventoryUI.UpdateWeaponInventory();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }
}
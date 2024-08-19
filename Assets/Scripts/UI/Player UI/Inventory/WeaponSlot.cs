using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : ItemSlot
{
    public void SelectWeaponSlot()
    {
        PlayerInventory playerInventory = UIManager.instance.playerInventory;
        WeaponItem weaponItem = playerInventory.weaponItems[slotIndex];
        UIManager.instance.equipmentUI.EquipWeapon(weaponItem);
    }
}
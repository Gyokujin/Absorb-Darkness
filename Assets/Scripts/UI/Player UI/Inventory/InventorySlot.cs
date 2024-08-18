using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Image icon;

    [Header("Component")]
    [SerializeField]
    private EquipmentWindowUI equipmentWindow;
    private WeaponItem item;
    private PlayerInventory playerInventory;
    
    void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void AddItem(WeaponItem newItem)
    {
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void EquipItem()
    {
        if (UIManager.instance.leftHandSlot01Selected)
        {
            playerInventory.equipmentWeapons.Add(playerInventory.weaponInLeftSlots[0]);
            playerInventory.weaponInLeftSlots[0] = item;
            playerInventory.equipmentWeapons.Remove(item);
        }
        else if (UIManager.instance.leftHandSlot02Selected)
        {
            playerInventory.equipmentWeapons.Add(playerInventory.weaponInLeftSlots[1]);
            playerInventory.weaponInLeftSlots[1] = item;
            playerInventory.equipmentWeapons.Remove(item);
        }
        else if (UIManager.instance.rightHandSlot01Selected)
        {
            playerInventory.equipmentWeapons.Add(playerInventory.weaponInRightSlots[0]);
            playerInventory.weaponInRightSlots[0] = item;
            playerInventory.equipmentWeapons.Remove(item);
        }
        else if (UIManager.instance.rightHandSlot02Selected)
        {
            playerInventory.equipmentWeapons.Add(playerInventory.weaponInRightSlots[1]);
            playerInventory.weaponInRightSlots[1] = item;
            playerInventory.equipmentWeapons.Remove(item);
        }
        else
        {
            return;
        }

        if (playerInventory.curLeftWeaponIndex >= 0)
        {
            playerInventory.curLeftWeapon = playerInventory.weaponInLeftSlots[playerInventory.curLeftWeaponIndex];
        }

        if (playerInventory.curRightWeaponIndex >= 0)
        {
            playerInventory.curRightWeapon = playerInventory.weaponInRightSlots[playerInventory.curRightWeaponIndex];
        }

        playerInventory.LoadWeaponSlot(playerInventory.curLeftWeapon, true);
        playerInventory.LoadWeaponSlot(playerInventory.curRightWeapon, false);
        equipmentWindow.LoadWeaponsOnEquipmentScreen(playerInventory);

        UIManager.instance.ResetAllSelectedSlots();
        UIManager.instance.InventoryUIUpdate();
        // UIManager.instance.inventoryManager.gameObject.SetActive(false);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }
}
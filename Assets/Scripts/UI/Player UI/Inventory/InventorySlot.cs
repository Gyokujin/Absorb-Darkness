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
    private PlayerWeaponSlotManager weaponSlotManager;
    
    void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        weaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();
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
            playerInventory.weaponsInventory.Add(playerInventory.weaponInLeftSlots[0]);
            playerInventory.weaponInLeftSlots[0] = item;
            playerInventory.weaponsInventory.Remove(item);
        }
        else if (UIManager.instance.leftHandSlot02Selected)
        {
            playerInventory.weaponsInventory.Add(playerInventory.weaponInLeftSlots[1]);
            playerInventory.weaponInLeftSlots[1] = item;
            playerInventory.weaponsInventory.Remove(item);
        }
        else if (UIManager.instance.rightHandSlot01Selected)
        {
            playerInventory.weaponsInventory.Add(playerInventory.weaponInRightSlots[0]);
            playerInventory.weaponInRightSlots[0] = item;
            playerInventory.weaponsInventory.Remove(item);
        }
        else if (UIManager.instance.rightHandSlot02Selected)
        {
            playerInventory.weaponsInventory.Add(playerInventory.weaponInRightSlots[1]);
            playerInventory.weaponInRightSlots[1] = item;
            playerInventory.weaponsInventory.Remove(item);
        }
        else
        {
            return;
        }

        if (playerInventory.currentLeftWeaponIndex >= 0)
        {
            playerInventory.leftWeapon = playerInventory.weaponInLeftSlots[playerInventory.currentLeftWeaponIndex];
        }

        if (playerInventory.currentRightWeaponIndex >= 0)
        {
            playerInventory.rightWeapon = playerInventory.weaponInRightSlots[playerInventory.currentRightWeaponIndex];
        }

        weaponSlotManager.LoadWeaponSlot(playerInventory.leftWeapon, true);
        weaponSlotManager.LoadWeaponSlot(playerInventory.rightWeapon, false);
        equipmentWindow.LoadWeaponsOnEquipmentScreen(playerInventory);

        UIManager.instance.ResetAllSelectedSlots();
        UIManager.instance.InventoryUIUpdate();
        UIManager.instance.inventoryManager.gameObject.SetActive(false);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Click]);
    }
}
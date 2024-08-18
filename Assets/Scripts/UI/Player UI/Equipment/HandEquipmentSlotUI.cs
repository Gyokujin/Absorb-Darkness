using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandEquipmentSlotUI : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField]
    private Image icon;
    private WeaponItem weapon;

    [Header("Weapon Slot")]
    public bool leftHandSlot01;
    public bool leftHandSlot02;
    public bool rightHandSlot01;
    public bool rightHandSlot02;

    public void AddItem(WeaponItem newWeapon)
    {
        weapon = newWeapon;
        icon.sprite = weapon.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearItem()
    {
        weapon = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void SelectSlot()
    {
        if (leftHandSlot01)
        {
            UIManager.instance.equipmentUI.leftHandSlot01Selected = true;
        }
        else if (leftHandSlot02)
        {
            UIManager.instance.equipmentUI.leftHandSlot02Selected = true;
        }
        else if (rightHandSlot01)
        {
            UIManager.instance.equipmentUI.rightHandSlot01Selected = true;
        }
        else
        {
            UIManager.instance.equipmentUI.rightHandSlot02Selected = true;
        }

        // UIManager.instance.inventoryManager.gameObject.SetActive(true);
        UIManager.instance.equipmentUI.gameObject.SetActive(false);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }
}
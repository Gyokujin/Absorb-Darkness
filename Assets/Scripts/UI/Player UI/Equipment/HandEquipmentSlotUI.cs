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

    public void WeaponSlotClick()
    {
        UIManager.instance.equipmentUI.SelectWeapon(this);
    }
}
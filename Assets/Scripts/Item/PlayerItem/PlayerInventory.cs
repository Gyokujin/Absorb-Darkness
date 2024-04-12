using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Weapon")]
    public WeaponItem leftWeapon;
    public WeaponItem rightWeapon;
    public WeaponItem unarmedWeapon;

    [Header("Weapon Slot")]
    public WeaponItem[] weaponInLeftSlots = new WeaponItem[1];
    public WeaponItem[] weaponInRightSlots = new WeaponItem[1];
    public int currentLeftWeaponIndex = 0;
    public int currentRightWeaponIndex = 0;
    private WeaponSlotManager weaponSlotManager;

    [Header("Inventory")]
    public List<WeaponItem> weaponsInventory;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    void Start()
    {
        leftWeapon = weaponInLeftSlots[0];
        rightWeapon = weaponInRightSlots[0];
        weaponSlotManager.LoadWeaponSlot(leftWeapon, true);
        weaponSlotManager.LoadWeaponSlot(rightWeapon, false);
    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex++;

        if (currentLeftWeaponIndex > weaponInLeftSlots.Length - 1) // index�� ���� ������ �ʰ��� ���
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponSlot(unarmedWeapon, true);
        }
        else if (weaponInLeftSlots[currentLeftWeaponIndex] != null) // ������ ���Ⱑ �ִ� ���
        {
            leftWeapon = weaponInLeftSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponSlot(weaponInLeftSlots[currentLeftWeaponIndex], true);
        }
        else
        {
            currentLeftWeaponIndex++;
        }
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex++;

        if (currentRightWeaponIndex > weaponInRightSlots.Length - 1) // index�� ���� ������ �ʰ��� ���
        {
            currentRightWeaponIndex = -1;
            rightWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponSlot(unarmedWeapon, false);
        }
        else if (weaponInRightSlots[currentRightWeaponIndex] != null) // ������ ���Ⱑ �ִ� ���
        {
            rightWeapon = weaponInRightSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponSlot(weaponInRightSlots[currentRightWeaponIndex], false);
        }
        else
        {
            currentRightWeaponIndex++;
        }
    }
}
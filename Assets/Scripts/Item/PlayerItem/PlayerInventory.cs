using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Weapon")]
    public WeaponItem leftWeapon;
    public WeaponItem rightWeapon;
    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponInLeftSlots = new WeaponItem[1];
    public WeaponItem[] weaponInRightSlots = new WeaponItem[1];

    private int currentLeftWeaponIndex = -1;
    private int currentRightWeaponIndex = -1;

    [Header("Inventory")]
    public List<WeaponItem> weaponsInventory;

    [Header("Component")]
    private WeaponSlotManager weaponSlotManager;

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
        leftWeapon = unarmedWeapon;
        rightWeapon = unarmedWeapon;
    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex++;

        if (currentLeftWeaponIndex > weaponInLeftSlots.Length - 1) // index가 슬롯 범위를 초과할 경우
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponSlot(unarmedWeapon, true);
        }
        else if (weaponInLeftSlots[currentLeftWeaponIndex] != null) // 장착할 무기가 있는 경우
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

        if (currentRightWeaponIndex > weaponInRightSlots.Length - 1) // index가 슬롯 범위를 초과할 경우
        {
            currentRightWeaponIndex = -1;
            rightWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponSlot(unarmedWeapon, false);
        }
        else if (weaponInRightSlots[currentRightWeaponIndex] != null) // 장착할 무기가 있는 경우
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
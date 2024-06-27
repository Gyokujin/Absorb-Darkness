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
    private PlayerItemSlotManager itemSlotManager;

    [Header("Using Item")]
    public UsingItem curUsingItem;
    public UsingItem[] usingItemSlots = new UsingItem[1];
    public int estusCount = 3;

    [Header("Inventory")]
    public List<WeaponItem> weaponsInventory;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        itemSlotManager = GetComponentInChildren<PlayerItemSlotManager>();
    }

    void Start()
    {
        leftWeapon = weaponInLeftSlots[0];
        rightWeapon = weaponInRightSlots[0];
        itemSlotManager.LoadWeaponSlot(leftWeapon, true);
        itemSlotManager.LoadWeaponSlot(rightWeapon, false);

        curUsingItem = usingItemSlots[0];
        itemSlotManager.LoadUsingItemSlot(curUsingItem);
    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex++;

        if (currentLeftWeaponIndex > weaponInLeftSlots.Length - 1) // index가 슬롯 범위를 초과할 경우
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            itemSlotManager.LoadWeaponSlot(unarmedWeapon, true);
        }
        else if (weaponInLeftSlots[currentLeftWeaponIndex] != null) // 장착할 무기가 있는 경우
        {
            leftWeapon = weaponInLeftSlots[currentLeftWeaponIndex];
            itemSlotManager.LoadWeaponSlot(weaponInLeftSlots[currentLeftWeaponIndex], true);
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
            itemSlotManager.LoadWeaponSlot(unarmedWeapon, false);
        }
        else if (weaponInRightSlots[currentRightWeaponIndex] != null) // 장착할 무기가 있는 경우
        {
            rightWeapon = weaponInRightSlots[currentRightWeaponIndex];
            itemSlotManager.LoadWeaponSlot(weaponInRightSlots[currentRightWeaponIndex], false);
        }
        else
        {
            currentRightWeaponIndex++;
        }
    }
}
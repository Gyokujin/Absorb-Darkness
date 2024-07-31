using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class PlayerInventory : MonoBehaviour
{
    // private InitItemData initItemData; // 이후에 데이터 관리가 필요하면 쓰자

    [Header("Using Inventory")]
    public UsingItem curUsingItem;

    [Header("Weapon")]
    public WeaponItem leftWeapon;
    public WeaponItem rightWeapon;
    public WeaponItem unarmedWeapon;

    [Header("Weapon Inventory")]
    public WeaponItem[] weaponInLeftSlots = new WeaponItem[1];
    public WeaponItem[] weaponInRightSlots = new WeaponItem[1];
    public int currentLeftWeaponIndex = 0;
    public int currentRightWeaponIndex = 0;
    private PlayerWeaponSlotManager playerWeaponSlotManager;
    public List<WeaponItem> weaponsInventory;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        // initItemData = new InitItemData();
        playerWeaponSlotManager = GetComponentInChildren<PlayerWeaponSlotManager>();
    }

    void Start()
    {
        InitWeaponInventory();
    }

    void InitWeaponInventory()
    {
        leftWeapon = weaponInLeftSlots[0];
        rightWeapon = weaponInRightSlots[0];
        playerWeaponSlotManager.LoadWeaponSlot(leftWeapon, true);
        playerWeaponSlotManager.LoadWeaponSlot(rightWeapon, false);
    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex++;

        if (currentLeftWeaponIndex > weaponInLeftSlots.Length - 1) // index가 슬롯 범위를 초과할 경우
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            playerWeaponSlotManager.LoadWeaponSlot(unarmedWeapon, true);
        }
        else if (weaponInLeftSlots[currentLeftWeaponIndex] != null) // 장착할 무기가 있는 경우
        {
            leftWeapon = weaponInLeftSlots[currentLeftWeaponIndex];
            playerWeaponSlotManager.LoadWeaponSlot(weaponInLeftSlots[currentLeftWeaponIndex], true);
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
            playerWeaponSlotManager.LoadWeaponSlot(unarmedWeapon, false);
        }
        else if (weaponInRightSlots[currentRightWeaponIndex] != null) // 장착할 무기가 있는 경우
        {
            rightWeapon = weaponInRightSlots[currentRightWeaponIndex];
            playerWeaponSlotManager.LoadWeaponSlot(weaponInRightSlots[currentRightWeaponIndex], false);
        }
        else
        {
            currentRightWeaponIndex++;
        }
    }
}
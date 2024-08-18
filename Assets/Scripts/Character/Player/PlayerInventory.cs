using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private PlayerManager player;

    [Header("Weapon")]
    public List<WeaponItem> equipmentWeapons;
    public int currentLeftWeaponIndex = 0;
    public int currentRightWeaponIndex = 0;

    public WeaponItem leftWeapon;
    public WeaponItem rightWeapon;
    public WeaponItem unarmedWeapon;
    public WeaponItem[] weaponInLeftSlots;
    public WeaponItem[] weaponInRightSlots;

    [Header("Using Item")]
    public UsingItem curUsingItem;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponentInParent<PlayerManager>();
    }

    void Start()
    {
        InitWeaponInventory();
    }

    void InitWeaponInventory()
    {
        leftWeapon = weaponInLeftSlots[0];
        rightWeapon = weaponInRightSlots[0];
        player.playerWeaponSlotManager.LoadWeaponSlot(leftWeapon, true);
        player.playerWeaponSlotManager.LoadWeaponSlot(rightWeapon, false);
    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex++;

        if (currentLeftWeaponIndex > weaponInLeftSlots.Length - 1) // index�� ���� ������ �ʰ��� ���
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            player.playerWeaponSlotManager.LoadWeaponSlot(unarmedWeapon, true);
        }
        else if (weaponInLeftSlots[currentLeftWeaponIndex] != null) // ������ ���Ⱑ �ִ� ���
        {
            leftWeapon = weaponInLeftSlots[currentLeftWeaponIndex];
            player.playerWeaponSlotManager.LoadWeaponSlot(weaponInLeftSlots[currentLeftWeaponIndex], true);
        }
        else
            currentLeftWeaponIndex++;
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex++;

        if (currentRightWeaponIndex > weaponInRightSlots.Length - 1) // index�� ���� ������ �ʰ��� ���
        {
            currentRightWeaponIndex = -1;
            rightWeapon = unarmedWeapon;
            player.playerWeaponSlotManager.LoadWeaponSlot(unarmedWeapon, false);
        }
        else if (weaponInRightSlots[currentRightWeaponIndex] != null) // ������ ���Ⱑ �ִ� ���
        {
            rightWeapon = weaponInRightSlots[currentRightWeaponIndex];
            player.playerWeaponSlotManager.LoadWeaponSlot(weaponInRightSlots[currentRightWeaponIndex], false);
        }
        else
            currentRightWeaponIndex++;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;

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
        weaponSlotManager.LoadWeaponSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponSlot(leftWeapon, true);
    }
}
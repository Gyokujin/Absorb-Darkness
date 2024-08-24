using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField]
    private WeaponItem leftHandWeapon;
    [SerializeField]
    private WeaponItem rightHandWeapon;
    private WeaponHolderSlot leftHandSlot;
    private WeaponHolderSlot rightHandSlot;
    private WeaponDamageCollider leftDamageCollider;
    private WeaponDamageCollider rightDamageCollider;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
                leftHandSlot = weaponSlot;
            else if (weaponSlot.isRightHandSlot)
                rightHandSlot = weaponSlot;
        }
    }

    void Start()
    {
        LoadWeaponsHands();
    }

    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(true);
        }
        else
        {
            rightHandSlot.currentWeapon = weapon;
            rightHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(false);
        }
    }

    public void LoadWeaponsHands()
    {
        if (leftHandWeapon != null)
            LoadWeaponOnSlot(leftHandWeapon, true);

        if (rightHandWeapon != null)
            LoadWeaponOnSlot(rightHandWeapon, false);
    }

    public void LoadWeaponsDamageCollider(bool isLeft)
    {
        if (isLeft)
            leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponDamageCollider>();
        else
            rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponDamageCollider>();
    }

    public void OpenDamageCollider()
    {
        rightDamageCollider.OpenDamageCollider();
    }

    public void CloseDamageCollider()
    {
        rightDamageCollider.CloseDamageCollider();
    }
}
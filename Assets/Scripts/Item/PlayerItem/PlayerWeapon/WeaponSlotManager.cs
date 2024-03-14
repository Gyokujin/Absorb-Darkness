using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    private WeaponHolderSlot leftHandSlot;
    private WeaponHolderSlot rightHandSlot;

    private DamageCollider leftHandDamageCollider;
    private DamageCollider rightHandDamageCollider;

    [Header("Component")]
    private Animator animator;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        animator = GetComponent<Animator>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.left_Hand_Idle, 0.2f);
            }
            else
            {
                animator.CrossFade("LeftArm Empty", 0.2f);
            }
        }
        else
        {
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.right_Hand_Idle, 0.2f);
            }
            else
            {
                animator.CrossFade("RightArm Empty", 0.2f);
            }
        }
    }

    void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenLeftDamageCollider()
    {
        leftHandDamageCollider.AbleDamageCollider(true);
    }

    public void OpenRightDamageCollider()
    {
        rightHandDamageCollider.AbleDamageCollider(true);
    }

    public void CloseLeftDamageCollider()
    {
        leftHandDamageCollider.AbleDamageCollider(false);
    }

    public void CloseRightDamageCollider()
    {
        rightHandDamageCollider.AbleDamageCollider(false);
    }
}
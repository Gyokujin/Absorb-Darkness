using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemSlotManager : MonoBehaviour
{
    [Header("Weapon Slot")]
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    public WeaponHolderSlot backSlot;

    [Header("Weapon Attack")]
    [HideInInspector]
    public WeaponItem attackingWeapon;
    private DamageCollider leftHandDamageCollider;
    private DamageCollider rightHandDamageCollider;

    [Header("Animation")]
    [SerializeField]
    private string[] weaponArmIdleAnimations = { "LeftArm Empty", "RightArm Empty", "BothArms Empty" };
    [SerializeField]
    private float animationFadeAmount = 0.2f;

    [Header("Component")]
    private Animator animator;
    private PlayerManager player;
    private QuickSlotsUI quickSlotsUI;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponentInParent<PlayerManager>();
        animator = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
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
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weaponItem;
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.left_Hand_Idle, animationFadeAmount);
            }
            else
            {
                animator.CrossFade(weaponArmIdleAnimations[0], animationFadeAmount);
            }
        }
        else
        {
            if (player.playerInput.twoHandFlag)
            {
                backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                leftHandSlot.UnloadWeaponAndDestroy();
                animator.CrossFade(weaponItem.th_idle, animationFadeAmount);
            }
            else
            {
                animator.CrossFade(weaponArmIdleAnimations[2], animationFadeAmount);
                backSlot.UnloadWeaponAndDestroy();

                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.right_Hand_Idle, animationFadeAmount);
                }
                else
                {
                    animator.CrossFade(weaponArmIdleAnimations[1], animationFadeAmount);
                }
            }

            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }
    }

    public void LoadUsingItemSlot(UsingItem usingItem)
    {
        int itemCount = 0;

        switch (usingItem.itemType)
        {
            case UsingItem.UsingItemType.EstusFlask:
                itemCount = player.playerInventory.estusCount;
                break;
        }
        quickSlotsUI.UpdateUsingItemUI(usingItem, itemCount);
    }

    void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenDamageCollider()
    {
        if (player.isAttack)
        {
            if (player.isUsingLeftHand)
            {
                leftHandDamageCollider.OpenDamageCollider();
            }
            else if (player.isUsingRightHand)
            {
                rightHandDamageCollider.OpenDamageCollider();
            }
        }
    }

    public void CloseDamageCollider()
    {
        if (leftHandDamageCollider != null)
            leftHandDamageCollider.CloseDamageCollider();

        if (rightHandDamageCollider != null)
            rightHandDamageCollider.CloseDamageCollider();
    }

    public void DrainStaminaLightAttack() 
    {
        player.playerStatus.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        player.playerStatus.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
}
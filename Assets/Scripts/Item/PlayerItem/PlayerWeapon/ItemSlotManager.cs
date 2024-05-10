using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotManager : MonoBehaviour
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
    private float animacionFadeAmount = 0.2f;

    [Header("Component")]
    private Animator animator;
    private PlayerManager playerManager;
    private QuickSlotsUI quickSlotsUI;
    private PlayerInput playerInput;
    private PlayerInventory playerInventory;
    private PlayerStatus playerStatus;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        animator = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        playerStatus = GetComponentInParent<PlayerStatus>();

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
                animator.CrossFade(weaponItem.left_Hand_Idle, animacionFadeAmount);
            }
            else
            {
                animator.CrossFade(weaponArmIdleAnimations[0], animacionFadeAmount);
            }
        }
        else
        {
            if (playerInput.twoHandFlag)
            {
                backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                leftHandSlot.UnloadWeaponAndDestroy();
                animator.CrossFade(weaponItem.th_idle, animacionFadeAmount);
            }
            else
            {
                animator.CrossFade(weaponArmIdleAnimations[2], animacionFadeAmount);
                backSlot.UnloadWeaponAndDestroy();

                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.right_Hand_Idle, animacionFadeAmount);
                }
                else
                {
                    animator.CrossFade(weaponArmIdleAnimations[1], animacionFadeAmount);
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
                itemCount = playerInventory.estusCount;
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
        if (playerManager.isUsingLeftHand)
        {
            leftHandDamageCollider.OpenDamageCollider();
        }

        if (playerManager.isUsingRightHand)
        {
            rightHandDamageCollider.OpenDamageCollider();
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
        playerStatus.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStatus.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
}
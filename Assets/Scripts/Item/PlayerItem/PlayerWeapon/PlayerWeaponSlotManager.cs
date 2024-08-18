using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSlotManager : MonoBehaviour
{
    private PlayerManager player;

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
    private string[] weaponArmIdleAnimations;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponentInParent<PlayerManager>();
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
                leftHandSlot = weaponSlot;
            else if (weaponSlot.isRightHandSlot)
                rightHandSlot = weaponSlot;
            else if (weaponSlot.isBackSlot)
                backSlot = weaponSlot;
        }
    }

    public void LoadWeaponSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weaponItem;
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadWeaponDamageCollider(true);
            UIManager.instance.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);

            if (weaponItem != null)
                player.playerAnimator.animator.CrossFade(weaponItem.left_Hand_Idle, player.characterAnimatorData.AnimationFadeAmount);
            else
                player.playerAnimator.animator.CrossFade(weaponArmIdleAnimations[0], player.characterAnimatorData.AnimationFadeAmount);
        }
        else
        {
            if (player.playerInput.twoHandFlag)
            {
                backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                leftHandSlot.UnloadWeaponAndDestroy();
                player.playerAnimator.animator.CrossFade(weaponItem.th_idle, player.characterAnimatorData.AnimationFadeAmount);
            }
            else
            {
                player.playerAnimator.animator.CrossFade(weaponArmIdleAnimations[2], player.characterAnimatorData.AnimationFadeAmount);
                backSlot.UnloadWeaponAndDestroy();

                if (weaponItem != null)
                    player.playerAnimator.animator.CrossFade(weaponItem.right_Hand_Idle, player.characterAnimatorData.AnimationFadeAmount);
                else
                    player.playerAnimator.animator.CrossFade(weaponArmIdleAnimations[1], player.characterAnimatorData.AnimationFadeAmount);
            }

            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadWeaponDamageCollider(false);
            UIManager.instance.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }
    }

    public void LoadUsingItemSlot(UsingItem usingItem)
    {
        UIManager.instance.quickSlotsUI.UpdateUsingItemUI(usingItem, usingItem.itemCount);
    }

    void LoadWeaponDamageCollider(bool isLeft)
    {
        if (isLeft)
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        else
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

    }

    public void OpenDamageCollider()
    {
        if (player.isAttack)
        {
            if (player.isUsingLeftHand)
                leftHandDamageCollider.OpenDamageCollider();
            else if (player.isUsingRightHand)
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
        player.playerStatus.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        player.playerStatus.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
}
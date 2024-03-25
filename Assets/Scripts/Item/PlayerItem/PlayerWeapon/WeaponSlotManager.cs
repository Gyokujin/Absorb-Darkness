using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [Header("Weapon")]
    public WeaponItem attackingWeapon;
    private WeaponHolderSlot leftHandSlot;
    private WeaponHolderSlot rightHandSlot;
    private DamageCollider leftHandDamageCollider;
    private DamageCollider rightHandDamageCollider;

    [Header("Animation")]
    [SerializeField]
    private string[] weaponArmIdleAnimations = { "LeftArm Empty", "RightArm Empty" };
    [SerializeField]
    private float animacionFadeAmount = 0.2f;

    [Header("Component")]
    private Animator animator;
    private QuickSlotsUI quickSlotsUI;
    private PlayerStats playerStats;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponentInParent<PlayerStats>();
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
        }
    }

    public void LoadWeaponSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
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
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.right_Hand_Idle, animacionFadeAmount);
            }
            else
            {
                animator.CrossFade(weaponArmIdleAnimations[1], animacionFadeAmount);
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

    public void DrainStaminaLightAttack() 
    {
        playerStats.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStamina(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
}
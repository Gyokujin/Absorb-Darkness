using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField]
    private string lastAttack;

    [Header("Component")]
    private PlayerInput playerInput;
    private PlayerAnimator playerAnimator;
    private WeaponSlotManager weaponSlotManager;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (!playerInput.twoHandFlag) // 한손
        {
            playerAnimator.PlayTargetAnimation(weapon.oneHand_LightAttack1, true);
            lastAttack = weapon.oneHand_LightAttack1;
        }
        else // 두손
        {
            playerAnimator.PlayTargetAnimation(weapon.twoHand_LightAttack1, true);
            lastAttack = weapon.twoHand_LightAttack1;
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (!playerInput.twoHandFlag)
        {
            playerAnimator.PlayTargetAnimation(weapon.oneHand_HeavyAttack1, true);
            lastAttack = weapon.oneHand_HeavyAttack1;
        }
        else
        {
            playerAnimator.PlayTargetAnimation(weapon.twoHand_HeavyAttack1, true);
            lastAttack = weapon.twoHand_HeavyAttack1;
        }
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerInput.comboFlag)
        {
            playerAnimator.animator.SetBool("canDoCombo", false);

            if (lastAttack == weapon.oneHand_LightAttack1)
            {
                playerAnimator.PlayTargetAnimation(weapon.oneHand_LightAttack2, true);
            }
            else if (lastAttack == weapon.twoHand_LightAttack1)
            {
                playerAnimator.PlayTargetAnimation(weapon.twoHand_LightAttack2, true);
            }
        }
    }
}
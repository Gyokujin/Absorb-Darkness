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
        playerAnimator.PlayTargetAnimation(weapon.OneHand_LightAttack1, true);
        lastAttack = weapon.OneHand_LightAttack1;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;
        playerAnimator.PlayTargetAnimation(weapon.OneHand_HeavyAttack1, true);
        lastAttack = weapon.OneHand_HeavyAttack1;
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerInput.comboFlag)
        {
            playerAnimator.animator.SetBool("canDoCombo", false);

            if (lastAttack == weapon.OneHand_LightAttack1)
            {
                playerAnimator.PlayTargetAnimation(weapon.OneHand_LightAttack2, true);
            }
        }
    }
}
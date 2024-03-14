using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    public string lastAttack;

    [Header("Component")]
    private PlayerInput playerInput;
    private PlayerAnimator playerAnimator;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        playerAnimator.PlayTargetAnimation(weapon.OneHand_LightAttack1, true);
        lastAttack = weapon.OneHand_LightAttack1;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
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
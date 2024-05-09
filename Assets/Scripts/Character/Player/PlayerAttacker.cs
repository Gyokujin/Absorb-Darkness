using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField]
    private string lastAttack;

    [Header("Component")]
    private PlayerInput playerInput;
    private PlayerStatus playerStatus;
    private PlayerAnimator playerAnimator;
    private ItemSlotManager weaponSlotManager;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerInput = GetComponent<PlayerInput>();
        playerStatus = GetComponent<PlayerStatus>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        weaponSlotManager = GetComponentInChildren<ItemSlotManager>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (playerStatus.CurrentStamina >= playerStatus.actionLimitStamina)
        {
            if (!playerInput.twoHandFlag) // �Ѽ�
            {
                playerAnimator.PlayTargetAnimation(weapon.oneHand_LightAttack1, true);
                lastAttack = weapon.oneHand_LightAttack1;
            }
            else // �μ�
            {
                playerAnimator.PlayTargetAnimation(weapon.twoHand_LightAttack1, true);
                lastAttack = weapon.twoHand_LightAttack1;
            }
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (playerStatus.CurrentStamina >= playerStatus.actionLimitStamina)
        {
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
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerInput.comboFlag && playerStatus.CurrentStamina >= playerStatus.actionLimitStamina)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private PlayerManager player;

    [Header("Attack")]
    [SerializeField]
    private string lastAttack;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        player.playerItemSlotManager.attackingWeapon = weapon;

        if (player.playerStatus.CurrentStamina >= player.playerStatus.actionLimitStamina)
        {
            if (!player.playerInput.twoHandFlag) // 한손
            {
                player.playerAnimator.PlayTargetAnimation(weapon.oneHand_LightAttack1, true);
                lastAttack = weapon.oneHand_LightAttack1;
            }
            else // 두손
            {
                player.playerAnimator.PlayTargetAnimation(weapon.twoHand_LightAttack1, true);
                lastAttack = weapon.twoHand_LightAttack1;
            }
        }
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        player.playerItemSlotManager.attackingWeapon = weapon;

        if (player.playerStatus.CurrentStamina >= player.playerStatus.actionLimitStamina)
        {
            if (!player.playerInput.twoHandFlag)
            {
                player.playerAnimator.PlayTargetAnimation(weapon.oneHand_HeavyAttack1, true);
                lastAttack = weapon.oneHand_HeavyAttack1;
            }
            else
            {
                player.playerAnimator.PlayTargetAnimation(weapon.twoHand_HeavyAttack1, true);
                lastAttack = weapon.twoHand_HeavyAttack1;
            }
        }
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (player.playerInput.comboFlag && player.playerStatus.CurrentStamina >= player.playerStatus.actionLimitStamina)
        {
            player.playerAnimator.animator.SetBool("canDoCombo", false);

            if (lastAttack == weapon.oneHand_LightAttack1)
            {
                player.playerAnimator.PlayTargetAnimation(weapon.oneHand_LightAttack2, true);
            }
            else if (lastAttack == weapon.twoHand_LightAttack1)
            {
                player.playerAnimator.PlayTargetAnimation(weapon.twoHand_LightAttack2, true);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

public class PlayerAttacker : MonoBehaviour
{
    private PlayerManager player;
    private PlayerAnimatorData animatorData;

    public enum PlayerAttackType
    {
        Sword_OneHand_LightAttack1, Sword_OneHand_LightAttack2, Sword_OneHand_HeavyAttack1, Sword_OneHand_HeavyAttack2,
        Sword_TwoHand_LightAttack1, Sword_TwoHand_LightAttack2, Sword_TwoHand_HeavyAttack1, Sword_TwoHand_HeavyAttack2
    }

    private PlayerAttackType curAttack;

    [Header("Attack")]
    [SerializeField]
    private string lastAttack;

    [Header("Weapon")]
    private WeaponItem playerWeapon;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
        animatorData = new PlayerAnimatorData();
    }

    public void HandleWeaponAttack(WeaponItem weapon, bool onLightAttack)
    {
        player.playerItemSlotManager.attackingWeapon = weapon;
        player.playerAnimator.animator.SetBool(animatorData.attackParameter, true);
        player.playerAnimator.animator.SetBool(animatorData.onUsingRightHand, true);
        playerWeapon = player.playerItemSlotManager.attackingWeapon;
        bool oneHand = !player.playerInput.twoHandFlag;

        switch (playerWeapon.weaponType)
        {
            case WeaponItem.WeaponType.None:
                break;

            case WeaponItem.WeaponType.Sword:

                if (onLightAttack)
                {
                    lastAttack = oneHand ? weapon.oneHand_LightAttack1 : weapon.twoHand_LightAttack1;
                }
                else
                {
                    lastAttack = oneHand ? weapon.oneHand_HeavyAttack1 : weapon.twoHand_HeavyAttack1;
                }
                break;

            case WeaponItem.WeaponType.Axe:
                break;
        }

        if (lastAttack != null)
        {
            player.playerAnimator.PlayTargetAnimation(lastAttack, true);
            player.playerAnimator.animator.SetBool(animatorData.attackParameter, true);
        }
    }

    public void HandleWeaponCombo(WeaponItem weapon, bool onLightAttack)
    {
        player.playerAnimator.animator.SetBool(animatorData.comboAbleParameter, false);
        bool oneHand = !player.playerInput.twoHandFlag;

        switch (playerWeapon.weaponType)
        {
            case WeaponItem.WeaponType.None:
                break;

            case WeaponItem.WeaponType.Sword:
                if (onLightAttack)
                {
                    lastAttack = oneHand ? weapon.oneHand_LightAttack2 : weapon.twoHand_LightAttack2;
                }
                else
                {
                    lastAttack = oneHand ? weapon.oneHand_HeavyAttack2 : weapon.twoHand_HeavyAttack2;
                }

                break;

            case WeaponItem.WeaponType.Axe:

                break;
        }

        if (lastAttack != null)
        {
            player.playerAnimator.PlayTargetAnimation(lastAttack, true);
        }
    }
}
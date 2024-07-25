using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private PlayerManager player;

    public enum PlayerAttackType
    {
        Sword_OneHand_LightAttack1, Sword_OneHand_LightAttack2, Sword_OneHand_HeavyAttack1, Sword_OneHand_HeavyAttack2,
        Sword_TwoHand_LightAttack1, Sword_TwoHand_LightAttack2, Sword_TwoHand_HeavyAttack1, Sword_TwoHand_HeavyAttack2
    }

    [Header("Attack")]
    [SerializeField]
    private string lastAttack;
    private PlayerAttackType curAttack;

    private WeaponItem playerWeapon;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
    }

    public void HandleWeaponAttack(WeaponItem weapon, bool onLightAttack)
    {
        player.playerItemSlotManager.attackingWeapon = weapon;
        player.playerAnimator.animator.SetBool("onAttack", true);
        player.playerAnimator.animator.SetBool("usingRightHand", true);
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
            player.playerAnimator.animator.SetBool("onAttack", true);
        }
    }

    public void HandleWeaponCombo(WeaponItem weapon, bool onLightAttack)
    {
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
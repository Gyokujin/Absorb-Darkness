using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerManager player;

    [Header("Combat")]
    private string lastAttack;
    [HideInInspector]
    public int defaultLayer;
    [HideInInspector]
    public int invincibleLayer;

    [Header("Weapon")]
    public WeaponItem usingWeapon;
    [HideInInspector]
    public WeaponDamageCollider leftHandDamageCollider;
    [HideInInspector]
    public WeaponDamageCollider rightHandDamageCollider;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
        defaultLayer = LayerMask.NameToLayer(player.gameObjectData.PlayerLayer);
        invincibleLayer = LayerMask.NameToLayer(player.gameObjectData.InvincibleLayer);
    }

    public void HandleWeaponAttack(WeaponItem weapon, bool onLightAttack)
    {
        usingWeapon = weapon;
        player.playerInput.sprintFlag = false;
        player.isSprinting = false;

        player.playerAnimator.animator.SetBool(player.characterAnimatorData.AttackParameter, true);
        player.playerAnimator.animator.SetBool(player.characterAnimatorData.OnUsingRightHand, true);
        bool oneHand = !player.playerInput.twoHandFlag;

        switch (usingWeapon.weaponType)
        {
            case WeaponItem.WeaponType.None:
                break;

            case WeaponItem.WeaponType.Sword:

                if (onLightAttack)
                    lastAttack = oneHand ? weapon.oneHand_LightAttack1 : weapon.twoHand_LightAttack1;
                else
                    lastAttack = oneHand ? weapon.oneHand_HeavyAttack1 : weapon.twoHand_HeavyAttack1;
                break;

            case WeaponItem.WeaponType.Axe:
                break;
        }

        if (lastAttack != null)
        {
            player.playerAnimator.PlayTargetAnimation(lastAttack, true);
            player.playerAnimator.animator.SetBool(player.characterAnimatorData.AttackParameter, true);
        }
    }

    public void HandleWeaponCombo(WeaponItem weapon, bool onLightAttack)
    {
        player.playerAnimator.animator.SetBool(player.characterAnimatorData.ComboAbleParameter, false);
        bool oneHand = !player.playerInput.twoHandFlag;

        switch (usingWeapon.weaponType)
        {
            case WeaponItem.WeaponType.None:
            case WeaponItem.WeaponType.Axe:
                break;

            case WeaponItem.WeaponType.Sword:
                if (onLightAttack)
                    lastAttack = oneHand ? weapon.oneHand_LightAttack2 : weapon.twoHand_LightAttack2;
                else
                    lastAttack = oneHand ? weapon.oneHand_HeavyAttack2 : weapon.twoHand_HeavyAttack2;

                break;
        }

        if (lastAttack != null)
            player.playerAnimator.PlayTargetAnimation(lastAttack, true);
    }

    public void LoadWeaponDamageCollider(bool isLeft)
    {
        if (isLeft)
            leftHandDamageCollider = player.playerInventory.leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponDamageCollider>();
        else
            rightHandDamageCollider = player.playerInventory.rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponDamageCollider>();
    }
}
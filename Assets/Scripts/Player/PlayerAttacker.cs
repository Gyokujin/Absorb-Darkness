using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private PlayerAnimator playerAnimator;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        playerAnimator.PlayTargetAnimation(weapon.OneHand_LightAttack1, true);
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        playerAnimator.PlayTargetAnimation(weapon.OneHand_HeavyAttack1, true);
    }
}
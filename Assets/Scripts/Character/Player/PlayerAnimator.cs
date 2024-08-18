using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : AnimatorManager
{
    private PlayerManager player;

    [Header("Animator Parameters")]
    private float parameterHor;
    private float parameterVer;

    void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        player = GetComponentInParent<PlayerManager>();
    }

    public void AnimatorValue(float moveVer, float moveHor, bool isSprinting)
    {
        // Vertical 파라미터
        parameterVer = player.characterAnimatorData.IdleParameterValue;

        if (moveVer > 0 && moveVer < player.characterAnimatorData.RunAnimationCondition)
        {
            parameterVer = player.characterAnimatorData.WalkParameterValue;
        }
        else if (moveVer > player.characterAnimatorData.RunAnimationCondition)
        {
            parameterVer = player.characterAnimatorData.RunParameterValue;
        }
        else if (moveVer < 0 && moveVer > -player.characterAnimatorData.RunAnimationCondition)
        {
            parameterVer = player.characterAnimatorData.WalkParameterValue ;
        }
        else if (moveVer < -player.characterAnimatorData.RunAnimationCondition)
        {
            parameterVer = -player.characterAnimatorData.RunParameterValue;
        }
        else
        {
            parameterVer = player.characterAnimatorData.IdleParameterValue;
        }

        // Horizontal 파라미터
        parameterHor = player.characterAnimatorData.IdleParameterValue;

        if (moveHor > 0 && moveHor < player.characterAnimatorData.RunAnimationCondition)
        {
            parameterHor = player.characterAnimatorData.WalkParameterValue;
        }
        else if (moveHor > player.characterAnimatorData.RunAnimationCondition)
        {
            parameterHor = player.characterAnimatorData.RunParameterValue;
        }
        else if (moveHor < 0 && moveHor > -player.characterAnimatorData.RunAnimationCondition)
        {
            parameterHor = -player.characterAnimatorData.WalkParameterValue;
        }
        else if (moveHor < -player.characterAnimatorData.RunAnimationCondition)
        {
            parameterHor = -player.characterAnimatorData.RunParameterValue;
        }
        else
        {
            parameterHor = player.characterAnimatorData.IdleParameterValue;
        }

        if (isSprinting)
        {
            parameterVer = player.characterAnimatorData.SprintParameterValue;
            parameterHor = player.characterAnimatorData.SprintParameterValue;
        }

        // 애니메이터 파라미터 입력
        animator.SetFloat(player.characterAnimatorData.VerticalParameter, parameterVer, player.characterAnimatorData.AnimationDampTime, Time.deltaTime);
        animator.SetFloat(player.characterAnimatorData.HorizontalParameter, parameterHor, player.characterAnimatorData.AnimationDampTime, Time.deltaTime);
    }

    void OnAnimatorMove()
    {
        if (!player.isInteracting)
            return;

        Vector3 velocity = new Vector3(animator.deltaPosition.x, 0, animator.deltaPosition.z) / Time.deltaTime;
        player.playerMove.rigidbody.velocity = velocity;
    }

    public void PlayAttackSFX()
    {
        player.playerAudio.PlaySFX(player.playerAudio.playerClips[(int)PlayerAudio.PlayerSound.Attack1]);
    }

    public void DodgeEnd()
    {
        player.isDodge = false;
        player.playerInput.rollFlag = false;
    }

    public void HitEnd()
    {
        player.onDamage = false;
    }

    public void EnableCombo()
    {
        player.playerAnimator.animator.SetBool(player.characterAnimatorData.ComboAbleParameter, true);
    }

    public void DisableCombo()
    {
        player.playerAnimator.animator.SetBool(player.characterAnimatorData.ComboAbleParameter, false);
    }

    public void SwitchStance(bool onStance)
    {
        animator.SetBool(player.characterAnimatorData.OnStanceParameter, onStance);
    }

    public void Drink()
    {
        player.playerAudio.PlaySFX(player.playerAudio.playerClips[(int)PlayerAudio.PlayerSound.Drink]);
    }

    public void Recovery()
    {
        UsingItem item = player.playerInventory.curUsingItem;

        if (item is RecoveryItem recoveryItem)
        {
            if (recoveryItem.hpRecoveryAmount > 0)
            {
                player.playerStatus.RecoveryHP(recoveryItem.hpRecoveryAmount);
            }

            //if (recoveryItem.mpRecoveryAmount > 0) // 아직 미정
            //{
            //    player.playerStatus.RecoveryMP(recoveryItem.mpRecoveryAmount);
            //}
        }
    }

    public void ItemUseEnd()
    {
        player.playerBehavior.EndItemUse();
    }

    public override void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool(player.characterAnimatorData.InteractParameter, isInteracting);
        animator.CrossFade(targetAnim, animatorData.AnimationFadeAmount);
    }

    public void OpenDamageCollider()
    {
        if (player.isAttack)
        {
            if (player.isUsingLeftHand)
                player.playerCombat.leftHandDamageCollider.OpenDamageCollider();
            else if (player.isUsingRightHand)
                player.playerCombat.rightHandDamageCollider.OpenDamageCollider();
        }
    }

    public void CloseDamageCollider()
    {
        if (player.playerCombat.leftHandDamageCollider != null)
            player.playerCombat.leftHandDamageCollider.CloseDamageCollider();

        if (player.playerCombat.rightHandDamageCollider != null)
            player.playerCombat.rightHandDamageCollider.CloseDamageCollider();
    }

    public void DrainStaminaLightAttack()
    {
        player.playerStatus.TakeStamina(Mathf.RoundToInt(player.playerCombat.usingWeapon.baseStamina * player.playerCombat.usingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        player.playerStatus.TakeStamina(Mathf.RoundToInt(player.playerCombat.usingWeapon.baseStamina * player.playerCombat.usingWeapon.heavyAttackMultiplier));
    }
}
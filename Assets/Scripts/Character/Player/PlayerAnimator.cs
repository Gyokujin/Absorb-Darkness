using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using Unity.VisualScripting;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : AnimatorManager
{
    private PlayerManager player;

    [Header("Data")]
    private PlayerAnimatorData playerAnimatorData;

    [Header("Animator Parameters")]
    private float parameterHor;
    private float parameterVer;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponentInParent<PlayerManager>();
        animator = GetComponent<Animator>();
        playerAnimatorData = new PlayerAnimatorData(); // PlayerData 구조체 생성
    }

    public void AnimatorValue(float moveVer, float moveHor, bool isSprinting)
    {
        // Vertical 파라미터
        parameterVer = playerAnimatorData.IdleParameterValue;

        if (moveVer > 0 && moveVer < playerAnimatorData.RunAnimationCondition)
        {
            parameterVer = playerAnimatorData.WalkParameterValue;
        }
        else if (moveVer > playerAnimatorData.RunAnimationCondition)
        {
            parameterVer = playerAnimatorData.RunParameterValue;
        }
        else if (moveVer < 0 && moveVer > -playerAnimatorData.RunAnimationCondition)
        {
            parameterVer = playerAnimatorData.WalkParameterValue ;
        }
        else if (moveVer < -playerAnimatorData.RunAnimationCondition)
        {
            parameterVer = -playerAnimatorData.RunParameterValue;
        }
        else
        {
            parameterVer = playerAnimatorData.IdleParameterValue;
        }

        // Horizontal 파라미터
        parameterHor = playerAnimatorData.IdleParameterValue;

        if (moveHor > 0 && moveHor < playerAnimatorData.RunAnimationCondition)
        {
            parameterHor = playerAnimatorData.WalkParameterValue;
        }
        else if (moveHor > playerAnimatorData.RunAnimationCondition)
        {
            parameterHor = playerAnimatorData.RunParameterValue;
        }
        else if (moveHor < 0 && moveHor > -playerAnimatorData.RunAnimationCondition)
        {
            parameterHor = -playerAnimatorData.WalkParameterValue;
        }
        else if (moveHor < -playerAnimatorData.RunAnimationCondition)
        {
            parameterHor = -playerAnimatorData.RunParameterValue;
        }
        else
        {
            parameterHor = playerAnimatorData.IdleParameterValue;
        }

        if (isSprinting)
        {
            parameterVer = playerAnimatorData.SprintParameterValue;
            parameterHor = playerAnimatorData.SprintParameterValue;
        }

        // 애니메이터 파라미터 입력
        animator.SetFloat(playerAnimatorData.VerticalParameter, parameterVer, playerAnimatorData.AnimationDampTime, Time.deltaTime);
        animator.SetFloat(playerAnimatorData.HorizontalParameter, parameterHor, playerAnimatorData.AnimationDampTime, Time.deltaTime);
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
        player.playerAnimator.animator.SetBool(playerAnimatorData.ComboAbleParameter, true);
    }

    public void DisableCombo()
    {
        player.playerAnimator.animator.SetBool(playerAnimatorData.ComboAbleParameter, false);
    }

    public void SwitchStance(bool onStance)
    {
        animator.SetBool(playerAnimatorData.OnStanceParameter, onStance);
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
        animator.SetBool(playerAnimatorData.InteractParameter, isInteracting);
        animator.CrossFade(targetAnim, animationFadeAmount);
    }
}
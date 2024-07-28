using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : AnimatorManager
{
    private PlayerManager player;
    private PlayerAnimatorData animatorData;

    [Header("Animator Parameters")]
    private float parameterHor;
    private float parameterVer;

    void Awake()
    {
        player = GetComponentInParent<PlayerManager>();
        animator = GetComponent<Animator>();
        animatorData = new PlayerAnimatorData(); // PlayerData 구조체 생성
    }

    public void AnimatorValue(float moveVer, float moveHor, bool isSprinting)
    {
        // Vertical 파라미터
        parameterVer = animatorData.idleParameterValue;

        if (moveVer > 0 && moveVer < animatorData.runAnimationCondition)
        {
            parameterVer = animatorData.walkParameterValue;
        }
        else if (moveVer > animatorData.runAnimationCondition)
        {
            parameterVer = animatorData.runParameterValue;
        }
        else if (moveVer < 0 && moveVer > -animatorData.runAnimationCondition)
        {
            parameterVer = animatorData.walkParameterValue ;
        }
        else if (moveVer < -animatorData.runAnimationCondition)
        {
            parameterVer = -animatorData.runParameterValue;
        }
        else
        {
            parameterVer = animatorData.idleParameterValue;
        }

        // Horizontal 파라미터
        parameterHor = animatorData.idleParameterValue;

        if (moveHor > 0 && moveHor < animatorData.runAnimationCondition)
        {
            parameterHor = animatorData.walkParameterValue;
        }
        else if (moveHor > animatorData.runAnimationCondition)
        {
            parameterHor = animatorData.runParameterValue;
        }
        else if (moveHor < 0 && moveHor > -animatorData.runAnimationCondition)
        {
            parameterHor = -animatorData.walkParameterValue;
        }
        else if (moveHor < -animatorData.runAnimationCondition)
        {
            parameterHor = -animatorData.runParameterValue;
        }
        else
        {
            parameterHor = animatorData.idleParameterValue;
        }

        if (isSprinting)
        {
            parameterVer = animatorData.sprintParameterValue;
            parameterHor = animatorData.sprintParameterValue;
        }

        // 애니메이터 파라미터 입력
        animator.SetFloat(animatorData.verticalParameter, parameterVer, animatorData.animationDampTime, Time.deltaTime);
        animator.SetFloat(animatorData.horizontalParameter, parameterHor, animatorData.animationDampTime, Time.deltaTime);
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
        player.playerAnimator.animator.SetBool(animatorData.comboAbleParameter, true);
    }

    public void DisableCombo()
    {
        player.playerAnimator.animator.SetBool(animatorData.comboAbleParameter, false);
    }

    public void SwitchStance(bool onStance)
    {
        animator.SetBool(animatorData.onStanceParameter, onStance);
    }

    public void Drink()
    {
        player.playerAudio.PlaySFX(player.playerAudio.playerClips[(int)PlayerAudio.PlayerSound.Drink]);
    }

    public void Recovery()
    {
        player.playerStatus.RecoveryHealth();
    }

    public void ItemUseEnd()
    {
        player.playerBehavior.EndItemUse();
    }
}
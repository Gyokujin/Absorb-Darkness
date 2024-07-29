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
        parameterVer = animatorData.IdleParameterValue;

        if (moveVer > 0 && moveVer < animatorData.RunAnimationCondition)
        {
            parameterVer = animatorData.WalkParameterValue;
        }
        else if (moveVer > animatorData.RunAnimationCondition)
        {
            parameterVer = animatorData.RunParameterValue;
        }
        else if (moveVer < 0 && moveVer > -animatorData.RunAnimationCondition)
        {
            parameterVer = animatorData.WalkParameterValue ;
        }
        else if (moveVer < -animatorData.RunAnimationCondition)
        {
            parameterVer = -animatorData.RunParameterValue;
        }
        else
        {
            parameterVer = animatorData.IdleParameterValue;
        }

        // Horizontal 파라미터
        parameterHor = animatorData.IdleParameterValue;

        if (moveHor > 0 && moveHor < animatorData.RunAnimationCondition)
        {
            parameterHor = animatorData.WalkParameterValue;
        }
        else if (moveHor > animatorData.RunAnimationCondition)
        {
            parameterHor = animatorData.RunParameterValue;
        }
        else if (moveHor < 0 && moveHor > -animatorData.RunAnimationCondition)
        {
            parameterHor = -animatorData.WalkParameterValue;
        }
        else if (moveHor < -animatorData.RunAnimationCondition)
        {
            parameterHor = -animatorData.RunParameterValue;
        }
        else
        {
            parameterHor = animatorData.IdleParameterValue;
        }

        if (isSprinting)
        {
            parameterVer = animatorData.SprintParameterValue;
            parameterHor = animatorData.SprintParameterValue;
        }

        // 애니메이터 파라미터 입력
        animator.SetFloat(animatorData.VerticalParameter, parameterVer, animatorData.AnimationDampTime, Time.deltaTime);
        animator.SetFloat(animatorData.HorizontalParameter, parameterHor, animatorData.AnimationDampTime, Time.deltaTime);
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
        player.playerAnimator.animator.SetBool(animatorData.ComboAbleParameter, true);
    }

    public void DisableCombo()
    {
        player.playerAnimator.animator.SetBool(animatorData.ComboAbleParameter, false);
    }

    public void SwitchStance(bool onStance)
    {
        animator.SetBool(animatorData.OnStanceParameter, onStance);
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
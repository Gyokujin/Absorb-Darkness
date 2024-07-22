using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerDatas;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : AnimatorManager
{
    private PlayerManager player;

    [Header("Animator Parameters")]
    [HideInInspector]
    public bool canRotate = true;
    private float parameterHor;
    private float parameterVer;

    [Header("Component")]
    private PlayerAnimatorData animatorData;

    public void Init()
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
        animator.SetFloat("vertical", parameterVer, animatorData.animationDampTime, Time.deltaTime);
        animator.SetFloat("horizontal", parameterHor, animatorData.animationDampTime, Time.deltaTime);
    }

    void OnAnimatorMove()
    {
        if (!player.isInteracting)
            return;

        Vector3 velocity = new Vector3(animator.deltaPosition.x, 0, animator.deltaPosition.z) / Time.deltaTime;
        player.playerMove.rigidbody.velocity = velocity;
    }

    public void CanRotate(bool isCan)
    {
        canRotate = isCan;
    }

    public void PlayAttackSFX()
    {
        AudioManager.instance.PlayPlayerActionSFX(AudioManager.instance.playerActionClips[(int)PlayerActionSound.Attack1]);
    }

    public void DodgeEnd()
    {
        player.onDodge = false;
        player.playerInput.rollFlag = false;
    }

    public void HitEnd()
    {
        player.onDamage = false;
    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public void SwitchStance(bool onStance)
    {
        animator.SetBool("onStance", onStance);
    }

    public void Drink()
    {
        AudioManager.instance.PlayPlayerActionSFX(AudioManager.instance.playerActionClips[(int)PlayerActionSound.Drink]);
    }

    public void Recovery()
    {
        player.playerStatus.RecoveryHealth();
    }

    public void ItemUseEnd()
    {
        player.playerItemUse.EndItemUse();
    }
}
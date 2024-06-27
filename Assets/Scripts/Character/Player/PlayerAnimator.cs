using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : AnimatorManager
{
    private PlayerManager player;

    [Header("Animator Parameters")]
    public bool canRotate = true;
    private float parameterHor;
    private float parameterVer;
    [SerializeField]
    private float idleParameterValue = 0;
    [SerializeField]
    private float walkParameterValue = 0.5f;
    [SerializeField]
    private float runParameterValue = 1;
    [SerializeField]
    private float sprintParameterValue = 2;
    [SerializeField]
    private float animationDampTime = 0.1f;

    public void Init()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<PlayerManager>();
    }

    public void AnimatorValue(float moveVer, float moveHor, bool isSprinting)
    {
        // Vertical 파라미터
        parameterVer = idleParameterValue;

        if (moveVer > 0 && moveVer < 0.55f)
        {
            parameterVer = walkParameterValue;
        }
        else if (moveVer > 0.55f)
        {
            parameterVer = runParameterValue;
        }
        else if (moveVer < 0 && moveVer > -0.55f)
        {
            parameterVer = -walkParameterValue;
        }
        else if (moveVer < -0.55f)
        {
            parameterVer = -runParameterValue;
        }
        else
        {
            parameterVer = idleParameterValue;
        }

        // Horizontal 파라미터
        parameterHor = idleParameterValue;

        if (moveHor > 0 && moveHor < 0.55f)
        {
            parameterHor = walkParameterValue;
        }
        else if (moveHor > 0.55f)
        {
            parameterHor = runParameterValue;
        }
        else if (moveHor < 0 && moveHor > -0.55f)
        {
            parameterHor = -walkParameterValue;
        }
        else if (moveHor < -0.55f)
        {
            parameterHor = -runParameterValue;
        }
        else
        {
            parameterHor = idleParameterValue;
        }

        if (isSprinting)
        {
            parameterVer = sprintParameterValue;
            parameterHor = sprintParameterValue;
        }

        // 애니메이터 파라미터 입력
        animator.SetFloat("vertical", parameterVer, animationDampTime, Time.deltaTime);
        animator.SetFloat("horizontal", parameterHor, animationDampTime, Time.deltaTime);
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
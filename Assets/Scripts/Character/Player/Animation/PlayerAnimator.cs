using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    public bool canRotate = true;

    [Header("Animator Parameters")]
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
    [SerializeField]
    private float animacionFadeAmount = 0.2f;

    [Header("Component")]
    [HideInInspector]
    public Animator animator;
    private PlayerManager playerManager;
    private PlayerInput playerInput;
    private PlayerMove playerMove;

    public void Init()
    {
        animator = GetComponent<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerMove = GetComponentInParent<PlayerMove>();
        AA();
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

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, animacionFadeAmount);
    }

    public void CanRotate(bool isCan)
    {
        canRotate = isCan;
    }

    public void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }
    public void AA()
    {
        animator.SetLayerWeight(0, 0);
    }

    void OnAnimatorMove()
    {
        if (!playerManager.isInteracting)
            return;

        playerMove.rigidbody.drag = 0;
        Vector3 deltaPosition = new Vector3(animator.deltaPosition.x, 0, animator.deltaPosition.z);
        Vector3 velocity = deltaPosition / Time.deltaTime;
        playerMove.rigidbody.velocity = velocity;
    }
}
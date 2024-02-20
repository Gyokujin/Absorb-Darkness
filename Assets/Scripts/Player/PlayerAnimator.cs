using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    public bool canRotate = true;

    public void Init()
    {
        animator = GetComponent<Animator>();
    }

    public void AnimatorValue(float moveVer, float moveHor)
    {
        // Vertical 파라미터
        float v = 0;

        if (moveVer > 0 && moveVer < 0.55f)
        {
            v = 0.5f;
        }
        else if (moveVer > 0.55f)
        {
            v = 1;
        }
        else if (moveVer < 0 && moveVer > -0.55f)
        {
            v = -0.5f;
        }
        else if (moveVer < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }

        // Horizontal 파라미터
        float h = 0;

        if (moveHor > 0 && moveHor < 0.55f)
        {
            h = 0.5f;
        }
        else if (moveHor > 0.55f)
        {
            h = 1;
        }
        else if (moveHor < 0 && moveHor > -0.55f)
        {
            h = -0.5f;
        }
        else if (moveHor < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }

        // 애니메이터 파라미터 입력
        animator.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
        animator.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);
    }

    public void CanRotate(bool isCan)
    {
        canRotate = isCan;
    }
}
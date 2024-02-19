using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    [Header("Parameter")]
    private int vertical;
    private int horizontal;

    public bool canRotate = true;

    public void Init()
    {
        animator = GetComponent<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
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
        animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void CanRotate(bool isCan)
    {
        canRotate = isCan;
    }
}
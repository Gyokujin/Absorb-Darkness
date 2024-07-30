using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [Header("Animator")]
    protected const float animationFadeAmount = 0.2f;
    [HideInInspector]
    public Animator animator;

    public virtual void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, animationFadeAmount);
    }
}
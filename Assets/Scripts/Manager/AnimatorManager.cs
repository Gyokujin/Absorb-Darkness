using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterData;

[RequireComponent(typeof(Animator))]
public class AnimatorManager : MonoBehaviour
{
    [Header("Data")]
    protected CharacterAnimatorData animatorData;

    [Header("Animator")]
    [HideInInspector]
    public Animator animator;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool(animatorData.InteractParameter, isInteracting);
        animator.CrossFade(targetAnim, animatorData.AnimationFadeAmount);
    }
}
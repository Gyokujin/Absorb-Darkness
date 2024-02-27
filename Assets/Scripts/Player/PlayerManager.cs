using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private Animator animator;

    void Start()
    {
        Init();
    }

    void Init()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        playerInput.isInteracting = animator.GetBool("isInteracting");
        playerInput.rollFlag = false;
        playerInput.sprintFlag = false;
    }
}
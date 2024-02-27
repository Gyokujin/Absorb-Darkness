using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputHandler inputHandler;
    private Animator animator;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        inputHandler.isInteracting = animator.GetBool("isInteracting");
        inputHandler.rollFlag = false;
    }
}
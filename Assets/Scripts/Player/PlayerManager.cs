using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Action")]
    public bool isInteracting;
    public bool isSprinting;

    [Header("Component")]
    private PlayerInput playerInput;
    private PlayerMove playerMove;
    private PlayerCamera playerCamera;
    private Animator animator;

    void Start()
    {
        Init();
    }

    void Init()
    {
        playerCamera = PlayerCamera.instance;

        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = animator.GetBool("isInteracting");
        playerInput.TickInput(delta);
        playerMove.HandleMovement(delta);
        playerMove.HandleRollingAndSprinting(delta);
    }

    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (playerCamera != null)
        {
            playerCamera.FollowTarget(delta);
            playerCamera.HandleCameraRotation(delta, playerInput.mouseX, playerInput.mouseY);
        }
    }

    void LateUpdate()
    {
        playerInput.rollFlag = false;
        playerInput.sprintFlag = false;
        isSprinting = playerInput.b_Input;
    }
}
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
        float fixedDelta = Time.fixedDeltaTime;

        isInteracting = animator.GetBool("isInteracting");
        playerInput.TickInput(delta);
        playerMove.HandleMovement(delta);
        playerMove.HandleRollingAndSprinting(delta);

        if (playerCamera != null)
        {
            playerCamera.FollowTarget(fixedDelta);
            playerCamera.HandleCameraRotation(fixedDelta, playerInput.mouseX, playerInput.mouseY);
        }
    }

    void LateUpdate()
    {
        playerInput.rollFlag = false;
        playerInput.sprintFlag = false;
        isSprinting = playerInput.b_Input;
    }
}
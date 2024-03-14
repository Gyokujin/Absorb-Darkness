using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Action")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    [Header("Component")]
    private PlayerInput playerInput;
    private PlayerMove playerMove;
    private PlayerCamera playerCamera;
    private Animator animator;

    void Awake()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        float fixedDelta = Time.fixedDeltaTime;
        canDoCombo = animator.GetBool("canDoCombo");

        isInteracting = animator.GetBool("isInteracting");
        playerInput.TickInput(delta);
        playerMove.HandleMovement(delta);
        playerMove.HandleRollingAndSprinting(delta);
        playerMove.HandleFalling(delta, playerMove.moveDirection);

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
        playerInput.rb_Input = false;
        playerInput.rt_Input = false;
        playerInput.d_Pad_Up = false;
        playerInput.d_Pad_Down = false;
        playerInput.d_Pad_Left = false;
        playerInput.d_Pad_Right = false;

        if (isInAir)
        {
            playerMove.inAirTimer += Time.deltaTime;
        }
    }
}
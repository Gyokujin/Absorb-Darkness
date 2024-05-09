using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Player Action")]
    public bool isInteracting;
    public bool isSprinting;
    public bool onDodge;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isUsingLeftHand;
    public bool isUsingRightHand;

    [Header("Combat")]
    public int defaultLayer;
    public int invincibleLayer;

    [Header("Component")]
    [HideInInspector]
    public PlayerMove playerMove;
    private PlayerInput playerInput;
    [HideInInspector]
    public PlayerAnimator playerAnimator;
    [HideInInspector]
    public PlayerInventory playerInventory;
    private PlayerCamera playerCamera;
    private PlayerInteract playerInteract;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        defaultLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("Invincible");

        playerMove = GetComponent<PlayerMove>();
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        playerInteract = GetComponent<PlayerInteract>();
    }

    void Update()
    {
        isInteracting = playerAnimator.animator.GetBool("isInteracting");
        canDoCombo = playerAnimator.animator.GetBool("canDoCombo");
        isUsingLeftHand = playerAnimator.animator.GetBool("usingLeftHand");
        isUsingRightHand = playerAnimator.animator.GetBool("usingRightHand");
        playerAnimator.animator.SetBool("isInAir", isInAir);

        playerInput.TickInput(Time.deltaTime);
        playerMove.HandleRolling(Time.deltaTime);
        playerInteract.CheckInteractableObject(this);
    }

    void LateUpdate()
    {
        playerInput.rollFlag = false;
        playerInput.interactInput = false;
        playerInput.useItemInpt = false;
        playerInput.lockOnInput = false;
        playerInput.lightAttackInput = false;
        playerInput.heavyAttackInput = false;
        playerInput.gameSystemInput = false;
        playerInput.quickSlotUpInput = false;
        playerInput.quickSlotDownInput = false;
        playerInput.quickSlotLeftInput = false;
        playerInput.quickSlotRightInput = false;

        if (isInAir)
        {
            playerMove.inAirTimer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (playerCamera != null)
        {
            playerCamera.FollowTarget(Time.fixedDeltaTime);
            playerCamera.HandleCameraRotation(Time.fixedDeltaTime, playerInput.mouseX, playerInput.mouseY);
        }

        playerMove.HandleFalling(Time.fixedDeltaTime, playerMove.moveDirection);
        playerMove.HandleMovement(Time.fixedDeltaTime);
    }
}
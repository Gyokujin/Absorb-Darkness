using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

public class PlayerManager : CharacterManager
{
    private PlayerAnimatorData animatorData;

    [Header("Player Action")]
    public bool isInteracting;
    public bool isSprinting;
    public bool onAttack;
    public bool comboAble;
    public bool onDodge;
    public bool isInAir;
    public bool isGrounded;
    public bool isUsingLeftHand;
    public bool isUsingRightHand;

    [Header("Combat")]
    public int defaultLayer;
    public int invincibleLayer;

    [Header("Component")]
    [HideInInspector]
    public PlayerStatus playerStatus;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public PlayerMove playerMove;
    [HideInInspector]
    public PlayerInventory playerInventory;
    [HideInInspector]
    public PlayerAttacker playerAttacker;
    [HideInInspector]
    private PlayerBehavior playerBehavior;
    [HideInInspector]
    public PlayerItemUse playerItemUse;
    [HideInInspector]
    public PlayerAnimator playerAnimator;
    [HideInInspector]
    public PlayerAudio playerAudio;
    [HideInInspector]
    public PlayerItemSlotManager playerItemSlotManager;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        animatorData = new PlayerAnimatorData();

        playerStatus = GetComponent<PlayerStatus>();
        playerInput = GetComponent<PlayerInput>();
        playerMove = GetComponent<PlayerMove>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerBehavior = GetComponent<PlayerBehavior>();
        playerItemUse = GetComponent<PlayerItemUse>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerAudio = GetComponent<PlayerAudio>();
        playerItemSlotManager = GetComponentInChildren<PlayerItemSlotManager>();

        isGrounded = true;
        playerAnimator.Init();
        defaultLayer = LayerMask.NameToLayer("Player");
        invincibleLayer = LayerMask.NameToLayer("Invincible");
    }

    void Update()
    {
        isInteracting = playerAnimator.animator.GetBool("isInteracting");
        onAttack = playerAnimator.animator.GetBool("onAttack");
        comboAble = playerAnimator.animator.GetBool(animatorData.comboAbleParameter);
        isUsingLeftHand = playerAnimator.animator.GetBool("usingLeftHand");
        isUsingRightHand = playerAnimator.animator.GetBool("usingRightHand");
        playerAnimator.animator.SetBool("isInAir", isInAir);

        playerInput.TickInput();
        playerBehavior.CheckInteractableObject(this);
    }

    void LateUpdate()
    {
        playerInput.interactInput = false;
        playerInput.twoHandInput = false;
        playerInput.useItemInpt = false;
        playerInput.lockOnInput = false;
        playerInput.lightAttackInput = false;
        playerInput.heavyAttackInput = false;
        playerInput.gameSystemInput = false;
        playerInput.quickSlotUpInput = false;
        playerInput.quickSlotDownInput = false;
        playerInput.quickSlotLeftInput = false;
        playerInput.quickSlotRightInput = false;

        PlayerCamera.instance.FollowTarget(Time.fixedDeltaTime);
        PlayerCamera.instance.HandleCameraRotation(Time.fixedDeltaTime, playerInput.mouseX, playerInput.mouseY);

        if (isInAir)
        {
            playerMove.inAirTimer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        playerMove.HandleFalling(Time.fixedDeltaTime, playerMove.moveDirection);
        playerMove.HandleMovement(Time.fixedDeltaTime);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

public class PlayerManager : CharacterManager
{
    private PlayerPhysicsData layerData;
    private PlayerAnimatorData animatorData;

    [Header("Player Action")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isAttack;
    public bool isComboAble;
    public bool isDodge;
    public bool isInAir;
    public bool isGrounded = true;
    public bool isUsingLeftHand;
    public bool isUsingRightHand;
    public bool isItemUse;

    [Header("Combat")]
    [HideInInspector]
    public int defaultLayer;
    [HideInInspector]
    public int invincibleLayer;

    [Header("Component")]
    [HideInInspector]
    public PlayerStatus playerStatus;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public PlayerMove playerMove;
    [HideInInspector]
    public PlayerBehavior playerBehavior;
    [HideInInspector]
    public PlayerInventory playerInventory;
    [HideInInspector]
    public PlayerAttacker playerAttacker;
    [HideInInspector]
    public PlayerAudio playerAudio;
    [HideInInspector]
    public PlayerAnimator playerAnimator;
    [HideInInspector]
    public PlayerItemSlotManager playerItemSlotManager;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerStatus = GetComponent<PlayerStatus>();
        playerInput = GetComponent<PlayerInput>();
        playerMove = GetComponent<PlayerMove>();
        playerBehavior = GetComponent<PlayerBehavior>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerAudio = GetComponent<PlayerAudio>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerItemSlotManager = GetComponentInChildren<PlayerItemSlotManager>();

        layerData = new PlayerPhysicsData();
        animatorData = new PlayerAnimatorData();
        defaultLayer = LayerMask.NameToLayer(layerData.playerLayer);
        invincibleLayer = LayerMask.NameToLayer(layerData.invincibleLayer);
    }

    void Update()
    {
        isInteracting = playerAnimator.animator.GetBool(animatorData.interactParameter);
        isAttack = playerAnimator.animator.GetBool(animatorData.attackParameter);
        isComboAble = playerAnimator.animator.GetBool(animatorData.comboAbleParameter);
        isUsingLeftHand = playerAnimator.animator.GetBool(animatorData.onUsingLeftHand);
        isUsingRightHand = playerAnimator.animator.GetBool(animatorData.onUsingRightHand);
        isItemUse = playerAnimator.animator.GetBool(animatorData.isItemUseParameter);
        playerAnimator.animator.SetBool(animatorData.inAirParameter, isInAir);

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
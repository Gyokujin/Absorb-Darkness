using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterData;
using PlayerData;

public class PlayerManager : CharacterManager
{
    [Header("Data")]
    private CharacterAnimatorData characterAnimatorData;
    private PlayerPhysicsData layerData;
    private PlayerAnimatorData animatorData;

    [Header("Player Action")]
    public bool isInteracting;
    public bool isGrounded = true;
    public bool isDodge;
    public bool isSprinting;
    public bool isAttack;
    public bool isComboAble;
    public bool isInAir;
    public bool isItemUse;
    public bool isUsingLeftHand;
    public bool isUsingRightHand;

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
    public PlayerWeaponSlotManager playerItemSlotManager;

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
        playerItemSlotManager = GetComponentInChildren<PlayerWeaponSlotManager>();

        characterAnimatorData = new CharacterAnimatorData();
        layerData = new PlayerPhysicsData();
        animatorData = new PlayerAnimatorData();

        defaultLayer = LayerMask.NameToLayer(layerData.PlayerLayer);
        invincibleLayer = LayerMask.NameToLayer(layerData.InvincibleLayer);
    }

    void Update()
    {
        if (onDie)
            return;

        isInteracting = playerAnimator.animator.GetBool(characterAnimatorData.InteractParameter);
        isAttack = playerAnimator.animator.GetBool(animatorData.AttackParameter);
        isComboAble = playerAnimator.animator.GetBool(animatorData.ComboAbleParameter);
        isUsingLeftHand = playerAnimator.animator.GetBool(animatorData.OnUsingLeftHand);
        isUsingRightHand = playerAnimator.animator.GetBool(animatorData.OnUsingRightHand);
        isItemUse = playerAnimator.animator.GetBool(animatorData.IsItemUseParameter);
        playerAnimator.animator.SetBool(animatorData.InAirParameter, isInAir);

        playerInput.TickInput();
        playerBehavior.CheckInteractableObject();
    }

    void LateUpdate()
    {
        PlayerCamera.instance.FollowTarget(Time.fixedDeltaTime);
        PlayerCamera.instance.HandleCameraRotation(Time.fixedDeltaTime, playerInput.mouseX, playerInput.mouseY);

        if (isInAir)
        {
            playerMove.inAirTimer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        playerMove.HandleFalling(playerMove.moveDirection);
        playerMove.HandleMovement(Time.fixedDeltaTime);
    }
}
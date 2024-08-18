using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterData;
using PlayerData;
using SystemData;

public class PlayerManager : CharacterManager
{
    [Header("Data")]
    [HideInInspector]
    public CharacterAnimatorData characterAnimatorData;
    [HideInInspector]
    public PlayerStatusData playerStatusData;
    [HideInInspector]
    public PlayerPhysicsData playerPhysicsData;
    [HideInInspector]
    public InteractData interactData;
    [HideInInspector]
    public PhysicsData physicsData;
    [HideInInspector]
    public LayerData layerData;

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
    private string lastAttack;
    [HideInInspector]
    public int defaultLayer;
    [HideInInspector]
    public int invincibleLayer;
    private WeaponItem playerWeapon;

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
    public PlayerAudio playerAudio;
    [HideInInspector]
    public PlayerAnimator playerAnimator;
    [HideInInspector]
    public PlayerInventory playerInventory;
    [HideInInspector]
    public PlayerWeaponSlotManager playerWeaponSlotManager;

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
        playerAudio = GetComponent<PlayerAudio>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponentInChildren<PlayerInventory>();
        playerWeaponSlotManager = GetComponentInChildren<PlayerWeaponSlotManager>();

        defaultLayer = LayerMask.NameToLayer(layerData.PlayerLayer);
        invincibleLayer = LayerMask.NameToLayer(layerData.InvincibleLayer);
    }

    void Update()
    {
        if (onDie)
            return;

        isInteracting = playerAnimator.animator.GetBool(characterAnimatorData.InteractParameter);
        isAttack = playerAnimator.animator.GetBool(characterAnimatorData.AttackParameter);
        isComboAble = playerAnimator.animator.GetBool(characterAnimatorData.ComboAbleParameter);
        isUsingLeftHand = playerAnimator.animator.GetBool(characterAnimatorData.OnUsingLeftHand);
        isUsingRightHand = playerAnimator.animator.GetBool(characterAnimatorData.OnUsingRightHand);
        isItemUse = playerAnimator.animator.GetBool(characterAnimatorData.IsItemUseParameter);
        playerAnimator.animator.SetBool(characterAnimatorData.InAirParameter, isInAir);

        playerInput.TickInput();
        playerBehavior.CheckInteractableObject();
    }

    void LateUpdate()
    {
        PlayerCamera.instance.FollowTarget(Time.fixedDeltaTime);
        PlayerCamera.instance.HandleCameraRotation(Time.fixedDeltaTime, playerInput.mouseX, playerInput.mouseY);

        if (isInAir)
            playerMove.inAirTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        playerMove.HandleFalling(playerMove.moveDirection);
        playerMove.HandleMovement(Time.fixedDeltaTime);
    }

    public void HandleWeaponAttack(WeaponItem weapon, bool onLightAttack)
    {
        playerInput.sprintFlag = false;
        isSprinting = false;

        playerWeaponSlotManager.attackingWeapon = weapon;
        playerAnimator.animator.SetBool(characterAnimatorData.AttackParameter, true);
        playerAnimator.animator.SetBool(characterAnimatorData.OnUsingRightHand, true);
        playerWeapon = playerWeaponSlotManager.attackingWeapon;
        bool oneHand = !playerInput.twoHandFlag;

        switch (playerWeapon.weaponType)
        {
            case WeaponItem.WeaponType.None:
                break;

            case WeaponItem.WeaponType.Sword:

                if (onLightAttack)
                    lastAttack = oneHand ? weapon.oneHand_LightAttack1 : weapon.twoHand_LightAttack1;
                else
                    lastAttack = oneHand ? weapon.oneHand_HeavyAttack1 : weapon.twoHand_HeavyAttack1;
                break;

            case WeaponItem.WeaponType.Axe:
                break;
        }

        if (lastAttack != null)
        {
            playerAnimator.PlayTargetAnimation(lastAttack, true);
            playerAnimator.animator.SetBool(characterAnimatorData.AttackParameter, true);
        }
    }

    public void HandleWeaponCombo(WeaponItem weapon, bool onLightAttack)
    {
        playerAnimator.animator.SetBool(characterAnimatorData.ComboAbleParameter, false);
        bool oneHand = !playerInput.twoHandFlag;

        switch (playerWeapon.weaponType)
        {
            case WeaponItem.WeaponType.None:
            case WeaponItem.WeaponType.Axe:
                break;

            case WeaponItem.WeaponType.Sword:
                if (onLightAttack)
                    lastAttack = oneHand ? weapon.oneHand_LightAttack2 : weapon.twoHand_LightAttack2;
                else
                    lastAttack = oneHand ? weapon.oneHand_HeavyAttack2 : weapon.twoHand_HeavyAttack2;

                break;
        }

        if (lastAttack != null)
            playerAnimator.PlayTargetAnimation(lastAttack, true);
    }
}
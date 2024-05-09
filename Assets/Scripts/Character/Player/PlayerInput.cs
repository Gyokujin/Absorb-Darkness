using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Move & Action")]
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;
    private float rollInputTimer;

    [Header("Input")]
    private Vector2 movementInput;
    private Vector2 cameraInput;

    [Header("Input System")]
    [HideInInspector]
    public bool interactInput;
    private bool rollingInput;
    private bool twoHandInput;
    [HideInInspector]
    public bool lockOnInput;
    public bool useItemInpt;
    private bool rightStickLeftInput;
    private bool rightStickRightInput;
    [HideInInspector]
    public bool lightAttackInput;
    [HideInInspector]
    public bool heavyAttackInput;
    [HideInInspector]
    public bool gameSystemInput;
    [HideInInspector]
    public bool quickSlotUpInput;
    [HideInInspector]
    public bool quickSlotDownInput;
    [HideInInspector]
    public bool quickSlotLeftInput;
    [HideInInspector]
    public bool quickSlotRightInput;

    [Header("Action Flag")]
    public bool rollFlag;
    public bool sprintFlag;
    public bool twoHandFlag;
    public bool lockOnFlag;
    public bool comboFlag;
    public bool gameSystemFlag;

    [Header("Component")]
    private PlayerManager playerManager;
    private PlayerControls inputActions;
    private PlayerStatus playerStatus;
    private PlayerAnimator playerAnimator;
    private PlayerAttacker playerAttacker;
    private PlayerInventory playerInventory;
    private PlayerCamera playerCamera;
    private ItemSlotManager weaponSlotManager;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStatus = GetComponent<PlayerStatus>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        weaponSlotManager = GetComponentInChildren<ItemSlotManager>();
    }

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerActions.Interact.performed += i => interactInput = true;
            inputActions.PlayerActions.UseItem.performed += i => useItemInpt = true;
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerActions.LightAttack.performed += i => lightAttackInput = true;
            inputActions.PlayerActions.HeavyAttack.performed += i => heavyAttackInput = true;
            inputActions.PlayerActions.GameSystem.performed += i => gameSystemInput = true;
            inputActions.PlayerQuickSlots.QuickSlotLeft.performed += i => quickSlotLeftInput = true;
            inputActions.PlayerQuickSlots.QuickSlotRight.performed += i => quickSlotRightInput = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => rightStickLeftInput = true;
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => rightStickRightInput = true;
            inputActions.PlayerActions.TwoHand.performed += i => twoHandInput = true;
        }

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        HandleMoveInput(delta);
        HandleRollInput(delta);
        HandleUseItemInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleAttackInput(delta);
        HandleQuickSlotsInput();
        HandleGameSystemInput();
    }

    void HandleMoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    void HandleRollInput(float delta)
    {
        rollingInput = inputActions.PlayerActions.Rolling.phase == InputActionPhase.Performed;
        sprintFlag = rollingInput;

        if (rollingInput)
        {
            rollInputTimer += delta;
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f) // playerStatus.CurrentStamina >= playerStatus.actionLimitStamina
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    void HandleUseItemInput()
    {
        if (useItemInpt && !playerManager.isInteracting && moveAmount == 0)
        {
            playerManager.playerItemUse.UseItem(playerAnimator, playerInventory.curUsingItem);
        }
    }

    void HandleTwoHandInput()
    {
        if (twoHandInput)
        {
            twoHandInput = false;
            twoHandFlag = !twoHandFlag;

            if (twoHandFlag)
            {
                weaponSlotManager.LoadWeaponSlot(playerInventory.rightWeapon, false);
            }
            else
            {
                weaponSlotManager.LoadWeaponSlot(playerInventory.rightWeapon, false);
                weaponSlotManager.LoadWeaponSlot(playerInventory.leftWeapon, true);
            }
        }
    }

    void HandleAttackInput(float delta)
    {
        if (lightAttackInput && !gameSystemFlag && playerStatus.CurrentStamina >= playerStatus.actionLimitStamina)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else if (!playerManager.isInteracting && !playerManager.canDoCombo)
            {
                playerAnimator.animator.SetBool("usingRightHand", true);
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        if (heavyAttackInput && !playerManager.isInteracting)
        {
            playerAnimator.animator.SetBool("usingRightHand", true);
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }

    void HandleQuickSlotsInput()
    {
        if (quickSlotLeftInput)
        {
            playerInventory.ChangeLeftWeapon();
        }
        else if (quickSlotRightInput)
        {
            playerInventory.ChangeRightWeapon();
        }
    }

    void HandleGameSystemInput()
    {
        if (gameSystemInput)
        {
            if (!gameSystemFlag)
            {
                if (lockOnFlag)
                {
                    OffLockOn();
                }

                gameSystemFlag = true;
                UIManager.instance.OpenGameSystemUI();
            }
            else
            {
                gameSystemFlag = false;
                UIManager.instance.CloseGameSystemUI();
            }
        }
    }

    void HandleLockOnInput()
    {
        if (lockOnInput && !gameSystemFlag)
        {
            if (!lockOnFlag)
            {
                playerCamera.HandleLockOn();

                if (playerCamera.nearestLockOnTarget != null)
                {
                    lockOnFlag = true;
                    playerAnimator.animator.SetBool("onStance", true);
                    playerCamera.currentLockOnTarget = playerCamera.nearestLockOnTarget;
                }
            }
            else
            {
                OffLockOn();
            }
        }

        if (lockOnFlag && rightStickLeftInput)
        {
            rightStickLeftInput = false;
            playerCamera.HandleLockOn();

            if (playerCamera.leftLockTarget != null)
            {
                playerCamera.currentLockOnTarget = playerCamera.leftLockTarget;
            }
        }

        if (lockOnFlag && rightStickRightInput)
        {
            rightStickRightInput = false;
            playerCamera.HandleLockOn();

            if (playerCamera.rightLockTarget != null)
            {
                playerCamera.currentLockOnTarget = playerCamera.rightLockTarget;
            }
        }

        playerCamera.SetCameraHeight();
    }

    public void OffLockOn()
    {
        lockOnFlag = false;
        playerAnimator.animator.SetBool("onStance", false);
        playerCamera.ClearLockOnTargets();
    }
}
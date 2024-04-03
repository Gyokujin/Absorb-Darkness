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
    public float rollInputTimer;

    [Header("Input")]
    private Vector2 movementInput;
    private Vector2 cameraInput;

    [Header("Input System")]
    public bool interactInput;
    public bool rollingInput;
    public bool twoHandInput;
    public bool lockOnInput;
    public bool rightStickLeftInput;
    public bool rightStickRightInput;
    public bool lightAttackInput;
    public bool heavyAttackInput;
    public bool gameSystemInput;

    [Header("Action Flag")]
    public bool rollFlag;
    public bool sprintFlag;
    public bool twoHandFlag;
    public bool lockOnFlag;
    public bool comboFlag;
    public bool gameSystemFlag;

    [Header("Quick Slots")]
    public bool quickSlotUp;
    public bool quickSlotDown;
    public bool quickSlotLeft;
    public bool quickSlotRight;

    [Header("Component")]
    private PlayerControls inputActions;
    private PlayerAnimator playerAnimator;
    private PlayerAttacker playerAttacker;
    private PlayerInventory playerInventory;
    private PlayerManager playerManager;
    private PlayerCamera playerCamera;
    private WeaponSlotManager weaponSlotManager;
    private UIManager uiManager;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerActions.Interact.performed += i => interactInput = true;
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerActions.LightAttack.performed += i => lightAttackInput = true;
            inputActions.PlayerActions.HeavyAttack.performed += i => heavyAttackInput = true;
            inputActions.PlayerActions.GameSystem.performed += i => gameSystemInput = true;
            inputActions.PlayerQuickSlots.QuickSlotLeft.performed += i => quickSlotLeft = true;
            inputActions.PlayerQuickSlots.QuickSlotRight.performed += i => quickSlotRight = true;
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
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
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
        if (lightAttackInput && !gameSystemFlag)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else if (!playerManager.isInteracting && !playerManager.canDoCombo)
            {
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        if (heavyAttackInput && !playerManager.isInteracting)
        {
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }

    void HandleQuickSlotsInput()
    {
        if (quickSlotLeft)
        {
            playerInventory.ChangeLeftWeapon();
        }
        else if (quickSlotRight)
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
                gameSystemFlag = true;
                uiManager.OpenGameSystemUI();
                playerCamera.ClearLockOnTargets();
            }
            else
            {
                gameSystemFlag = false;
                uiManager.CloseGameSystemUI();
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
                lockOnFlag = false;
                playerAnimator.animator.SetBool("onStance", false);
                playerCamera.ClearLockOnTargets();
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Move")]
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    [Header("Action")]
    public bool a_Input;
    public bool b_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool jump_Input;
    public bool inventory_Input;

    public bool rollFlag;
    public bool sprintFlag;
    public float rollInputTimer;
    public bool comboFlag;
    public bool inventoryFlag;

    [Header("Quick Slots")]
    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;

    [Header("Input")]
    private Vector2 movementInput;
    private Vector2 cameraInput;

    [Header("Component")]
    private PlayerControls inputActions;
    private PlayerAttacker playerAttacker;
    private PlayerInventory playerInventory;
    private PlayerManager playerManager;
    private UIManager uiManager;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleInteractInput();
        HandleRollInput(delta);
        HandleJumpInput();
        HandleAttackInput(delta);
        HandleQuickSlotsInput();
        HandleInteractInput();
        HandleInverntoryInput();
    }

    void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    void HandleInteractInput()
    {
        inputActions.PlayerActions.A.performed += i => a_Input = true;
    }

    void HandleRollInput(float delta)
    {
        b_Input = inputActions.PlayerActions.B.phase == InputActionPhase.Performed;

        if (b_Input)
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
    
    void HandleJumpInput()
    {
        inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
    }

    void HandleAttackInput(float delta)
    {
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;

        if (rb_Input)
        {
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting || playerManager.canDoCombo)
                    return;

                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        if (rt_Input)
        {
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }
    }

    void HandleQuickSlotsInput()
    {
        inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
        inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;

        if (d_Pad_Left)
        {
            playerInventory.ChangeLeftWeapon();
        }
        else if (d_Pad_Right)
        {
            playerInventory.ChangeRightWeapon();
        }
    }

    void HandleInverntoryInput()
    {
        inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;

        if (inventory_Input)
        {
            inventoryFlag = !inventoryFlag;

            if (inventoryFlag)
            {
                uiManager.ControlSelectWindow(true);
            }
            else
            {
                uiManager.ControlSelectWindow(false);
            }
        }
    }
}
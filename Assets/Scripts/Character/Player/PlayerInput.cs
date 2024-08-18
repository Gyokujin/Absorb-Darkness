using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerManager player;
    private PlayerControls inputActions;

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

    [Header("Input Action")]
    private bool rollingInput;
    private bool lightAttackInput;
    private bool heavyAttackInput;
    private bool interactInput;
    private bool twoHandInput;
    private bool lockOnInput;
    private bool useItemInpt;

    [Header("Input System")]
    // private bool quickSlotUpInput;
    // private bool quickSlotDownInput;
    private bool quickSlotLeftInput;
    private bool quickSlotRightInput;
    private bool gameSystemInput;

    [Header("Action Flag")]
    public bool rollFlag;
    public bool sprintFlag;
    public bool twoHandFlag;
    public bool gameSystemFlag;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
    }

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();

            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerActions.LightAttack.performed += i => lightAttackInput = true;
            inputActions.PlayerActions.HeavyAttack.performed += i => heavyAttackInput = true;
            inputActions.PlayerActions.Interact.performed += i => interactInput = true;
            inputActions.PlayerActions.TwoHand.performed += i => twoHandInput = true;
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerActions.UseItem.performed += i => useItemInpt = true;
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            inputActions.PlayerQuickSlots.QuickSlotLeft.performed += i => quickSlotLeftInput = true;
            inputActions.PlayerQuickSlots.QuickSlotRight.performed += i => quickSlotRightInput = true;
            inputActions.PlayerActions.GameSystem.performed += i => gameSystemInput = true;
        }

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void LateUpdate()
    {
        lightAttackInput = false;
        heavyAttackInput = false;
        interactInput = false;
        twoHandInput = false;
        lockOnInput = false;
        useItemInpt = false;

        // quickSlotUpInput = false;
        // quickSlotDownInput = false;
        quickSlotLeftInput = false;
        quickSlotRightInput = false;
        gameSystemInput = false;
    }

    public void TickInput()
    {
        HandleMoveInput();
        HandleAttackInput();
        HandleGameSystemInput();

        if (!player.isInteracting)
        {
            HandleRollInput();
            HandleInteractInput();
            HandleTwoHandInput();
            HandleLockOnInput();
            HandleUseItemInput();
            HandleQuickSlotsInput();
        }
    }

    void HandleMoveInput()
    {
        if (gameSystemFlag)
            return;

        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    void HandleRollInput()
    {
        if (gameSystemFlag)
            return;

        rollingInput = inputActions.PlayerActions.Rolling.phase == InputActionPhase.Performed;

        if (rollingInput)
        {
            rollInputTimer += Time.deltaTime;

            if (rollInputTimer > player.characterAnimatorData.RunAnimationCondition)
                sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < player.characterAnimatorData.RunAnimationCondition)
            {
                rollFlag = true;
                player.playerMove.HandleRolling();
            }

            sprintFlag = false;
            rollInputTimer = 0;
        }
    }

    void HandleInteractInput()
    {
        if (gameSystemFlag)
            return;

        if (interactInput)
            player.playerBehavior.BehaviourAction();
    }

    void HandleAttackInput()
    {
        if (gameSystemFlag || player.onDamage || (player.isAttack && !player.isComboAble) || player.playerStatus.CurrentStamina < player.playerStatusData.ActionLimitStamina || 
            player.isDodge || player.isItemUse)
            return;

        if (lightAttackInput) // 약공격
        {
            if (!player.isComboAble)
                player.playerCombat.HandleWeaponAttack(player.playerInventory.curRightWeapon, true);
            else // 콤보 공격
                player.playerCombat.HandleWeaponCombo(player.playerInventory.curRightWeapon, true);
        }
        else if (heavyAttackInput) // 강공격
        {
            if (!player.isComboAble)
                player.playerCombat.HandleWeaponAttack(player.playerInventory.curRightWeapon, false);
            else // 콤보 공격
                player.playerCombat.HandleWeaponCombo(player.playerInventory.curRightWeapon, false);
        }
    }

    void HandleTwoHandInput()
    {
        if (gameSystemFlag)
            return;

        if (twoHandInput)
        {
            twoHandFlag = !twoHandFlag;

            if (twoHandFlag)
                player.playerInventory.LoadWeaponSlot(player.playerInventory.curRightWeapon, false);
            else
            {
                player.playerInventory.LoadWeaponSlot(player.playerInventory.curRightWeapon, false);
                player.playerInventory.LoadWeaponSlot(player.playerInventory.curLeftWeapon, true);
            }
        }
    }

    void HandleLockOnInput()
    {
        if (gameSystemFlag)
            return;

        if (lockOnInput && !gameSystemFlag)
            PlayerCamera.instance.SwitchLockOn();
    }

    void HandleUseItemInput()
    {
        if (gameSystemFlag)
            return;

        if (useItemInpt)
            player.playerBehavior.UseItem(player.playerInventory.curUsingItem);
    }

    void HandleQuickSlotsInput()
    {
        if (gameSystemFlag)
            return;

        if (quickSlotLeftInput)
            player.playerInventory.ChangeLeftWeapon();
        else if (quickSlotRightInput)
            player.playerInventory.ChangeRightWeapon();
    }

    void HandleGameSystemInput()
    {
        if (gameSystemInput)
        {
            if (!gameSystemFlag)
            {
                if (PlayerCamera.instance.isLockOn)
                    PlayerCamera.instance.SwitchLockOn();

                gameSystemFlag = true;
                UIManager.instance.OpenGameUI();
            }
            else
            {
                gameSystemFlag = false;
                UIManager.instance.CloseGameUI();
            }
        }
    }
}
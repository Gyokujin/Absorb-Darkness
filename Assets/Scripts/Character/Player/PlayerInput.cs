using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerData;

public class PlayerInput : MonoBehaviour
{
    private PlayerManager player;
    private PlayerControls inputActions;

    [Header("Data")]
    private PlayerStatusData playerStatusData;
    private PlayerAnimatorData playerAnimatorData;

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
    [HideInInspector]
    public bool twoHandInput;
    [HideInInspector]
    public bool lockOnInput;
    [HideInInspector]
    public bool useItemInpt;
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
    public bool gameSystemFlag;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
        playerStatusData = new PlayerStatusData();
        playerAnimatorData = new PlayerAnimatorData();
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
            inputActions.PlayerActions.TwoHand.performed += i => twoHandInput = true;
        }

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
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
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    void HandleRollInput()
    {
        rollingInput = inputActions.PlayerActions.Rolling.phase == InputActionPhase.Performed;

        if (rollingInput)
        {
            rollInputTimer += Time.deltaTime;

            if (rollInputTimer > playerAnimatorData.RunAnimationCondition)
            {
                sprintFlag = true;
            }
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < playerAnimatorData.RunAnimationCondition)
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
        if (interactInput)
        {
            player.playerBehavior.BehaviourAction();
        }
    }

    void HandleAttackInput()
    {
        if (gameSystemFlag || player.onDamage || player.onDie || (player.isAttack && !player.isComboAble) || player.playerStatus.CurrentStamina < playerStatusData.ActionLimitStamina || player.isDodge || player.isItemUse)
            return;

        if (lightAttackInput) // 약공격
        {
            if (!player.isComboAble)
            {
                player.playerAttacker.HandleWeaponAttack(player.playerInventory.rightWeapon, true);
            }
            else // 콤보 공격
            {
                player.playerAttacker.HandleWeaponCombo(player.playerInventory.rightWeapon, true);
            }
        }
        else if (heavyAttackInput) // 강공격
        {
            if (!player.isComboAble)
            {
                player.playerAttacker.HandleWeaponAttack(player.playerInventory.rightWeapon, false);
            }
            else // 콤보 공격
            {
                player.playerAttacker.HandleWeaponCombo(player.playerInventory.rightWeapon, false);
            }
        }
    }

    void HandleTwoHandInput()
    {
        if (twoHandInput)
        {
            twoHandFlag = !twoHandFlag;

            if (twoHandFlag)
            {
                player.playerItemSlotManager.LoadWeaponSlot(player.playerInventory.rightWeapon, false);
            }
            else
            {
                player.playerItemSlotManager.LoadWeaponSlot(player.playerInventory.rightWeapon, false);
                player.playerItemSlotManager.LoadWeaponSlot(player.playerInventory.leftWeapon, true);
            }
        }
    }

    void HandleLockOnInput()
    {
        if (lockOnInput && !gameSystemFlag)
        {
            PlayerCamera.instance.SwitchLockOn();
        }
    }

    void HandleUseItemInput()
    {
        if (useItemInpt)
        {
            player.playerBehavior.UseItem(player.playerAnimator, player.playerInventory.curUsingItem);
        }
    }

    void HandleQuickSlotsInput()
    {
        if (quickSlotLeftInput)
        {
            player.playerInventory.ChangeLeftWeapon();
        }
        else if (quickSlotRightInput)
        {
            player.playerInventory.ChangeRightWeapon();
        }
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
                UIManager.instance.OpenGameSystemUI();
            }
            else
            {
                gameSystemFlag = false;
                UIManager.instance.CloseGameSystemUI();
            }
        }
    }
}
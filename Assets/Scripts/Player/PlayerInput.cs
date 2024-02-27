using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool b_Input;

    public bool rollFlag;
    public bool sprintFlag;
    public float rollInputTimer;
    public bool isInteracting;

    [Header("Input")]
    private Vector2 movementInput;
    private Vector2 cameraInput;

    [Header("Component")]
    private PlayerControls inputActions;
    private PlayerCamera playerCamera;

    void Start()
    {
        Init();
    }

    void Init()
    {
        playerCamera = PlayerCamera.instance;
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

    void FixedUpdate()
    {
        if (playerCamera != null)
        {
            playerCamera.FollowTarget(Time.fixedDeltaTime);
            
        }
    }

    void LateUpdate()
    {

        if (playerCamera != null)
        {
            playerCamera.HandleCameraRotation(Time.fixedDeltaTime, mouseX, mouseY);
        }
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
    }

    void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    void HandleRollInput(float delta)
    {
        // b_Input = inputActions.PlayerActions.Roll.triggered;
        b_Input = inputActions.PlayerActions.Roll.phase == InputActionPhase.Performed;

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
}
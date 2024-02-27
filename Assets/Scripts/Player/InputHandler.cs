using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool inputAble;
    public bool rollFlag;
    public bool isInteracting;

    [Header("Input")]
    private Vector2 movementInput;
    private Vector2 cameraInput;

    [Header("Component")]
    private PlayerControls inputActions;
    private CameraHandler cameraHandler;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        cameraHandler = CameraHandler.instance;
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
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(Time.fixedDeltaTime);
            
        }
    }

    void LateUpdate()
    {
        if (cameraHandler != null)
        {
            cameraHandler.HandleCameraRotation(Time.fixedDeltaTime, mouseX, mouseY);
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
        inputAble = inputActions.PlayerActions.Roll.triggered;

        if (inputAble)
        {
            rollFlag = true;
        }
    }
}
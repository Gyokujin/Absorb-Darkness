using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Player Action")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isUsingLeftHand;
    public bool isUsingRightHand;

    [Header("Interact")]
    [SerializeField]
    private float checkRadius = 0.3f;
    [SerializeField]
    private float checkMaxDis = 1f;
    [SerializeField]
    private GameObject interactableUIObj;
    public GameObject itemInteractableObj;

    [Header("Component")]
    [HideInInspector]
    public PlayerMove playerMove;
    private PlayerInput playerInput;
    [HideInInspector]
    public PlayerAnimator playerAnimator;
    [HideInInspector]
    public PlayerInventory playerInventory;
    private PlayerCamera playerCamera;
    private InteractableUI interactableUI;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerMove = GetComponent<PlayerMove>();
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInventory = GetComponent<PlayerInventory>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        isInteracting = playerAnimator.animator.GetBool("isInteracting");
        canDoCombo = playerAnimator.animator.GetBool("canDoCombo");
        isUsingLeftHand = playerAnimator.animator.GetBool("usingLeftHand");
        isUsingRightHand = playerAnimator.animator.GetBool("usingRightHand");
        playerAnimator.animator.SetBool("isInAir", isInAir);

        playerInput.TickInput(Time.deltaTime);
        playerMove.HandleRollingAndSprinting(Time.deltaTime);
        CheckInteractableObject();
    }

    void LateUpdate()
    {
        playerInput.rollFlag = false;
        playerInput.interactInput = false;
        playerInput.lockOnInput = false;
        playerInput.lightAttackInput = false;
        playerInput.heavyAttackInput = false;
        playerInput.gameSystemInput = false;
        playerInput.quickSlotUpInput = false;
        playerInput.quickSlotDownInput = false;
        playerInput.quickSlotLeftInput = false;
        playerInput.quickSlotRightInput = false;

        if (playerCamera != null)
        {
            playerCamera.FollowTarget(Time.deltaTime);
            playerCamera.HandleCameraRotation(Time.deltaTime, playerInput.mouseX, playerInput.mouseY);
        }

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

    public void CheckInteractableObject()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, checkRadius, transform.forward, out hit, checkMaxDis, playerCamera.targetLayer))
        {
            if (hit.collider.tag == "Interactable" && hit.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactableObj = hit.collider.GetComponent<Interactable>();

                string interactableText = interactableObj.interactableText;
                interactableUI.interactableText.text = interactableText;
                interactableUIObj.SetActive(true);

                if (playerInput.interactInput)
                {
                    interactableObj.Interact(this);
                    UIManager.instance.UpdateUI();
                }
            }
        }
        else
        {
            if (interactableUIObj != null)
            {
                interactableUIObj.SetActive(false);

                if (playerInput.interactInput)
                {
                    itemInteractableObj.SetActive(false);
                }
            }
        }
    }
}
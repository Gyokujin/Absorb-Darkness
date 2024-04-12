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

    [Header("Interact")]
    [SerializeField]
    private float checkRadius = 0.3f;
    [SerializeField]
    private float checkMaxDis = 1f;
    [SerializeField]
    private GameObject interactableUIObj;
    public GameObject itemInteractableObj;

    [Header("Component")]
    private PlayerInput playerInput;
    private PlayerMove playerMove;
    private PlayerCamera playerCamera;
    private PlayerAnimator playerAnimator;
    private InteractableUI interactableUI;

    void Awake()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerInput = GetComponent<PlayerInput>();
        playerMove = GetComponent<PlayerMove>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        canDoCombo = playerAnimator.animator.GetBool("canDoCombo");
        isInteracting = playerAnimator.animator.GetBool("isInteracting");
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
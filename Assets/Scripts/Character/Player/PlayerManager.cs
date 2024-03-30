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
    private Animator animator;
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
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerMove = GetComponent<PlayerMove>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        canDoCombo = animator.GetBool("canDoCombo");
        isInteracting = animator.GetBool("isInteracting");
        animator.SetBool("isInAir", isInAir);
        playerInput.TickInput(Time.deltaTime);
        playerMove.HandleRollingAndSprinting(Time.deltaTime);
        CheckInteractableObject();
    }

    void LateUpdate()
    {
        playerInput.rollFlag = false;
        playerInput.interactInput = false;
        playerInput.lockOnInput = false; // 이후에 록온 문제 생기면 지운다
        playerInput.lightAttackInput = false;
        playerInput.heavyAttackInput = false;
        playerInput.gameSystemInput = false;
        playerInput.quickSlotUp = false;
        playerInput.quickSlotDown = false;
        playerInput.quickSlotLeft = false;
        playerInput.quickSlotRight = false;

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

        if (Physics.SphereCast(transform.position, checkRadius, transform.forward, out hit, checkMaxDis, playerCamera.layerMask))
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
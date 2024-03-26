using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
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
        playerMove.HandleJumping();

        CheckInteractableObject();
    }

    void LateUpdate()
    {
        playerInput.rollFlag = false;
        playerInput.a_Input = false;
        playerInput.rb_Input = false;
        playerInput.rt_Input = false;
        playerInput.d_Pad_Up = false;
        playerInput.d_Pad_Down = false;
        playerInput.d_Pad_Left = false;
        playerInput.d_Pad_Right = false;
        playerInput.jump_Input = false;
        playerInput.inventory_Input = false;

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

                if (playerInput.a_Input)
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

                if (playerInput.a_Input)
                {
                    itemInteractableObj.SetActive(false);
                }
            }
        }
    }
}
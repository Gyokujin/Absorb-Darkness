using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Status")]
    [SerializeField]
    private float walkSpeed = 3;
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float sprintSpeed = 7;
    [SerializeField]
    private float rotationSpeed = 10;
    [SerializeField]
    private float fallingDownSpeed = 45;
    [SerializeField]
    private float fallingFrontSpeed = 6f;

    [Header("Ground & Air Detection States")]
    [SerializeField]
    private float groundCheckDis = 0.4f;
    [SerializeField]
    private float groundDetectionRayStart = 0.5f;
    [SerializeField]
    private float distanceBeginFallMin = 1f;
    [SerializeField]
    private float groundDirRayDistance = 0.2f;
    public LayerMask ignoreGroundCheck;
    public float inAirTimer;

    [Header("Physics")]
    private Transform playerTransform;
    [HideInInspector]
    public new Rigidbody rigidbody;
    public Vector3 moveDirection;
    private Vector3 normalVec;
    private Vector3 targetPosition;

    [Header("Camera")]
    private Transform cameraPos;
    [SerializeField]
    private GameObject playerCamera;

    [Header("Component")]
    private PlayerManager playerManager;
    private PlayerInput playerInput;
    private PlayerAnimator playerAnimator;

    void Start()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerManager = GetComponent<PlayerManager>();
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();

        cameraPos = playerCamera.transform;
        playerTransform = transform;
        playerAnimator.Init();
        playerManager.isGrounded = true;
    }

    public void HandleMovement(float delta)
    {
        if (playerInput.rollFlag || playerManager.isInteracting)
            return;

        // Ű �Է¿� ���� ���� ���͸� ���Ѵ�.
        moveDirection = cameraPos.forward * playerInput.vertical;
        moveDirection += cameraPos.right * playerInput.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // �ش� ���⿡ ���ǵ常ŭ rigidbody �̵���Ų��.
        float speed = moveSpeed;
        
        if (playerInput.sprintFlag && playerInput.moveAmount > 0.5f) // sprintFlag�� Ȱ��ȭ �Ǿ� ���� ������ �⺻�ӵ�. �Ǿ� ������ �޸��� �ӵ��� ����
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            if (playerInput.moveAmount < 0.5f)
            {
                moveDirection *= walkSpeed;
                playerManager.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }
        }
        
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVec);
        rigidbody.velocity = projectedVelocity;

        // �ִϸ��̼� ����
        playerAnimator.AnimatorValue(playerInput.moveAmount, 0, playerManager.isSprinting);

        // ȸ���� ������ ��쿡�� �̵� �������� ĳ���͸� ȸ���Ѵ�.
        if (playerAnimator.canRotate)
        {
            HandleRotation(delta);
        }
    }

    void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = playerInput.moveAmount;

        targetDir = cameraPos.forward * playerInput.vertical;
        targetDir += cameraPos.right * playerInput.horizontal;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
            targetDir = playerTransform.forward;

        float rs = rotationSpeed;
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(playerTransform.rotation, tr, rs * delta);
        playerTransform.rotation = targetRotation;
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (playerAnimator.animator.GetBool("isInteracting")) // ���� �÷��̾ �ൿ ������ ���� ���� ����
            return;

        if (playerInput.rollFlag)
        {
            moveDirection = cameraPos.forward * playerInput.vertical;
            moveDirection += cameraPos.right * playerInput.horizontal;

            if (playerInput.moveAmount > 0) // �̵��߿� ȸ��Ű�� ������ ������
            {
                playerAnimator.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                playerTransform.rotation = rollRotation;
            }
            else // ȸ��Ű�� ������ ������ �齺��
            {
                playerAnimator.PlayTargetAnimation("Backstep", true);
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = playerTransform.position;
        origin.y += groundDetectionRayStart;

        if (Physics.Raycast(origin, playerTransform.forward, out hit, groundCheckDis))
        {
            moveDirection = Vector3.zero;
        }

        if (playerManager.isInAir)
        {
            rigidbody.AddForce(Vector3.down * fallingDownSpeed); // �Ʒ� ����
            rigidbody.AddForce(moveDirection * fallingDownSpeed / fallingFrontSpeed); // ���� ������ ���� ������ �̵�
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirRayDistance;
        targetPosition = playerTransform.position;

        Debug.DrawRay(origin, Vector3.down * distanceBeginFallMin, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, Vector3.down, out hit, distanceBeginFallMin, ignoreGroundCheck))
        {
            normalVec = hit.normal;
            Vector3 transform = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = transform.y;

            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    playerAnimator.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    playerAnimator.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                playerManager.isInAir = false;
            }
        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if (!playerManager.isInAir)
            {
                if (!playerManager.isInteracting)
                {
                    playerAnimator.PlayTargetAnimation("Falling", true);
                }

                Vector3 velocity = rigidbody.velocity;
                velocity.Normalize();
                rigidbody.velocity = velocity * (moveSpeed / 2);
                playerManager.isInAir = true;
            }
        }

        if (playerManager.isInteracting || playerInput.moveAmount > 0)
        {
            playerTransform.position = Vector3.Lerp(playerTransform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            playerTransform.position = targetPosition;
        }

        if (playerManager.isGrounded)
        {
            if (playerManager.isInteracting || playerInput.moveAmount > 0)
            {
                playerTransform.position = Vector3.Lerp(playerTransform.position, targetPosition, Time.deltaTime);
            }
            else
            {
                playerTransform.position = targetPosition;
            }
        }
    }

    public void HandleJumping()
    {
        if (playerManager.isInteracting)
            return;

        if (playerInput.jump_Input)
        {
            if (playerInput.moveAmount > 0)
            {
                moveDirection = cameraPos.forward * playerInput.vertical;
                moveDirection += cameraPos.right * playerInput.horizontal;
                playerAnimator.PlayTargetAnimation("Jump", true);
                
                moveDirection.y = 0;
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                playerTransform.rotation = jumpRotation;
            }
        }
    }
}
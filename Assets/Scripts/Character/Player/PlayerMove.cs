using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Ground & Air Detection States")]
    [SerializeField]
    private float groundCheckDis = 0.4f;
    [SerializeField]
    private float groundDetectionRayStart = 0.5f;
    [SerializeField]
    private float distanceBeginFallMin = 1f;
    [SerializeField]
    private float groundDirRayDistance = 0.2f;
    [SerializeField]
    private LayerMask ignoreGroundCheck;
    public float inAirTimer;

    [Header("Physics")]
    public new Rigidbody rigidbody;
    public Vector3 moveDirection;
    private Vector3 normalVec;
    private Vector3 targetPosition;
    [SerializeField]
    private float fallingFactor = 0.1f;
    [SerializeField]
    private float fallingSpeedRatio = 2;
    [SerializeField]
    private float fallingDownForce = 750;
    [SerializeField]
    private float fallingFrontForce = 5f;

    [Header("Component")]
    private Transform playerTransform;
    [SerializeField]
    private Collider playerCollider;
    [SerializeField]
    private Collider playerBlockerCollider;

    private PlayerManager playerManager;
    private PlayerInput playerInput;
    private PlayerStatus playerStatus;
    private PlayerAnimator playerAnimator;
    private PlayerCamera playerCamera;

    void Start()
    {
        Init();
    }

    void Init()
    {
        playerManager = GetComponent<PlayerManager>();
        playerInput = GetComponent<PlayerInput>();
        playerStatus = GetComponent<PlayerStatus>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerCamera = FindObjectOfType<PlayerCamera>();

        playerTransform = transform;
        playerAnimator.Init();
        playerManager.isGrounded = true;
        Physics.IgnoreCollision(playerCollider, playerBlockerCollider, true);
    }

    public void HandleMovement(float delta)
    {
        if (playerInput.rollFlag || playerManager.isInteracting)
            return;

        // Ű �Է¿� ���� ���� ���͸� ���Ѵ�.
        moveDirection = playerCamera.transform.forward * playerInput.vertical;
        moveDirection += playerCamera.transform.right * playerInput.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // �ش� ���⿡ ���ǵ常ŭ rigidbody �̵���Ų��.
        float speed = playerStatus.runSpeed;
        
        if (playerInput.sprintFlag && playerInput.moveAmount > 0.5f) // sprintFlag�� Ȱ��ȭ �Ǿ� ���� ������ �⺻�ӵ�. �Ǿ� ������ �޸��� �ӵ��� ����
        {
            playerManager.isSprinting = true;
            moveDirection *= playerStatus.sprintSpeed;
        }
        else if (playerInput.moveAmount < 0.5f)
        {
            moveDirection *= playerStatus.walkSpeed;
            playerManager.isSprinting = false;
        }
        else
        {
            moveDirection *= speed;
            playerManager.isSprinting = false;
        }
        
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVec);
        rigidbody.velocity = projectedVelocity;

        // �ִϸ��̼� ����
        if (playerInput.lockOnFlag && !playerInput.sprintFlag)
        {
            playerAnimator.AnimatorValue(playerInput.vertical, playerInput.horizontal, playerManager.isSprinting);
        }
        else
        {
            playerAnimator.AnimatorValue(playerInput.moveAmount, 0, playerManager.isSprinting);
        }

        // ȸ���� ������ ��쿡�� �̵� �������� ĳ���͸� ȸ���Ѵ�.
        if (playerAnimator.canRotate)
        {
            HandleRotation(delta);
        }
    }

    void HandleRotation(float delta)
    {
        if (playerInput.lockOnFlag)
        {
            if (playerInput.sprintFlag || playerInput.rollFlag)
            {
                Vector3 targetDir = playerCamera.cameraTransform.forward * playerInput.vertical;
                targetDir += playerCamera.cameraTransform.right * playerInput.horizontal;
                targetDir.Normalize();
                targetDir.y = 0;

                if (targetDir == Vector3.zero)
                {
                    targetDir = transform.forward;
                }

                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, playerStatus.rotationSpeed * Time.deltaTime);
                transform.rotation = targetRotation;
            }
            else
            {
                Vector3 rotationDir = moveDirection;
                rotationDir = playerCamera.currentLockOnTarget.position - transform.position;
                rotationDir.y = 0;
                rotationDir.Normalize();
                
                Quaternion tr = Quaternion.LookRotation(rotationDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, playerStatus.rotationSpeed * Time.deltaTime);
                transform.rotation = targetRotation;
            }
        }
        else
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = playerInput.moveAmount;

            targetDir = playerCamera.cameraTransform.forward * playerInput.vertical;
            targetDir += playerCamera.cameraTransform.right * playerInput.horizontal;
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = playerTransform.forward;

            float rs = playerStatus.rotationSpeed;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(playerTransform.rotation, tr, rs * delta);
            playerTransform.rotation = targetRotation;
        }
    }

    public void HandleRollingAndSprinting(float delta)
    {
        if (playerAnimator.animator.GetBool("isInteracting")) // ���� �÷��̾ �ൿ ������ ���� ���� ����
            return;

        if (playerInput.rollFlag)
        {
            playerManager.onDodge = true;
            moveDirection = playerCamera.transform.forward * playerInput.vertical;
            moveDirection += playerCamera.transform.right * playerInput.horizontal;

            if (playerInput.moveAmount > 0) // �̵��߿� ȸ��Ű�� ������ ������
            {
                playerAnimator.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                playerTransform.rotation = rollRotation;
            }
            else // �̵�Ű�� ������ ������ �齺��
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
            rigidbody.AddForce(Vector3.down * fallingDownForce); // �Ʒ� ����
            rigidbody.AddForce(moveDirection * fallingDownForce / fallingFrontForce); // ���� ������ ���� ������ �̵�
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
                if (inAirTimer > 0.5f) // ���� �ð��� 0.5�� �̻��϶��� Land �ִϸ��̼��� �����Ѵ�.
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
                rigidbody.velocity = velocity * (playerStatus.runSpeed / fallingSpeedRatio);
                playerManager.isInAir = true;
            }
        }

        if (playerManager.isInteracting || playerInput.moveAmount > 0)
        {
            playerTransform.position = Vector3.Lerp(playerTransform.position, targetPosition, Time.deltaTime / fallingFactor);
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
}
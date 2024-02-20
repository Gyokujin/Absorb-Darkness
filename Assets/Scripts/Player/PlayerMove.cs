using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class PlayerMove : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float rotationSpeed = 10;

    [Header("Physics")]
    private Transform playerTransform;
    private Rigidbody rigidbody; // new ���� �䱸
    private Vector3 moveDirection;
    private Vector3 normalVec;
    private Vector3 targetPosition;

    [Header("Camera")]
    private Transform cameraPos;
    [SerializeField]
    private GameObject playerCamera;

    [Header("Component")]
    private InputHandler inputHandler;
    private PlayerAnimator playerAnimator;

    void Start()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();

        cameraPos = playerCamera.transform;
        playerTransform = transform;
        playerAnimator.Init();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        inputHandler.TickInput(delta);
        HandleMovement(delta);
        HandleRollingAndSprinting(delta);
    }

    public void HandleMovement(float delta)
    {
        // Ű �Է¿� ���� ���� ���͸� ���Ѵ�.
        moveDirection = cameraPos.forward * inputHandler.vertical;
        moveDirection += cameraPos.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // �ش� ���⿡ ���ǵ常ŭ rigidbody �̵���Ų��.
        float speed = moveSpeed;
        moveDirection *= speed;
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVec);
        rigidbody.velocity = projectedVelocity;

        // �ִϸ��̼� ����
        playerAnimator.AnimatorValue(inputHandler.moveAmount, 0);

        // ȸ���� ������ ��쿡�� �̵� �������� ĳ���͸� ȸ���Ѵ�.
        if (playerAnimator.canRotate)
        {
            HandleRotation(delta);
        }
    }

    void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraPos.forward * inputHandler.vertical;
        targetDir += cameraPos.right * inputHandler.horizontal;
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

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraPos.forward * inputHandler.vertical;
            moveDirection += cameraPos.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0) // �̵��߿� ȸ��Ű�� ������ ������
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
}
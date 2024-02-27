using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMove : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float sprintSpeed = 7;
    [SerializeField]
    private float rotationSpeed = 10;

    [HideInInspector]
    public bool isSprinting;

    [Header("Physics")]
    private Transform playerTransform;
    [HideInInspector]
    public Rigidbody rigidbody; // new ���� �䱸
    private Vector3 moveDirection;
    private Vector3 normalVec;
    private Vector3 targetPosition;

    [Header("Camera")]
    private Transform cameraPos;
    [SerializeField]
    private GameObject playerCamera;

    [Header("Component")]
    private PlayerInput playerInput;
    private PlayerAnimator playerAnimator;

    void Start()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();

        cameraPos = playerCamera.transform;
        playerTransform = transform;
        playerAnimator.Init();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        isSprinting = playerInput.b_Input;
        playerInput.TickInput(delta);
        HandleMovement(delta);
        HandleRollingAndSprinting(delta);
    }

    public void HandleMovement(float delta)
    {
        if (playerInput.rollFlag)
            return;

        // Ű �Է¿� ���� ���� ���͸� ���Ѵ�.
        moveDirection = cameraPos.forward * playerInput.vertical;
        moveDirection += cameraPos.right * playerInput.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // �ش� ���⿡ ���ǵ常ŭ rigidbody �̵���Ų��.
        float speed = moveSpeed;
        
        if (playerInput.sprintFlag) // sprintFlag�� Ȱ��ȭ �Ǿ� ���� ������ �⺻�ӵ�. �Ǿ� ������ �޸��� �ӵ��� ����
        {
            speed = sprintSpeed;
            isSprinting = true;
            moveDirection *= speed;
        }
        else
        {
            moveDirection *= speed;
        }
        
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVec);
        rigidbody.velocity = projectedVelocity;

        // �ִϸ��̼� ����
        playerAnimator.AnimatorValue(playerInput.moveAmount, 0, isSprinting);

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
}
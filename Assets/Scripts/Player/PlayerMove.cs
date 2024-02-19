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
    private Rigidbody rigidbody; // new 선언 요구
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
        Move();
    }

    void Move()
    {
        float delta = Time.deltaTime;

        // 키 입력에 따른 방향 벡터를 구한다.
        inputHandler.TickInput(delta);
        moveDirection = cameraPos.forward * inputHandler.vertical;
        moveDirection += cameraPos.right * inputHandler.horizontal;
        moveDirection.Normalize();

        // 해당 방향에 스피드만큼 rigidbody 이동시킨다.
        float speed = moveSpeed;
        moveDirection *= speed;
        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVec);
        rigidbody.velocity = projectedVelocity;

        // 애니메이션 실행
        playerAnimator.AnimatorValue(inputHandler.moveAmount, 0);

        // 회전이 가능한 경우에는 이동 방향으로 캐릭터를 회전한다.
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
}
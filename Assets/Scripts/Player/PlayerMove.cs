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
        float delta = Time.deltaTime;

        inputHandler.TickInput(delta);
        HandleMovement(delta);
        HandleRollingAndSprinting(delta);
    }

    public void HandleMovement(float delta)
    {
        // 키 입력에 따른 방향 벡터를 구한다.
        moveDirection = cameraPos.forward * inputHandler.vertical;
        moveDirection += cameraPos.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

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

    public void HandleRollingAndSprinting(float delta)
    {
        if (playerAnimator.animator.GetBool("isInteracting")) // 현재 플레이어가 행동 중이지 않을 때만 실행
            return;

        if (inputHandler.rollFlag)
        {
            moveDirection = cameraPos.forward * inputHandler.vertical;
            moveDirection += cameraPos.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0) // 이동중에 회피키를 누르면 구르기
            {
                playerAnimator.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                playerTransform.rotation = rollRotation;
            }
            else // 회피키를 누르지 않으면 백스텝
            {
                playerAnimator.PlayTargetAnimation("Backstep", true);
            }
        }
    }
}
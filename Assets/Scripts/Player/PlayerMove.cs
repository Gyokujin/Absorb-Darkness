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
    public Rigidbody rigidbody; // new 선언 요구
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

        // 키 입력에 따른 방향 벡터를 구한다.
        moveDirection = cameraPos.forward * playerInput.vertical;
        moveDirection += cameraPos.right * playerInput.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // 해당 방향에 스피드만큼 rigidbody 이동시킨다.
        float speed = moveSpeed;
        
        if (playerInput.sprintFlag) // sprintFlag가 활성화 되어 있지 않으면 기본속도. 되어 있으면 달리기 속도로 적용
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

        // 애니메이션 실행
        playerAnimator.AnimatorValue(playerInput.moveAmount, 0, isSprinting);

        // 회전이 가능한 경우에는 이동 방향으로 캐릭터를 회전한다.
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
        if (playerAnimator.animator.GetBool("isInteracting")) // 현재 플레이어가 행동 중이지 않을 때만 실행
            return;

        if (playerInput.rollFlag)
        {
            moveDirection = cameraPos.forward * playerInput.vertical;
            moveDirection += cameraPos.right * playerInput.horizontal;

            if (playerInput.moveAmount > 0) // 이동중에 회피키를 누르면 구르기
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private PlayerManager player;

    [Header("Physics")]
    public new Rigidbody rigidbody;
    public Vector3 moveDirection;
    private Vector3 normalVec;
    private Vector3 targetPosition;

    [Header("Ground Landed")]
    public float inAirTimer;
    private LayerMask ignoreGroundCheck;

    [Header("Component")]
    [SerializeField]
    private Collider playerCollider;
    [SerializeField]
    private Collider playerBlockerCollider;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
        ignoreGroundCheck = LayerMask.GetMask(player.layerData.GroundLayer);
        Physics.IgnoreCollision(playerCollider, playerBlockerCollider, true);
    }

    public void HandleMovement(float delta)
    {
        if (player.isInteracting || player.playerInput.rollFlag)
            return;

        Movement();

        // 플레이어의 LockOn에 따라 다른 애니메이션을 실행한다.
        if (PlayerCamera.instance.isLockOn && !player.playerInput.sprintFlag)
            player.playerAnimator.AnimatorValue(player.playerInput.vertical, player.playerInput.horizontal, player.isSprinting);
        else
            player.playerAnimator.AnimatorValue(player.playerInput.moveAmount, 0, player.isSprinting);

        // 회전이 가능한 경우에는 이동 방향으로 캐릭터를 회전한다.
        if (!PlayerCamera.instance.isLockOn)
            HandleRotation(delta);
    }

    void Movement()
    {
        // 키 입력에 따른 방향 벡터를 구한다.
        moveDirection = PlayerCamera.instance.transform.forward * player.playerInput.vertical;
        moveDirection += PlayerCamera.instance.transform.right * player.playerInput.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // 해당 방향에 스피드만큼 rigidbody 이동시킨다.
        float speed = player.playerStatus.runSpeed;

        if (player.playerInput.sprintFlag && player.playerInput.moveAmount > player.playerPhysicsData.RunCondition && player.playerStatus.CurrentStamina > 0) // sprint
        {
            moveDirection *= player.playerPhysicsData.SprintSpeed;
            player.isSprinting = true;
            player.playerAudio.PlaySprintSFX();
        }
        else if (player.playerInput.moveAmount < player.playerPhysicsData.RunCondition) // Idle, walk
        {
            moveDirection *= player.playerStatus.walkSpeed;
            player.isSprinting = false;
            player.playerAudio.StopFootstepSFX();
            player.playerAudio.StopSprintSFX();
        }
        else if (player.playerInput.moveAmount > 0) // run
        {
            moveDirection *= speed;
            player.isSprinting = false;
            player.playerAudio.PlayFootstepSFX();
        }

        rigidbody.velocity = Vector3.ProjectOnPlane(moveDirection, normalVec);
    }

    void HandleRotation(float delta)
    {
        Vector3 targetdir;
        targetdir = PlayerCamera.instance.cameraTransform.forward * player.playerInput.vertical;
        targetdir += PlayerCamera.instance.cameraTransform.right * player.playerInput.horizontal;
        targetdir.Normalize();
        targetdir.y = 0;

        if (targetdir == Vector3.zero)
            targetdir = player.transform.forward;

        float rs = player.playerStatus.rotationSpeed;
        Quaternion tr = Quaternion.LookRotation(targetdir);
        Quaternion targetrotation = Quaternion.Slerp(player.transform.rotation, tr, rs * delta);
        player.transform.rotation = targetrotation;
    }

    public void LookRotation(Vector3 targetPos)
    {
        if (player.playerInput.sprintFlag || player.playerInput.rollFlag)
            return;

        Vector3 lookDir = (targetPos - player.lockOnTransform.position).normalized;
        lookDir.y = transform.position.y;
        transform.forward = Vector3.Lerp(transform.forward, lookDir, Time.deltaTime * player.physicsData.LookAtSmoothing);
    }

    public void HandleRolling()
    {
        if (player.playerInput.rollFlag && player.playerStatus.CurrentStamina >= player.playerStatusData.ActionLimitStamina)
        {
            player.isDodge = true;
            player.playerInput.rollFlag = true;
            moveDirection.y = 0;

            if (player.playerInput.moveAmount <= 0) // 이동키를 누르지 않으면 백스텝
            {
                player.playerAnimator.PlayTargetAnimation(player.characterAnimatorData.BackstepAnimation, true);
                player.playerStatus.TakeStamina(player.playerStatusData.BackstapStaminaAmount);
                player.playerAudio.PlaySFX(player.playerAudio.playerClips[(int)PlayerAudio.PlayerSound.Backstep]);
            }
            else
            {
                player.transform.LookAt(rigidbody.position + moveDirection);
                player.playerAnimator.PlayTargetAnimation(player.characterAnimatorData.RollingAnimation, true);
                player.playerStatus.TakeStamina(player.playerStatusData.RollingStaminaAmount);
                player.playerAudio.PlaySFX(player.playerAudio.playerClips[(int)PlayerAudio.PlayerSound.Rolling]);
            }
        }
    }

    public void HandleFalling(Vector3 moveDirection)
    {
        player.isGrounded = false;
        Vector3 origin = player.transform.position;
        origin.y += player.physicsData.GroundDetectionRayStart;

        if (Physics.Raycast(origin, player.transform.forward, out RaycastHit hit, player.physicsData.GroundCheckDis))
            moveDirection = Vector3.zero;

        if (player.isInAir)
        {
            rigidbody.AddForce(Vector3.down * player.physicsData.FallingDownForce); // 아래 낙하
            rigidbody.AddForce(moveDirection * player.physicsData.FallingDownForce / player.physicsData.FallingFrontForce); // 끼임 방지를 위해 앞으로 이동
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin += dir * player.physicsData.GroundDirRayDistance;
        targetPosition = player.transform.position;

        if (Physics.Raycast(origin, Vector3.down, out hit, player.physicsData.DistanceBeginFallMin, ignoreGroundCheck))
        {
            normalVec = hit.normal;
            Vector3 transform = hit.point;
            player.isGrounded = true;
            targetPosition.y = transform.y;

            if (player.isInAir)
            {
                if (inAirTimer > player.playerPhysicsData.LandRequirement) // 낙하 시간이 요구치 이상일때만 Land 애니메이션을 실행한다.
                {
                    player.playerAnimator.PlayTargetAnimation(player.characterAnimatorData.LandAnimation, true);
                    inAirTimer = 0;
                }
                else
                {
                    player.playerAnimator.PlayTargetAnimation(player.characterAnimatorData.EmptyAnimation, false);
                    inAirTimer = 0;
                }

                player.isInAir = false;
            }
        }
        else
        {
            if (player.isGrounded)
                player.isGrounded = false;

            if (!player.isInAir)
            {
                if (player.isInteracting)
                    player.playerAnimator.PlayTargetAnimation(player.characterAnimatorData.FallingAnimation, true);

                Vector3 velocity = rigidbody.velocity;
                velocity.Normalize();
                rigidbody.velocity = velocity * (player.playerStatus.runSpeed / player.physicsData.FallingSpeedRatio);
                player.isInAir = true;
            }
        }

        if (player.isInteracting || player.playerInput.moveAmount > 0)
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime / player.physicsData.FallingFactor);
        else
            player.transform.position = targetPosition;

        if (player.isGrounded)
        {
            if (player.isInteracting || player.playerInput.moveAmount > 0)
                player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime);
            else
                player.transform.position = targetPosition;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemDatas;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private PlayerManager player;

    [Header("Ground & Air Detection States")]
    private LayerMask ignoreGroundCheck;
    public float inAirTimer;

    [Header("Physics")]
    public new Rigidbody rigidbody;
    public Vector3 moveDirection;
    private Vector3 normalVec;
    private Vector3 targetPosition;

    [Header("Component")]
    [SerializeField]
    private Collider playerCollider;
    [SerializeField]
    private Collider playerBlockerCollider;
    [SerializeField]
    private AudioSource moveAudio;
    [SerializeField]
    private AudioSource splintAudio;
    PhysicsData physicsData;

    void Start()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
        physicsData = new PhysicsData();
        ignoreGroundCheck = LayerMask.GetMask("Ground");
        Physics.IgnoreCollision(playerCollider, playerBlockerCollider, true);
    }

    public void HandleMovement(float delta)
    {
        if (player.playerInput.rollFlag || player.isInteracting)
            return;

        Movement();

        // 플레이어의 LockOn에 따라 다른 애니메이션을 실행한다.
        if (PlayerCamera.instance.isLockOn && !player.playerInput.sprintFlag)
        {
            player.playerAnimator.AnimatorValue(player.playerInput.vertical, player.playerInput.horizontal, player.isSprinting);
        }
        else
        {
            player.playerAnimator.AnimatorValue(player.playerInput.moveAmount, 0, player.isSprinting);
        }

        // 회전이 가능한 경우에는 이동 방향으로 캐릭터를 회전한다.
        if (player.playerAnimator.canRotate && !PlayerCamera.instance.isLockOn)
        {
            HandleRotation(delta);
        }
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

        if (player.playerInput.sprintFlag && player.playerInput.moveAmount > 0.5f && player.playerStatus.CurrentStamina > 0) // sprint
        {
            moveDirection *= player.playerStatus.sprintSpeed;
            player.isSprinting = true;
            PlaySplintSFX();
        }
        else if (player.playerInput.moveAmount < 0.5f) // walk
        {
            moveDirection *= player.playerStatus.walkSpeed;
            player.isSprinting = false;
        }
        else // run
        {
            moveDirection *= speed;
            player.isSprinting = false;
            PlayMoveSFX();
        }

        rigidbody.velocity = Vector3.ProjectOnPlane(moveDirection, normalVec);
    }

    void HandleRotation(float delta)
    {
        Vector3 targetdir = Vector3.zero;
        float moveoverride = player.playerInput.moveAmount;
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
        transform.forward = Vector3.Lerp(transform.forward, lookDir, Time.deltaTime * physicsData.lookAtSmoothing);
    }

    public void HandleRolling(float delta)
    {
        if (player.isInteracting) // 현재 플레이어가 행동 중이지 않을 때만 실행
        {
            player.playerInput.rollFlag = false;
            return;
        }

        if (player.playerInput.rollFlag && player.playerStatus.CurrentStamina >= player.playerStatus.actionLimitStamina)
        {
            player.onDodge = true;
            player.playerInput.rollFlag = true;
            moveDirection.y = 0;

            if (player.playerInput.moveAmount <= 0) // 이동키를 누르지 않으면 백스텝
            {
                player.playerAnimator.PlayTargetAnimation("Backstep", true);
                player.playerStatus.TakeStamina(player.playerStatus.backStapStaminaAmount);
                AudioManager.instance.PlayPlayerActionSFX(AudioManager.instance.playerActionClips[(int)PlayerActionSound.Backstep]);
            }
            else
            {
                player.transform.LookAt(rigidbody.position + moveDirection);
                player.playerAnimator.PlayTargetAnimation("Rolling", true);
                player.playerStatus.TakeStamina(player.playerStatus.rollingStaminaAmount);
                AudioManager.instance.PlayPlayerActionSFX(AudioManager.instance.playerActionClips[(int)PlayerActionSound.Rolling]);
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        player.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = player.transform.position;
        origin.y += physicsData.groundDetectionRayStart;

        if (Physics.Raycast(origin, player.transform.forward, out hit, physicsData.groundCheckDis))
        {
            moveDirection = Vector3.zero;
        }

        if (player.isInAir)
        {
            rigidbody.AddForce(Vector3.down * physicsData.fallingDownForce); // 아래 낙하
            rigidbody.AddForce(moveDirection * physicsData.fallingDownForce / physicsData.fallingFrontForce); // 끼임 방지를 위해 앞으로 이동
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * physicsData.groundDirRayDistance;
        targetPosition = player.transform.position;

        if (Physics.Raycast(origin, Vector3.down, out hit, physicsData.distanceBeginFallMin, ignoreGroundCheck))
        {
            normalVec = hit.normal;
            Vector3 transform = hit.point;
            player.isGrounded = true;
            targetPosition.y = transform.y;

            if (player.isInAir)
            {
                if (inAirTimer > 0.5f) // 낙하 시간이 0.5초 이상일때만 Land 애니메이션을 실행한다.
                {
                    player.playerAnimator.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    player.playerAnimator.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                player.isInAir = false;
            }
        }
        else
        {
            if (player.isGrounded)
            {
                player.isGrounded = false;
            }

            if (!player.isInAir)
            {
                if (player.isInteracting)
                {
                    player.playerAnimator.PlayTargetAnimation("Falling", true);
                }

                Vector3 velocity = rigidbody.velocity;
                velocity.Normalize();
                rigidbody.velocity = velocity * (player.playerStatus.runSpeed / physicsData.fallingSpeedRatio);
                player.isInAir = true;
            }
        }

        if (player.isInteracting || player.playerInput.moveAmount > 0)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime / physicsData.fallingFactor);
        }
        else
        {
            player.transform.position = targetPosition;
        }

        if (player.isGrounded)
        {
            if (player.isInteracting || player.playerInput.moveAmount > 0)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime);
            }
            else
            {
                player.transform.position = targetPosition;
            }
        }
    }

    void PlayMoveSFX()
    {
        if (!moveAudio.isPlaying)
        {
            moveAudio.Play();
        }
    }

    void PlaySplintSFX()
    {
        if (!splintAudio.isPlaying)
        {
            splintAudio.Play();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private PlayerManager player;

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
    [SerializeField]
    private Collider playerCollider;
    [SerializeField]
    private Collider playerBlockerCollider;
    [SerializeField]
    private AudioSource moveAudio;
    [SerializeField]
    private AudioSource splintAudio;

    void Start()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
        Physics.IgnoreCollision(playerCollider, playerBlockerCollider, true);
    }

    public void HandleMovement(float delta)
    {
        if (player.playerInput.rollFlag || player.isInteracting)
            return;

        // 키 입력에 따른 방향 벡터를 구한다.
        moveDirection = PlayerCamera.instance.transform.forward * player.playerInput.vertical;
        moveDirection += PlayerCamera.instance.transform.right * player.playerInput.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // 해당 방향에 스피드만큼 rigidbody 이동시킨다.
        float speed = player.playerStatus.runSpeed;
        
        if (moveDirection != Vector3.zero)
        {
            if (player.playerInput.sprintFlag && player.playerInput.moveAmount > 0.5f && player.playerStatus.CurrentStamina > 0) // sprintFlag가 활성화 되어 있지 않으면 기본속도. 되어 있으면 달리기 속도로 적용
            {
                player.isSprinting = true;
                moveDirection *= player.playerStatus.sprintSpeed;
                PlaySplintSFX();
            }
            else if (player.playerInput.moveAmount < 0.5f)
            {
                moveDirection *= player.playerStatus.walkSpeed;
                player.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                player.isSprinting = false;
                PlayMoveSFX();
            }
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVec);
        rigidbody.velocity = projectedVelocity;

        // 애니메이션 실행
        if (PlayerCamera.instance.isLockOn && !player.playerInput.sprintFlag)
        {
            player.playerAnimator.AnimatorValue(player.playerInput.vertical, player.playerInput.horizontal, player.isSprinting);
        }
        else
        {
            player.playerAnimator.AnimatorValue(player.playerInput.moveAmount, 0, player.isSprinting);
        }

        // 회전이 가능한 경우에는 이동 방향으로 캐릭터를 회전한다.
        if (player.playerAnimator.canRotate)
        {
            HandleRotation(delta);
        }
    }

    void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = player.playerInput.moveAmount;

        targetDir = PlayerCamera.instance.cameraTransform.forward * player.playerInput.vertical;
        targetDir += PlayerCamera.instance.cameraTransform.right * player.playerInput.horizontal;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
            targetDir = player.transform.forward;

        float rs = player.playerStatus.rotationSpeed;
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rs * delta);
        player.transform.rotation = targetRotation;

        //if (PlayerCamera.instance.isLockOn)
        //{
        //    if (player.playerInput.sprintFlag || player.playerInput.rollFlag)
        //    {
        //        Vector3 targetDir = PlayerCamera.instance.cameraTransform.forward * player.playerInput.vertical;
        //        targetDir += playerCamera.cameraTransform.right * playerInput.horizontal;
        //        targetDir.Normalize();
        //        targetDir.y = 0;

        //        if (targetDir == Vector3.zero)
        //        {
        //            targetDir = transform.forward;
        //        }

        //        Quaternion tr = Quaternion.LookRotation(targetDir);
        //        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, playerStatus.rotationSpeed * Time.deltaTime);
        //        transform.rotation = targetRotation;
        //    }
        //    else
        //    {
        //        Vector3 rotationDir = moveDirection;
        //        rotationDir = playerCamera.currentLockOnTarget.transform.position - transform.position;
        //        rotationDir.y = 0;
        //        rotationDir.Normalize();

        //        Quaternion tr = Quaternion.LookRotation(rotationDir);
        //        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, playerStatus.rotationSpeed * Time.deltaTime);
        //        transform.rotation = targetRotation;
        //    }
        //}
        //else
        //{
        //    Vector3 targetDir = Vector3.zero;
        //    float moveOverride = playerInput.moveAmount;

        //    targetDir = playerCamera.cameraTransform.forward * playerInput.vertical;
        //    targetDir += playerCamera.cameraTransform.right * playerInput.horizontal;
        //    targetDir.Normalize();
        //    targetDir.y = 0;

        //    if (targetDir == Vector3.zero)
        //        targetDir = playerTransform.forward;

        //    float rs = playerStatus.rotationSpeed;
        //    Quaternion tr = Quaternion.LookRotation(targetDir);
        //    Quaternion targetRotation = Quaternion.Slerp(playerTransform.rotation, tr, rs * delta);
        //    playerTransform.rotation = targetRotation;
        //}
    }

    public void HandleRolling(float delta)
    {
        if (player.playerAnimator.animator.GetBool("isInteracting") || player.onDamage) // 현재 플레이어가 행동 중이지 않을 때만 실행
            return;

        if (player.playerInput.rollFlag && player.playerStatus.CurrentStamina >= player.playerStatus.actionLimitStamina)
        {
            player.onDodge = true;
            moveDirection = PlayerCamera.instance.transform.forward * player.playerInput.vertical;
            moveDirection += PlayerCamera.instance.transform.right * player.playerInput.horizontal;

            if (player.playerInput.moveAmount > 0) // 이동중에 회피키를 누르면 구르기
            {
                player.playerAnimator.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = rollRotation;
                player.playerStatus.TakeStamina(player.playerStatus.rollingStaminaAmount);
                AudioManager.instance.PlayPlayerActionSFX(AudioManager.instance.playerActionClips[(int)PlayerActionSound.Rolling]);
            }
            else // 이동키를 누르지 않으면 백스텝
            {
                player.playerAnimator.PlayTargetAnimation("Backstep", true);
                player.playerStatus.TakeStamina(player.playerStatus.backStapStaminaAmount);
                AudioManager.instance.PlayPlayerActionSFX(AudioManager.instance.playerActionClips[(int)PlayerActionSound.Backstep]);
            }
        }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        player.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = player.transform.position;
        origin.y += groundDetectionRayStart;

        if (Physics.Raycast(origin, player.transform.forward, out hit, groundCheckDis))
        {
            moveDirection = Vector3.zero;
        }

        if (player.isInAir)
        {
            rigidbody.AddForce(Vector3.down * fallingDownForce); // 아래 낙하
            rigidbody.AddForce(moveDirection * fallingDownForce / fallingFrontForce); // 끼임 방지를 위해 앞으로 이동
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirRayDistance;
        targetPosition = player.transform.position;

        // Debug.DrawRay(origin, Vector3.down * distanceBeginFallMin, Color.red, 0.1f, false);

        if (Physics.Raycast(origin, Vector3.down, out hit, distanceBeginFallMin, ignoreGroundCheck))
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
                rigidbody.velocity = velocity * (player.playerStatus.runSpeed / fallingSpeedRatio);
                player.isInAir = true;
            }
        }

        if (player.isInteracting || player.playerInput.moveAmount > 0)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime / fallingFactor);
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
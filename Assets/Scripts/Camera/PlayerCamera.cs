using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    [Header("Status")]
    [SerializeField]
    private float lookSpeed = 0.025f;
    [SerializeField]
    private float followSpeed = 0.5f;
    [SerializeField]
    private float pivotSpeed = 0.01f;

    [Header("Player")]
    [SerializeField]
    private PlayerManager player;
    private Transform playerTransform;
    private PlayerInput playerInput; // 게임 시스템 사용시 카메라 제약
    private float playerPosition;
    [SerializeField]
    private float playerFollowRate = 0.2f;

    [Header("Camera")]
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform cameraPivotTransform;
    private Transform camTransform;
    private Vector3 cameraPos;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public LayerMask layerMask;

    [Header("Angle")]
    [SerializeField]
    private float minPivot = -35;
    [SerializeField]
    private float maxPivot = 35;
    private float defaultPosition;
    [SerializeField]
    private float lookAngle = 0.033f;
    private float pivotAngle;

    [Header("Lock On")]
    [SerializeField]
    private float lockRadius = 26;
    [SerializeField]
    private float maxLockOnDistance = 30;

    [Header("Camera Collision")]
    [SerializeField]
    private float cameraSphereRadius = 0.2f;
    [SerializeField]
    private float cameraCollisionOffset = 0.2f;
    [SerializeField]
    private float minCollisionOffset = 0.2f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    void Init()
    {
        playerTransform = player.transform;
        playerInput = player.GetComponent<PlayerInput>();
        camTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void FollowTarget(float delta)
    {
        Vector3 playerPos = Vector3.SmoothDamp(camTransform.position, playerTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        camTransform.position = playerPos;
        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseX, float mouseY)
    {
        if (playerInput.gameSystemFlag)
            return;

        lookAngle += mouseX * lookSpeed / delta;
        pivotAngle -= mouseY * pivotSpeed / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        camTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    public void HandleLockOn()
    {
        float shortesDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, lockRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if (character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - playerTransform.position;
                float distance = Vector3.Distance(playerTransform.position, character.transform.position);
                float viewAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                if (character.transform.root != playerTransform.transform.root && viewAngle > -50 && viewAngle < 50 && distance <= maxLockOnDistance)
                {

                }
            }
        }
    }

    void HandleCameraCollision(float delta)
    {
        playerPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(playerPosition), layerMask))
        {
            float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            playerPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(playerPosition) < minCollisionOffset)
        {
            playerPosition = -minCollisionOffset;
        }

        cameraPos.z = Mathf.Lerp(cameraTransform.localPosition.z, playerPosition, delta / playerFollowRate);
        cameraTransform.localPosition = cameraPos;
    }
}
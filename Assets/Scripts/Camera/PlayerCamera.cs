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
    private PlayerManager player;
    private Transform playerTransform;
    private PlayerInput playerInput; // 게임 시스템 사용시 카메라 제약
    private float playerPosition;
    [SerializeField]
    private float playerFollowRate = 0.2f;

    [Header("Camera")]
    public Transform cameraTransform;
    [SerializeField]
    private Transform cameraPivotTransform;
    public Transform camTransform;
    private Vector3 cameraPos;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public LayerMask targetLayer;

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
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public Transform currentLockOnTarget;
    public Transform nearestLockOnTarget;
    public Transform leftLockTarget;
    public Transform rightLockTarget;
    public LayerMask environmentLayer;
    [SerializeField]
    private float lockRadius = 26;
    [SerializeField]
    private float maxLockOnDistance = 30;
    [SerializeField]
    private float lockOnAngleLimit = 50;
    [SerializeField]
    private float lockedPivotPosition = 2.25f;
    [SerializeField]
    private float unlockedPivotPosition = 1.65f;
    [SerializeField]
    private GameObject lockOnUI;

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

    void Start()
    {
        environmentLayer = LayerMask.NameToLayer("Environment");
    }

    void Update()
    {
        ControlLockOn();
    }

    void Init()
    {
        player = FindObjectOfType<PlayerManager>();
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
        if (!playerInput.lockOnFlag && currentLockOnTarget == null && !playerInput.gameSystemFlag)
        {
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
        else if (currentLockOnTarget != null)
        {
            Vector3 dir = currentLockOnTarget.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
    }

    public void HandleLockOn()
    {
        float shortesDistance = Mathf.Infinity;
        float shortesDistanceLeftTarget = Mathf.Infinity;
        float shortesDistanceRightTarget = Mathf.Infinity;
        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, lockRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if (character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - playerTransform.position;
                float distance = Vector3.Distance(playerTransform.position, character.transform.position);
                float viewAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                RaycastHit hit;

                if (character.transform.root != playerTransform.transform.root
                    && viewAngle > -lockOnAngleLimit && viewAngle < lockOnAngleLimit
                    && distance <= maxLockOnDistance)
                {
                    if (Physics.Linecast(player.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(player.lockOnTransform.position, character.lockOnTransform.position);

                        if (hit.transform.gameObject.layer == environmentLayer)
                        {
                            Debug.Log("Block");
                        }
                        else
                        {
                            lockOnUI.SetActive(true);
                            availableTargets.Add(character);
                        }
                    }
                }
            }
        }

        for (int j = 0; j < availableTargets.Count; j++)
        {
            float targetDistance = Vector3.Distance(playerTransform.position, availableTargets[j].transform.position);

            if (targetDistance < shortesDistance)
            {
                shortesDistance = targetDistance;
                nearestLockOnTarget = availableTargets[j].lockOnTransform;
            }

            if (playerInput.lockOnFlag)
            {
                Vector3 relativeEnemyPos = currentLockOnTarget.InverseTransformPoint(availableTargets[j].transform.position);
                var distanceTargetLeft = currentLockOnTarget.transform.position.x - availableTargets[j].transform.position.x;
                var distanceTargetRight = currentLockOnTarget.transform.position.x + availableTargets[j].transform.position.x;

                if (relativeEnemyPos.x > 0 && distanceTargetLeft < shortesDistanceLeftTarget) // 문제시 0을 0.00
                {
                    shortesDistanceLeftTarget = distanceTargetLeft;
                    leftLockTarget = availableTargets[j].lockOnTransform;
                }

                if (relativeEnemyPos.x < 0 && distanceTargetRight < shortesDistanceRightTarget)
                {
                    shortesDistanceRightTarget = distanceTargetRight;
                    rightLockTarget = availableTargets[j].lockOnTransform;
                }
            }
        }
    }

    public void ClearLockOnTargets()
    {
        availableTargets.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;
        lockOnUI.SetActive(false);
    }

    public void ControlLockOn()
    {
        if (playerInput.lockOnFlag) 
        {
            float distance = Vector3.Distance(currentLockOnTarget.gameObject.transform.position, player.transform.position);

            if (distance > maxLockOnDistance) 
            {
                playerInput.OffLockOn();
            }
            else
            {

            }
        }
    }

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 targetPosition = new Vector3(0, currentLockOnTarget != null ? lockedPivotPosition : unlockedPivotPosition);
        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, targetPosition, ref velocity, Time.deltaTime);
    }

    void HandleCameraCollision(float delta)
    {
        playerPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(playerPosition), targetLayer))
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SystemDatas;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    [Header("Player")]
    private PlayerManager player;
    private PlayerInput playerInput; // 게임 시스템 사용시 카메라 제약
    private float playerPosition;

    [Header("Camera")]
    public Transform cameraTransform;
    [SerializeField]
    private Transform cameraPivotTransform;
    private Transform camTransform;
    private Vector3 cameraPos;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public LayerMask targetLayer;

    [Header("Angle")]
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;

    [Header("LockOn Target")]
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public Transform currentLockOnTarget;
    public Transform nearestLockOnTarget;
    public Transform leftLockTarget;
    public Transform rightLockTarget;
    [SerializeField]
    private LayerMask environmentLayer;

    [Header("LockOn Parameter")]
    [SerializeField]
    private float lockRadius = 26;

    [SerializeField]
    private float maxLockOnDistance = 30;
    [SerializeField]
    private float lockOnAngleLimit = 50;
    [SerializeField]
    private float lockOnRotateMax = 30;
    [SerializeField]
    private float lockedPivotPosition = 2.25f;
    [SerializeField]
    private float unlockedPivotPosition = 1.65f;

    [Header("Lock On UI")]
    [SerializeField]
    private Image lockOnUI;
    [SerializeField]
    private float lockOnUIScaleMin = 0.3f;

    [Header("Camera Collision")]
    [SerializeField]
    private float cameraSphereRadius = 0.2f;
    [SerializeField]
    private float cameraCollisionOffset = 0.2f;
    [SerializeField]
    private float minCollisionOffset = 0.2f;

    [Header("Component")]
    SystemData systemData;

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

    void FixedUpdate()
    {
        ControlLockOn();
    }

    void Init()
    {
        player = FindObjectOfType<PlayerManager>();
        playerInput = player.GetComponent<PlayerInput>();
        camTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;

        systemData = new SystemData();
    }

    public void FollowTarget(float delta)
    {
        Vector3 playerPos = Vector3.SmoothDamp(camTransform.position, player.transform.position, ref cameraFollowVelocity, delta / systemData.followSpeed);
        camTransform.position = playerPos;
        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseX, float mouseY)
    {
        if (!playerInput.lockOnFlag && currentLockOnTarget == null && !playerInput.gameSystemFlag)
        {
            lookAngle += mouseX * systemData.lookSpeed / delta;
            pivotAngle -= mouseY * systemData.pivotSpeed / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, systemData.minPivot, systemData.maxPivot);

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
            eulerAngle.x = Mathf.Min(eulerAngle.x, lockOnRotateMax);
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
    }

    public void HandleLockOn()
    {
        float shortesDistance = Mathf.Infinity;
        float shortesDistanceLeftTarget = Mathf.Infinity;
        float shortesDistanceRightTarget = Mathf.Infinity;
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if (character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - player.transform.position;
                float distance = Vector3.Distance(player.transform.position, character.transform.position);
                float viewAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                RaycastHit hit;

                if (character.transform.root != player.transform.transform.root
                    && viewAngle > -lockOnAngleLimit && viewAngle < lockOnAngleLimit
                    && distance <= maxLockOnDistance)
                {
                    if (Physics.Linecast(player.lockOnTransform.position, character.lockOnTransform.position, out hit))
                    {
                        Debug.DrawLine(player.lockOnTransform.position, character.lockOnTransform.position);

                        if (hit.transform.gameObject.layer != environmentLayer)
                        {
                            lockOnUI.gameObject.SetActive(true);
                            availableTargets.Add(character);
                        }
                    }
                }
            }
        }

        for (int j = 0; j < availableTargets.Count; j++)
        {
            float targetDistance = Vector3.Distance(player.transform.position, availableTargets[j].transform.position);

            if (targetDistance < shortesDistance)
            {
                shortesDistance = targetDistance;
                nearestLockOnTarget = availableTargets[j].lockOnTransform;
            }

            if (playerInput.lockOnFlag)
            {
                Vector3 relativeEnemyPos = currentLockOnTarget.InverseTransformPoint(availableTargets[j].transform.position);
                var distanceTargetLeft = currentLockOnTarget.position.x - availableTargets[j].transform.position.x;
                var distanceTargetRight = currentLockOnTarget.position.x + availableTargets[j].transform.position.x;

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
        lockOnUI.gameObject.SetActive(false);
    }

    public void ControlLockOn()
    {
        if (playerInput.lockOnFlag) 
        {
            float distance = Vector3.Distance(currentLockOnTarget.position, player.transform.position);

            if (distance > maxLockOnDistance) 
            {
                player.OffLockOn();
            }
            else
            {
                float scale = distance / maxLockOnDistance;

                if (scale > lockOnUIScaleMin)
                {
                    lockOnUI.rectTransform.localScale = new Vector3(scale, scale, 1);
                }
                else
                {
                    lockOnUI.rectTransform.localScale = new Vector3(lockOnUIScaleMin, lockOnUIScaleMin, 1);
                }
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

        cameraPos.z = Mathf.Lerp(cameraTransform.localPosition.z, playerPosition, delta / systemData.playerFollowRate);
        cameraTransform.localPosition = cameraPos;
    }
}
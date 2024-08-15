using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    [Header("Data")]
    private CameraData cameraData;
    private LayerData layerData;

    [Header("Player")]
    [SerializeField]
    private PlayerManager player;
    private float playerPosition;

    [Header("Camera")]
    public Transform cameraTransform;
    [SerializeField]
    private Transform cameraPivotTransform;
    private Transform camTransform;
    private Vector3 cameraPos;
    public LayerMask targetLayer;

    [Header("Angle")]
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;

    [Header("LockOn")]
    [HideInInspector]
    public bool isLockOn;
    private readonly List<EnemyManager> availableTargets = new();
    private EnemyManager currentLockOnTarget;
    private Vector3 currentTargetPos;
    [SerializeField]
    private Transform lockOnUI;
    private LayerMask lockOnLayer;
    private LayerMask environmentLayer;

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
        cameraData = new CameraData();
        
        lockOnLayer = LayerMask.GetMask(layerData.EnemyLayer);
        environmentLayer = LayerMask.GetMask(layerData.EnvironmentLayer);

        camTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    void Update()
    {
        TargetCheck();
    }

    void TargetCheck()
    {
        if (isLockOn)
        {
            if (IsTargetRange())
            {
                LookAtTarget();
            }
            else
            {
                SwitchLockOn();
            }
        }
    }

    public void FollowTarget(float delta)
    {
        Vector3 followPos = player.transform.position;
        followPos.y += isLockOn ? cameraData.LockedPivotPositionY : cameraData.UnlockedPivotPositionY;
        camTransform.position = followPos;
        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseX, float mouseY)
    {
        if (!isLockOn && currentLockOnTarget == null && !player.playerInput.gameSystemFlag)
        {
            lookAngle += mouseX * cameraData.LookSpeed / delta;
            pivotAngle -= mouseY * cameraData.PivotSpeed / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, cameraData.MinPivot, cameraData.MaxPivot);

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
            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.x = Mathf.Min(eulerAngle.x, cameraData.LockOnRotateMax);
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }
    }

    public void SwitchLockOn() 
    {
        if (!isLockOn) // 록온이 아닌 상태에서 탐색을 한후 적이 포착되면 활성화한다.
        {
            if (FindLockOnTarget())
            {
                isLockOn = true;
                LockOnTarget();
            }
        }
        else
        {
            isLockOn = false;
            ResetTarget();
        }

        player.playerAnimator.SwitchStance(isLockOn);
    }

    bool FindLockOnTarget()
    {
        Collider[] findTarget = Physics.OverlapSphere(player.transform.position, cameraData.LockOnRadius, lockOnLayer);

        for (int i = 0; i < findTarget.Length; i++)
        {
            EnemyManager target = findTarget[i].gameObject.GetComponent<EnemyManager>();

            if (target != null)
            {
                Vector3 targetDirection = target.transform.position - player.transform.position;
                float viewAngle = Vector3.Angle(targetDirection, cameraTransform.forward);

                if (viewAngle < cameraData.MaxLockOnDistance) // viewAngle > cameraData.minLockOnDistance
                {
                    if (Physics.Linecast(player.lockOnTransform.position, target.lockOnTransform.position, out RaycastHit hit))
                    {
                        if ((environmentLayer.value & (1 << hit.collider.gameObject.layer)) != 0) // 레이를 쏘았을때 벽을 먼저 감지한 경우
                            continue;
                        else // 벽보다 몬스터를 먼저 감지한 경우에 List에 추가
                            availableTargets.Add(target);
                    }
                }
            }
        }

        return availableTargets.Count > 0;
    }

    void LockOnTarget()
    {
        float shortDistance = Mathf.Infinity;
        lockOnUI.gameObject.SetActive(true);

        for (int i = 0; i < availableTargets.Count; i++)
        {
            if (availableTargets[i] != null)
            {
                float targetDistance = Vector3.Distance(player.lockOnTransform.position, availableTargets[i].transform.position);
                
                if (targetDistance < shortDistance) // 가장 가까운 타겟을 찾는다
                {
                    shortDistance = targetDistance;
                    currentLockOnTarget = availableTargets[i];
                }
            }
        }
    }

    void LookAtTarget()
    {
        if (currentLockOnTarget != null && !player.playerInput.rollFlag)
        {
            currentTargetPos = currentLockOnTarget.lockOnTransform.position;
            lockOnUI.position = Camera.main.WorldToScreenPoint(currentTargetPos);
            player.playerMove.LookRotation(currentTargetPos);
        }
    }

    bool IsTargetRange()
    {
        float targetDistance = (player.lockOnTransform.position - currentLockOnTarget.transform.position).magnitude;
        return targetDistance <= cameraData.MaxLockOnDistance;
    }

    void ResetTarget()
    {
        currentLockOnTarget = null;
        availableTargets.Clear();
        lockOnUI.gameObject.SetActive(false);
    }

    void HandleCameraCollision(float delta)
    {
        playerPosition = defaultPosition;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraData.CameraSphereRadius, direction, out RaycastHit hit, Mathf.Abs(playerPosition), targetLayer))
        {
            float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            playerPosition = -(distance - cameraData.CameraCollisionOffset);
        }

        if (Mathf.Abs(playerPosition) < cameraData.MinCollisionOffset)
        {
            playerPosition = -cameraData.MinCollisionOffset;
        }

        cameraPos.z = Mathf.Lerp(cameraTransform.localPosition.z, playerPosition, delta / cameraData.PlayerFollowRate);
    }
}
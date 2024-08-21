using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    private PlayerManager player;

    [Header("Data")]
    private CameraData cameraData;
    private LayerData layerData;

    [Header("Camera")]
    public Transform playerCamTransform; // Main Camera
    [SerializeField]
    private Transform pivotTransform; // Pivot
    private Transform holderTransform; // Camera Holder
    private float lookAngle;
    private float pivotAngle;
    private float preCamPosZ;
    private float curCamPosZ;
    private Vector3 camFollowVelocity = Vector3.zero;
    public LayerMask targetLayers;

    [Header("LockOn")]
    [HideInInspector]
    public bool isLockOn;
    private readonly List<EnemyManager> availableTargets = new();
    private EnemyManager currentLockOnTarget;
    [SerializeField]
    private Transform lockOnUI;
    private LayerMask lockOnLayer;
    private LayerMask environmentLayer;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Init();
    }

    void Init()
    {
        player = FindObjectOfType<PlayerManager>();
        cameraData = new CameraData();
        lockOnLayer = LayerMask.GetMask(layerData.EnemyLayer);
        environmentLayer = LayerMask.GetMask(layerData.EnvironmentLayer);

        holderTransform = transform;
        preCamPosZ = playerCamTransform.localPosition.z;
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
                LookAtTarget();
            else
                SwitchLockOn();
        }
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPos = player.transform.position;
        targetPos.y += isLockOn ? cameraData.LockedPivotPositionY : cameraData.UnlockedPivotPositionY;
        Vector3 followPos = Vector3.SmoothDamp(transform.position, targetPos, ref camFollowVelocity, delta / cameraData.FollowSpeed);
        holderTransform.position = followPos;
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
            holderTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            pivotTransform.localRotation = targetRotation;
        }
        else if (currentLockOnTarget != null)
        {
            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.transform.position - pivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.x = Mathf.Min(eulerAngle.x, cameraData.LockOnRotateMax);
            eulerAngle.y = 0;
            pivotTransform.localEulerAngles = eulerAngle;
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
            if (findTarget[i].GetComponent<EnemyManager>() != null)
            {
                EnemyManager target = findTarget[i].gameObject.GetComponent<EnemyManager>();
                Vector3 targetDirection = target.transform.position - player.transform.position;
                float viewAngle = Vector3.Angle(targetDirection, playerCamTransform.forward);

                if (viewAngle < cameraData.MaxLockOnDistance)
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
            lockOnUI.position = Camera.main.WorldToScreenPoint(currentLockOnTarget.lockOnTransform.position);
            player.playerMove.LookRotation(currentLockOnTarget.lockOnTransform.position);
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
        curCamPosZ = preCamPosZ;
        Vector3 camTransformPosition = Vector3.zero;
        Vector3 direction = (playerCamTransform.position - pivotTransform.position).normalized;

        if (Physics.SphereCast(pivotTransform.position, cameraData.CameraSphereRadius, direction, out RaycastHit hit, Mathf.Abs(curCamPosZ), targetLayers))
        {
            float distance = Vector3.Distance(pivotTransform.position, hit.point);
            curCamPosZ = -(distance - cameraData.CameraCollisionOffset);
        }

        if (Mathf.Abs(curCamPosZ) < cameraData.MinCollisionOffset)
            curCamPosZ = -cameraData.MinCollisionOffset;

        camTransformPosition.z = Mathf.Lerp(playerCamTransform.localPosition.z, curCamPosZ, delta / cameraData.PlayerFollowRate);
        playerCamTransform.localPosition = camTransformPosition;
    }
}
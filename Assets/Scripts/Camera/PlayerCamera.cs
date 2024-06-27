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
    public LayerMask targetLayer;

    [Header("Angle")]
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;

    [Header("LockOn")]
    [HideInInspector]
    public bool isLockOn;
    private List<EnemyManager> availableTargets = new List<EnemyManager>();
    private EnemyManager currentLockOnTarget;
    [SerializeField]
    private Vector3 currentTargetPos;
    [SerializeField]
    private float lockOnUIScaleMin = 0.3f;
    [SerializeField]
    private Transform lockOnUI;
    private LayerMask lockOnLayer;
    private LayerMask environmentLayer;

    [Header("Component")]
    private CameraData cameraData;

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
        camTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        cameraData = new CameraData(); // SystemData 구조체 생성
        lockOnLayer = LayerMask.GetMask("Enemy");
        environmentLayer = LayerMask.GetMask("Environment");

        player = FindObjectOfType<PlayerManager>();
        playerInput = player.GetComponent<PlayerInput>();
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
        followPos.y += isLockOn ? cameraData.lockedPivotPosition : cameraData.unlockedPivotPosition;
        camTransform.position = followPos;
        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseX, float mouseY)
    {
        if (!isLockOn && currentLockOnTarget == null && !playerInput.gameSystemFlag)
        {
            lookAngle += mouseX * cameraData.lookSpeed / delta;
            pivotAngle -= mouseY * cameraData.pivotSpeed / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, cameraData.minPivot, cameraData.maxPivot);

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
            eulerAngle.x = Mathf.Min(eulerAngle.x, cameraData.lockOnRotateMax);
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
    }

    bool FindLockOnTarget()
    {
        Collider[] findTarget = Physics.OverlapSphere(player.transform.position, cameraData.lockOnRadius, lockOnLayer);

        for (int i = 0; i < findTarget.Length; i++)
        {
            EnemyManager target = findTarget[i].gameObject.GetComponent<EnemyManager>();

            if (target != null)
            {
                Vector3 targetDirection = target.transform.position - player.transform.position;
                float viewAngle = Vector3.Angle(targetDirection, cameraTransform.forward);

                if (viewAngle > cameraData.minLockOnDistance && viewAngle < cameraData.maxLockOnDistance)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(player.lockOnTransform.position, target.lockOnTransform.position, out hit))
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
        if (currentLockOnTarget != null)
        {
            currentTargetPos = currentLockOnTarget.lockOnTransform.position;
            lockOnUI.position = Camera.main.WorldToScreenPoint(currentTargetPos);
        }
    }

    bool IsTargetRange()
    {
        float targetDistance = (player.lockOnTransform.position - currentLockOnTarget.transform.position).magnitude;
        return targetDistance <= cameraData.lockOnRadius;
    }

    void ResetTarget()
    {
        currentLockOnTarget = null;
        availableTargets.Clear();
        lockOnUI.gameObject.SetActive(false);
    }

    //public void HandleLockOn()
    //{
    //    float shortesDistance = Mathf.Infinity;
    //    float shortesDistanceLeftTarget = Mathf.Infinity;
    //    float shortesDistanceRightTarget = Mathf.Infinity;
    //    Collider[] colliders = Physics.OverlapSphere(player.transform.position, systemData.lockRadius, lockOnLayer);

    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        EnemyManager character = colliders[i].GetComponent<EnemyManager>();

    //        if (character != null)
    //        {
    //            Vector3 lockTargetDirection = character.transform.position - player.transform.position;
    //            float distance = Vector3.Distance(player.transform.position, character.transform.position);
    //            float viewAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
    //            RaycastHit hit;

    //            if (character.transform.root != player.transform.transform.root
    //                && viewAngle > -systemData.lockOnAngleLimit && viewAngle < systemData.lockOnAngleLimit
    //                && distance <= systemData.maxLockOnDistance)
    //            {
    //                if (Physics.Linecast(player.lockOnTransform.position, character.lockOnTransform.position, out hit))
    //                {
    //                    if (hit.transform.gameObject.layer != environmentLayer)
    //                    {
    //                        lockOnUI.gameObject.SetActive(true);
    //                        availableTargets.Add(character);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    for (int j = 0; j < availableTargets.Count; j++)
    //    {
    //        float targetDistance = Vector3.Distance(player.transform.position, availableTargets[j].transform.position);

    //        if (targetDistance < shortesDistance)
    //        {
    //            shortesDistance = targetDistance;
    //            nearestLockOnTarget = availableTargets[j].lockOnTransform;
    //        }

    //        if (playerInput.lockOnFlag)
    //        {
    //            Vector3 relativeEnemyPos = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[j].transform.position);
    //            var distanceTargetLeft = currentLockOnTarget.transform.position.x - availableTargets[j].transform.position.x;
    //            var distanceTargetRight = currentLockOnTarget.transform.position.x + availableTargets[j].transform.position.x;

    //            if (relativeEnemyPos.x > 0 && distanceTargetLeft < shortesDistanceLeftTarget) // 문제시 0을 0.00
    //            {
    //                shortesDistanceLeftTarget = distanceTargetLeft;
    //                leftLockTarget = availableTargets[j].lockOnTransform;
    //            }

    //            if (relativeEnemyPos.x < 0 && distanceTargetRight < shortesDistanceRightTarget)
    //            {
    //                shortesDistanceRightTarget = distanceTargetRight;
    //                rightLockTarget = availableTargets[j].lockOnTransform;
    //            }
    //        }
    //    }
    //}

    //public void ControlLockOn()
    //{
    //    if (currentLockOnTarget != null)
    //        Debug.Log(currentLockOnTarget.gameObject.name);

    //    if (playerInput.lockOnFlag) 
    //    {
    //        float distance = Vector3.Distance(currentLockOnTarget.transform.position, player.transform.position);

    //        if (distance > systemData.maxLockOnDistance) 
    //        {
    //            player.OffLockOn();
    //        }
    //        else
    //        {
    //            float scale = distance / systemData.maxLockOnDistance;

    //            if (scale > lockOnUIScaleMin)
    //            {
    //                lockOnUI.rectTransform.localScale = new Vector3(scale, scale, 1);
    //            }
    //            else
    //            {
    //                lockOnUI.rectTransform.localScale = new Vector3(lockOnUIScaleMin, lockOnUIScaleMin, 1);
    //            }
    //        }
    //    }
    //}

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 targetPosition = new Vector3(0, currentLockOnTarget != null ? cameraData.lockedPivotPosition : cameraData.unlockedPivotPosition);
        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, targetPosition, ref velocity, Time.deltaTime);
    }

    void HandleCameraCollision(float delta)
    {
        playerPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraData.cameraSphereRadius, direction, out hit, Mathf.Abs(playerPosition), targetLayer))
        {
            float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
            playerPosition = -(distance - cameraData.cameraCollisionOffset);
        }

        if (Mathf.Abs(playerPosition) < cameraData.minCollisionOffset)
        {
            playerPosition = -cameraData.minCollisionOffset;
        }

        cameraPos.z = Mathf.Lerp(cameraTransform.localPosition.z, playerPosition, delta / cameraData.playerFollowRate);
        // cameraTransform.localPosition = cameraPos;
    }
}
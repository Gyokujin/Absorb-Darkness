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
    private GameObjectData gameObjectData;

    [Header("Camera")]
    public Transform camTransforms; // Cameras
    [SerializeField]
    private Transform pivotTransform; // Pivot
    private Transform holderTransform; // Camera Holder
    private bool onShake;
    private float lookAngle;
    private float pivotAngle;
    private float preCamPosZ;
    private float curCamPosZ;
    private Vector3 camFollowVelocity = Vector3.zero;
    public LayerMask targetLayers;

    [Header("LockOn")]
    [HideInInspector]
    public bool isLockOn;
    private int curTargetIndex = 0;
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
        lockOnLayer = LayerMask.GetMask(gameObjectData.EnemyLayer);
        environmentLayer = LayerMask.GetMask(gameObjectData.EnvironmentLayer);

        holderTransform = transform;
        preCamPosZ = camTransforms.localPosition.z;
    }

    void Update()
    {
        TargetCheck();
    }

    void TargetCheck()
    {
        if (isLockOn)
        {
            if (IsTargetRange() && !currentLockOnTarget.onDie)
                LookAtTarget();
            else if (availableTargets.Count > 1)
                ChangeTarget(true);
            else
                SwitchLockOn();
        }
    }

    public void FollowTarget(float delta)
    {
        if (onShake)
            return;

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
            Vector3 dir = (currentLockOnTarget.transform.position - transform.position).normalized;
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
            lookAngle = transform.eulerAngles.y;
        }
    }

    public void SwitchLockOn()
    {
        if (!isLockOn) // �Ͽ��� �ƴ� ���¿��� Ž���� ���� ���� �����Ǹ� Ȱ��ȭ�Ѵ�.
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
        availableTargets.Clear();
        Collider[] findTarget = Physics.OverlapSphere(player.transform.position, cameraData.LockOnRadius, lockOnLayer);

        for (int i = 0; i < findTarget.Length; i++)
        {
            if (findTarget[i].GetComponent<EnemyManager>() != null)
            {
                EnemyManager target = findTarget[i].gameObject.GetComponent<EnemyManager>();

                if (target.onDie)
                    continue;

                Vector3 targetDirection = target.transform.position - player.transform.position;
                float viewAngle = Vector3.Angle(targetDirection, camTransforms.forward);

                if (viewAngle < cameraData.MaxLockOnDistance)
                {
                    if (Physics.Linecast(player.lockOnTransform.position, target.lockOnTransform.position, out RaycastHit hit))
                    {
                        if ((environmentLayer.value & (1 << hit.collider.gameObject.layer)) != 0) // ���̸� ������� ���� ���� ������ ���
                            continue;
                        else // ������ ���͸� ���� ������ ��쿡 List�� �߰�
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

                if (targetDistance < shortDistance) // ���� ����� Ÿ���� ã�´�
                {
                    shortDistance = targetDistance;
                    currentLockOnTarget = availableTargets[i];
                    curTargetIndex = i;
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

    public void ChangeTarget(bool isLeft)
    {
        if (FindLockOnTarget())
        {
            curTargetIndex += isLeft ? -1 : 1;

            if (curTargetIndex >= availableTargets.Count)
                curTargetIndex = 0;

            if (curTargetIndex < 0)
                curTargetIndex = availableTargets.Count - 1;

            currentLockOnTarget = availableTargets[curTargetIndex];
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
        Vector3 direction = (camTransforms.position - pivotTransform.position).normalized;

        if (Physics.SphereCast(pivotTransform.position, cameraData.CameraSphereRadius, direction, out RaycastHit hit, Mathf.Abs(curCamPosZ), targetLayers))
        {
            float distance = Vector3.Distance(pivotTransform.position, hit.point);
            curCamPosZ = -(distance - cameraData.CameraCollisionOffset);
        }

        if (Mathf.Abs(curCamPosZ) < cameraData.MinCollisionOffset)
            curCamPosZ = -cameraData.MinCollisionOffset;

        camTransformPosition.z = Mathf.Lerp(camTransforms.localPosition.z, curCamPosZ, delta / cameraData.PlayerFollowRate);
        camTransforms.localPosition = camTransformPosition;
    }

    public IEnumerator Shake()
    {
        onShake = true;
        float time = 0;
        Vector3 originPos = camTransforms.localPosition;

        while (time <= cameraData.ShakeDuration)
        {
            float x = Random.Range(-1, 1) * cameraData.ShakeMagnitude;
            float y = Random.Range(-1, 1) * cameraData.ShakeMagnitude;
            camTransforms.localPosition = new Vector3(x, y, originPos.z);
            time += Time.deltaTime;
            yield return null;
        }

        onShake = false;
        camTransforms.localPosition = originPos;
    }
}
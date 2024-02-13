using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform targetTransform;
    public Transform camTransform;
    public Transform camPivotTransform;
    private Transform myTransform;
    private Vector3 camTransformPos;
    private LayerMask ignoreLayer;

    public static CameraHandler instance;

    [Header("Cam Parameter")]
    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float defaultPos;
    private float lookAngle;
    private float pivotAngle;
    public float minPivot = -35;
    public float maxPivot = 35;

    void Awake()
    {
        instance = this;

        Init();
    }

    private void Init()
    {
        myTransform = transform;
        defaultPos = camTransform.localPosition.z;
        ignoreLayer = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPos = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
        myTransform.position = targetPos;
    }

    public void HandleCameraRotation(float delta, float mouseInputX, float mouseInputY)
    {
        lookAngle += (mouseInputX * lookSpeed) / delta;
        pivotAngle -= (mouseInputY * pivotSpeed) / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        myTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        camPivotTransform.localRotation = targetRotation;
    }
}
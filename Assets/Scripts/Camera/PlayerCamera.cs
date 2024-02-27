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
    private float followSpeed = 0.1f;
    [SerializeField]
    private float pivotSpeed = 0.0075f;

    [Header("Camera Target")]
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform cameraPivotTransform;
    private Transform myTransform;
    private Vector3 cameraPos;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private LayerMask layerMask;

    [Header("Angle")]
    [SerializeField]
    private float minPivot = -35;
    [SerializeField]
    private float maxPivot = 35;
    private float playerPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;

    [Header("Camera Collision")]
    [SerializeField]
    private float cameraSphereRadius = 0.2f;
    [SerializeField]
    private float cameraCollisionOffset = 0.2f;
    private float minCollisionOffset = 0.2f;

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        myTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        layerMask = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void FollowTarget(float delta)
    {
        Vector3 playerPos = Vector3.SmoothDamp(myTransform.position, playerTransform.position, ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = playerPos;

        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseX, float mouseY)
    {
        lookAngle += mouseX * lookSpeed / delta;
        pivotAngle -= mouseY * pivotSpeed / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        myTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
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

        cameraPos.z = Mathf.Lerp(cameraTransform.localPosition.z, playerPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraPos;
    }
}
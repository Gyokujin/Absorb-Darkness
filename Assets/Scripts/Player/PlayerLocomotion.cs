using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private Transform cameraObject;
    private InputHandler inputHandler;
    private Vector3 moveDirection;

    [HideInInspector]
    public Transform playerTransform;

    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Status")]
    [SerializeField]
    private float movementSpeed = 5;
    [SerializeField]
    private float rotationSpeed = 10;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        cameraObject = Camera.main.transform;
        playerTransform = transform;
    }

    public void Update()
    {
        float delta = Time.deltaTime;

        inputHandler.TickInput(delta);

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();

        float speed = movementSpeed;
        moveDirection *= speed;

        Vector3 projectVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectVelocity;
    }

    #region Movement
    private Vector3 normalVector;
    private Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
            targetDir = playerTransform.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(playerTransform.rotation, tr, rs * delta);

        playerTransform.rotation = targetRotation;
    }

    #endregion
}
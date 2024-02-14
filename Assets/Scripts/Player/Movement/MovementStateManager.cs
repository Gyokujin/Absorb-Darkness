using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private float moveSpeed = 3;
    [HideInInspector]
    public Vector3 direction;
    private float horInput, verInput;

    [Header("Physics")]
    [SerializeField]
    private float groundYOffset;
    [SerializeField]
    private LayerMask groundMask;
    private Vector3 spherePos;

    [SerializeField]
    private float gravity = -9.81f;
    private Vector3 velocity;

    [Header("Component")]
    private CharacterController controller;

    void Start()
    {
        Init();
    }

    void Update()
    {
        GetDirAndMove();
        Gravity();
    }

    void Init()
    {
        controller = GetComponent<CharacterController>();
    }

    void GetDirAndMove()
    {
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");

        direction = transform.forward * verInput + transform.right * horInput;
        controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        bool isGrounded = Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask);
        return isGrounded;
    }

    void Gravity()
    {
        if (!IsGrounded())
            velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0)
            velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Crouch,
        Run
    }

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

    [Header("State")]
    public PlayerState playerState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public CrouchState Crouch = new CrouchState();
    public RunState Run = new RunState();

    [Header("Component")]
    [HideInInspector]
    public Animator animator;
    private CharacterController controller;
    private MovementBaseState currentState;

    void Start()
    {
        Init();
    }

    void Update()
    {
        GetDirAndMove();
        Gravity();
        currentState.UpdateState(this);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
    }

    void Init()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    void GetDirAndMove()
    {
        // 키 입력
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");

        // 키 입력에 따른 캐릭터 이동
        direction = transform.forward * verInput + transform.right * horInput;
        controller.Move(direction.normalized * moveSpeed * Time.deltaTime);

        // 키 입력에 따른 애니메이터 파라미터 조절
        animator.SetFloat("InputHor", horInput);
        animator.SetFloat("InputVer", verInput);
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

    public void SwitchState(MovementBaseState state)
    {
        if (state == Idle)
            playerState = PlayerState.Idle;
        else if (state == Walk)
            playerState = PlayerState.Walk;
        else if (state == Crouch)
            playerState = PlayerState.Crouch;
        else if (state == Run)
            playerState = PlayerState.Run;

        Debug.Log($"current State = {playerState}");
        currentState = state;
        currentState.EnterState(this);
    }
}
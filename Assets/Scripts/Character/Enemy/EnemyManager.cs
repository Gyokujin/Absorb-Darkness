using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    [Header("State")]
    public EnemyState curState;

    [Header("Action")]
    public bool isInteracting;
    public bool isPreformingAction;
    public bool onHit;
    public LayerMask detectionLayer;

    [Header("Status")]
    [SerializeField]
    private float stopDistance = 2f;
    public float currentRecoveryTime = 0;

    [Header("Component")]
    public new Rigidbody rigidbody;
    public new Collider collider;
    public Collider blockerCollider;
    public NavMeshAgent navMeshAgent;
    public CharacterStatus currentTarget;
    private EnemyStatus enemyStatus;
    private EnemyAnimator enemyAnimator;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        enemyStatus = GetComponent<EnemyStatus>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
    }

    void Start()
    {
        rigidbody.isKinematic = false;
        navMeshAgent.enabled = false;
        navMeshAgent.speed = enemyStatus.runSpeed;
        navMeshAgent.acceleration = enemyStatus.runSpeed;
        navMeshAgent.angularSpeed = enemyStatus.rotationSpeed;
        navMeshAgent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        isInteracting = enemyAnimator.animator.GetBool("isInteracting");
    }

    void FixedUpdate()
    {
        HandleStateMachine();
    }

    void HandleStateMachine()
    {
        if (curState != null && !onDie)
        {
            EnemyState nextState = curState.Tick(this, enemyStatus, enemyAnimator);

            if (nextState != null)
            {
                SwitchNextState(nextState);
            }
        }
    }

    void SwitchNextState(EnemyState state)
    {
        curState = state;
    }
}
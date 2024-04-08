using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    [Header("State")]
    public bool isPreformingAction;

    [Header("PursueTarget")]
    public float targetDistance;
    public float stopDistance = 2f;

    [Header("Attack")]
    public float currentRecoveryTime = 0;

    [Header("Component")]
    public NavMeshAgent navMeshAgent;
    public new Rigidbody rigidbody;
    public CharacterStatus currentTarget;
    public EnemyState curState;
    public EnemyStatus enemyStatus;
    // private EnemyMove enemyMove;
    private EnemyAnimator enemyAnimator;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        // enemyMove = GetComponent<EnemyMove>();
        enemyStatus = GetComponent<EnemyStatus>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
    }

    void Start()
    {
        rigidbody.isKinematic = false;
        navMeshAgent.enabled = false;
        navMeshAgent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        HandleRecoveryTimer();
    }

    void FixedUpdate()
    {
        HandleStateMachine();
    }

    void HandleStateMachine()
    {
        if (curState != null)
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

    void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }
        else if (isPreformingAction)
        {
            isPreformingAction = false;
        }
    }
}
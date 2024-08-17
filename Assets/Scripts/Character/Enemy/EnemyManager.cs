using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CharacterData;
using SystemData;

public class EnemyManager : CharacterManager
{
    public enum EnemyType
    {
        Normal, Named, Boss
    }

    public EnemyType enemyType;

    [Header("Data")]
    public CharacterAnimatorData characterAnimatorData;
    public PhysicsData physicsData;
    public LayerData layerData;

    [Header("State")]
    [HideInInspector]
    public EnemyState curState;
    [HideInInspector]
    public AmbushState ambushState;
    [HideInInspector]
    public IdleState idleState;
    [HideInInspector]
    public PursueTargetState pursueTargetState;
    [HideInInspector]
    public CombatStanceState combatStanceState;
    [HideInInspector]
    public AttackState attackState;

    [Header("Action")]
    public DamageCollider[] attackColliders;
    [HideInInspector]
    public bool isInteracting;
    [HideInInspector]
    public bool isPreformingAction;
    [HideInInspector]
    public LayerMask detectionLayer;

    [Header("Status")]
    [SerializeField]
    private float stopDistance = 2f;
    public float currentRecoveryTime = 0;

    [Header("Component")]
    public new Rigidbody rigidbody;
    public new Collider collider;
    public Collider blockerCollider;
    public NavMeshAgent navMesh;
    public CharacterStatus currentTarget;

    [Header("Enemy Component")]
    [HideInInspector]
    public EnemyStatus enemyStatus;
    [HideInInspector]
    public EnemyAudio enemyAudio;
    [HideInInspector]
    public EnemyAnimator enemyAnimator;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        navMesh = GetComponentInChildren<NavMeshAgent>();
        detectionLayer = LayerMask.GetMask(layerData.PlayerLayer);

        enemyStatus = GetComponent<EnemyStatus>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        enemyAudio = GetComponent<EnemyAudio>();
        attackColliders = GetComponentsInChildren<DamageCollider>();

        ambushState = GetComponentInChildren<AmbushState>();
        idleState = GetComponentInChildren<IdleState>();
        pursueTargetState = GetComponentInChildren<PursueTargetState>();
        combatStanceState = GetComponentInChildren<CombatStanceState>();
        attackState = GetComponentInChildren<AttackState>();
        curState = idleState;
    }

    void Start()
    {
        rigidbody.isKinematic = false;
        navMesh.enabled = false;
        navMesh.speed = enemyStatus.runSpeed;
        navMesh.acceleration = enemyStatus.runSpeed;
        navMesh.angularSpeed = enemyStatus.rotationSpeed;
        navMesh.stoppingDistance = stopDistance;
        Physics.IgnoreCollision(collider, blockerCollider, true);
    }

    void Update()
    {
        isInteracting = enemyAnimator.animator.GetBool(characterAnimatorData.InteractParameter);
    }

    void FixedUpdate()
    {
        HandleStateMachine();
    }

    void HandleStateMachine()
    {
        if (curState != null && !onDie)
        {
            EnemyState nextState = curState.Tick();

            if (nextState != null)
                curState = nextState;
        }
    }

    public void CloseColliders()
    {

    }
}
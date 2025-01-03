using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CharacterData;
using SystemData;
using static PoolManager;

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
    public GameObjectData gameObjectData;

    [Header("State")]
    private EnemyState curState;
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

    [Header("Physics")]
    [SerializeField]
    private float stopDistance = 2f;
    public Collider blockerCollider;

    [Header("Combat")]
    public bool onChase;
    [HideInInspector]
    public WeaponDamageCollider[] attackColliders;
    [HideInInspector]
    public bool isInteracting;
    [HideInInspector]
    public bool isPreformingAction;
    [HideInInspector]
    public LayerMask detectionLayer;
    [HideInInspector]
    public CharacterStatus currentTarget;

    [Header("Component")]
    [HideInInspector]
    public new Rigidbody rigidbody;
    [HideInInspector]
    public new Collider collider;
    [HideInInspector]
    public NavMeshAgent navMesh;
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

    void OnEnable()
    {
        onDie = false;
        collider.enabled = true;
        blockerCollider.enabled = true;
        rigidbody.isKinematic = false;

        curState = idleState;
        navMesh.enabled = false;
    }

    protected virtual void Init()
    {
        detectionLayer = LayerMask.GetMask(gameObjectData.PlayerLayer);

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        navMesh = GetComponentInChildren<NavMeshAgent>();

        enemyStatus = GetComponent<EnemyStatus>();
        enemyAudio = GetComponent<EnemyAudio>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();

        ambushState = GetComponentInChildren<AmbushState>();
        idleState = GetComponentInChildren<IdleState>();
        pursueTargetState = GetComponentInChildren<PursueTargetState>();
        combatStanceState = GetComponentInChildren<CombatStanceState>();
        attackState = GetComponentInChildren<AttackState>();
    }

    void Start()
    {
        navMesh.speed = enemyStatus.runSpeed;
        navMesh.acceleration = enemyStatus.runSpeed;
        navMesh.angularSpeed = enemyStatus.rotationSpeed;
        navMesh.stoppingDistance = stopDistance;

        attackColliders = GetComponentsInChildren<WeaponDamageCollider>();
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyManager;

public class EnemyMove : MonoBehaviour
{
    [Header("Patrol")]
    public LayerMask detectionLayer;
    public CharacterStats currentTarget;
    public float targetDistance;
    public float stopDistance = 2f;

    [Header("Component")]
    public new Rigidbody rigidbody;
    private NavMeshAgent navMeshAgent;
    private EnemyManager enemyManager;
    private EnemyAnimator enemyAnimator;
    private EnemyStats enemyStats;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        enemyStats = GetComponent<EnemyStats>();
        navMeshAgent.stoppingDistance = stopDistance;
    }

    void Start()
    {
        navMeshAgent.enabled = false;
        rigidbody.isKinematic = false;
    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyStats.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyStats.detectionAngleMin && viewableAngle < enemyStats.detectionAngleMax)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }

    public void HandleMoveTarget()
    {
        Vector3 targetDirection = currentTarget.transform.position - transform.position;
        targetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.isPreformingAction)
        {
            enemyAnimator.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
            navMeshAgent.enabled = false;
        }
        else
        {
            if (targetDistance > stopDistance)
            {
                enemyManager.state = EnemyState.Move;
                enemyAnimator.animator.SetFloat("vertical", 1, 0.1f, Time.deltaTime);

                targetDirection.Normalize();
                targetDirection.y = 0;

                targetDirection *= enemyStats.runSpeed;
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
                rigidbody.velocity = projectedVelocity;
            }
            else if (targetDistance <= stopDistance)
            {
                enemyManager.state = EnemyState.Idle;
                enemyAnimator.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
                rigidbody.velocity = Vector3.zero;
            }
        }

        HandleRotateTarget();
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    void HandleRotateTarget()
    {
        if (enemyManager.isPreformingAction)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = Vector3.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyStats.rotationSpeed / Time.deltaTime);
        }
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = rigidbody.velocity;

            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(currentTarget.transform.position);
            navMeshAgent.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, enemyStats.rotationSpeed / Time.deltaTime);
        }
    }
}
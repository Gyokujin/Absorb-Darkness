using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyManager;

public class EnemyMove : MonoBehaviour
{
    [Header("Patrol")]
    // public LayerMask detectionLayer;
    public float targetDistance;
    public float stopDistance = 2f;

    [Header("Component")]
    public new Rigidbody rigidbody;
    private NavMeshAgent navMeshAgent;
    private EnemyManager enemyManager;
    private EnemyAnimator enemyAnimator;
    private EnemyStatus enemyStatus;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        enemyStatus = GetComponent<EnemyStatus>();
        navMeshAgent.stoppingDistance = stopDistance;
    }

    void Start()
    {
        navMeshAgent.enabled = false;
        rigidbody.isKinematic = false;
    }

    public void HandleMoveTarget()
    {
        if (enemyManager.isPreformingAction && targetDistance > stopDistance)
            return;

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
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
                enemyAnimator.animator.SetFloat("vertical", 1, 0.1f, Time.deltaTime);
                targetDirection.Normalize();
                targetDirection.y = 0;
                targetDirection *= enemyStatus.runSpeed;
                Vector3 projectedVelocity = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
                rigidbody.velocity = projectedVelocity;
            }
            else if (targetDistance <= stopDistance)
            {
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
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = Vector3.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyStatus.rotationSpeed / Time.deltaTime);
        }
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = rigidbody.velocity;

            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            navMeshAgent.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, enemyStatus.rotationSpeed / Time.deltaTime);
        }
    }
}
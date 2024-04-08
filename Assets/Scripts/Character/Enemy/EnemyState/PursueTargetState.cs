using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueTargetState : EnemyState
{
    public CombatStanceState combatStanceState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        if (enemyManager.isPreformingAction && enemyManager.targetDistance > enemyManager.stopDistance)
            return this;

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        enemyManager.targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.targetDistance > enemyStatus.attackRangeMax)
        {
            enemyAnimator.animator.SetFloat("vertical", 1, 0.1f, Time.deltaTime);
            targetDirection.Normalize();
            targetDirection.y = 0;
            targetDirection *= enemyStatus.runSpeed;
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
            enemyManager.rigidbody.velocity = projectedVelocity;
        }

        HandleRotateTarget(enemyManager, enemyStatus);
        enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
        enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

        if (enemyManager.targetDistance <= enemyStatus.attackRangeMax)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }
    }

    void HandleRotateTarget(EnemyManager enemyManager, EnemyStatus enemyStatus)
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
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyStatus.rotationSpeed / Time.deltaTime);
        }
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.rigidbody.velocity;

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.navMeshAgent.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyStatus.rotationSpeed / Time.deltaTime);
        }
    }
}
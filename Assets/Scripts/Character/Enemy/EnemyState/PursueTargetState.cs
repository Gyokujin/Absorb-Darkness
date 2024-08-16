using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : EnemyState
{
    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (enemyManager.isPreformingAction || enemyManager.onDamage)
        {
            enemyManager.navMeshAgent.enabled = false;
            return this;
        }
        else if (targetDistance > enemyStatus.attackRangeMax)
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

        if (targetDistance <= enemyStatus.attackRangeMax)
        {
            return enemyManager.combatStanceState;
        }
        else
        {
            return this;
        }
    }

    void HandleRotateTarget(EnemyManager enemyManager, EnemyStatus enemyStatus)
    {
        Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;

        if (enemyManager.isPreformingAction)
        {
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = Vector3.forward;
            }
        }
        else
        {
            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.velocity = enemyManager.rigidbody.velocity;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyStatus.rotationSpeed / Time.deltaTime);
        }

        enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), enemyStatus.rotationSpeed / Time.deltaTime);
    }
}
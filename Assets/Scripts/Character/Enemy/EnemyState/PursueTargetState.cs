using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : EnemyState
{
    public override EnemyState Tick()
    {
        Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
        float targetDistance = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);

        if (enemy.isPreformingAction || enemy.onDamage)
        {
            enemy.navMesh.enabled = false;
            return this;
        }
        else if (targetDistance > enemy.enemyStatus.attackRangeMax)
        {
            enemy.enemyAnimator.animator.SetFloat(enemy.characterAnimatorData.VerticalParameter, enemy.characterAnimatorData.RunParameterValue, enemy.characterAnimatorData.AnimationDampTime, Time.deltaTime);
            targetDirection.Normalize();
            targetDirection.y = 0;
            targetDirection *= enemy.enemyStatus.runSpeed;
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(targetDirection, Vector3.up);
            enemy.rigidbody.velocity = projectedVelocity;
        }

        HandleRotateTarget();
        enemy.navMesh.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (targetDistance <= enemy.enemyStatus.attackRangeMax)
            return enemy.combatStanceState;
        else
            return this;
    }

    void HandleRotateTarget()
    {
        Vector3 direction = enemy.currentTarget.transform.position - transform.position;

        if (enemy.isPreformingAction)
        {
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
                direction = Vector3.forward;
        }
        else
        {
            enemy.navMesh.enabled = true;
            enemy.navMesh.velocity = enemy.rigidbody.velocity;
            enemy.navMesh.SetDestination(enemy.currentTarget.transform.position);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, enemy.navMesh.transform.rotation, enemy.enemyStatus.rotationSpeed / Time.deltaTime);
        }

        enemy.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), enemy.enemyStatus.rotationSpeed / Time.deltaTime);
    }
}
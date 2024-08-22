using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    [Header("Attack")]
    [SerializeField]
    private EnemyAttackAction[] enemyAttacks;
    private EnemyAttackAction currentAttack;

    public override EnemyState Tick()
    {
        Vector3 targetDirection = enemy.currentTarget.transform.position - transform.position;
        float targetDistance = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemy.isPreformingAction)
            return enemy.combatStanceState;
        else if (currentAttack != null && targetDistance > currentAttack.attackDisMin && targetDistance < currentAttack.attackDisMax && viewableAngle <= currentAttack.attackAngleMax && viewableAngle >= currentAttack.attackAngleMin)
            Attack();
        else
            currentAttack = enemyAttacks[Random.Range(0, enemyAttacks.Length)];

        return enemy.pursueTargetState;
    }

    void Attack()
    {
        if (enemy.onDie)
            return;

        enemy.isPreformingAction = true;
        enemy.navMesh.enabled = false;
        enemy.currentRecoveryTime = Random.Range(currentAttack.recoveryTimeMin, currentAttack.recoveryTimeMax);

        enemy.enemyAnimator.animator.SetFloat(enemy.characterAnimatorData.HorizontalParameter, enemy.characterAnimatorData.IdleParameterValue); // 공격은 즉시 애니메이션을 정지하게 한다.
        enemy.enemyAnimator.animator.SetFloat(enemy.characterAnimatorData.VerticalParameter, enemy.characterAnimatorData.IdleParameterValue);
        enemy.enemyAnimator.PlayTargetAnimation(currentAttack.actionAnimation, true);
        currentAttack = null;
    }
}
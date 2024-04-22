using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    [Header("Attack")]
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;

    [Header("States")]
    [SerializeField]
    private CombatStanceState combatStanceState;
    [SerializeField]
    private PursueTargetState pursueTargetState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.isPreformingAction)
        {
            return combatStanceState;
        }
        else if (currentAttack != null && targetDistance > currentAttack.attackDisMin && targetDistance < currentAttack.attackDisMax && 
            viewableAngle <= currentAttack.attackAngleMax && viewableAngle >= currentAttack.attackAngleMin)
        {
            Attack(enemyManager, enemyAnimator);
        }
        else
        {
            currentAttack = enemyAttacks[Random.Range(0, enemyAttacks.Length)];
        }

        return pursueTargetState;
    }

    void Attack(EnemyManager enemyManager, EnemyAnimator enemyAnimator)
    {
        if (enemyManager.onDie)
            return;

        enemyManager.isPreformingAction = true;
        enemyManager.navMeshAgent.enabled = false;
        enemyManager.currentRecoveryTime = Random.Range(currentAttack.recoveryTimeMin, currentAttack.recoveryTimeMax);
        enemyAnimator.animator.SetFloat("horizontal", 0); // 공격은 즉시 애니메이션을 정지하게 한다.
        enemyAnimator.animator.SetFloat("vertical", 0);
        enemyAnimator.PlayTargetAnimation(currentAttack.actionAnimation, true);
        currentAttack = null;
    }
}
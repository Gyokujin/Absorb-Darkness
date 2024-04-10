using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public CombatStanceState combatStanceState;
    public PursueTargetState pursueTargetState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.isPreformingAction)
            return combatStanceState;

        if (currentAttack != null && targetDistance > currentAttack.attackDisMin && targetDistance < currentAttack.attackDisMax && 
            viewableAngle <= currentAttack.attackAngleMax && viewableAngle >= currentAttack.attackAngleMin)
        {
            Attack(enemyManager, enemyAnimator);
        }
        else
        {
            GetNewAttack(enemyManager);
        }

        return pursueTargetState;
    }

    void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetDir = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDir, transform.forward);
        float targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (targetDistance <= enemyAttackAction.attackDisMax && targetDistance >= enemyAttackAction.attackDisMin &&
                viewableAngle <= enemyAttackAction.attackAngleMax && viewableAngle >= enemyAttackAction.attackAngleMin)
            {
                maxScore += enemyAttackAction.attackScore;
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int tempScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (targetDistance <= enemyAttackAction.attackDisMax && targetDistance >= enemyAttackAction.attackDisMin &&
                viewableAngle <= enemyAttackAction.attackAngleMax && viewableAngle >= enemyAttackAction.attackAngleMin)
            {
                if (currentAttack != null)
                    return;

                tempScore += enemyAttackAction.attackScore;

                if (tempScore > randomValue)
                {
                    currentAttack = enemyAttackAction;
                }
            }
        }
    }

    void Attack(EnemyManager enemyManager, EnemyAnimator enemyAnimator)
    {
        enemyManager.navMeshAgent.enabled = false;
        enemyAnimator.animator.SetBool("onAttack", true);
        enemyAnimator.animator.SetFloat("horizontal", 0, 0.1f, Time.deltaTime);
        enemyAnimator.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
        enemyAnimator.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyManager.isPreformingAction = true;
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }
}
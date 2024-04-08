using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public CombatStanceState combatStanceState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        if (enemyManager.isPreformingAction)
            return combatStanceState;

        if (currentAttack != null)
        {
            if (enemyManager.targetDistance < currentAttack.attackDisMin)
            {
                return this;
            }
            else if (enemyManager.targetDistance < currentAttack.attackAngleMax)
            {
                if (enemyStatus.viewableAngle <= currentAttack.attackAngleMax && enemyStatus.viewableAngle >= currentAttack.attackAngleMin &&
                    enemyManager.currentRecoveryTime <= 0 && !enemyManager.isPreformingAction)
                {
                    Attack(enemyManager, enemyAnimator);
                    // return combatStanceState;
                }
            }
        }
        else
        {
            GetNewAttack(enemyManager);
        }

        return combatStanceState;
    }

    void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetDir = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDir, transform.forward);
        enemyManager.targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyManager.targetDistance <= enemyAttackAction.attackDisMax && enemyManager.targetDistance >= enemyAttackAction.attackDisMin &&
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

            if (enemyManager.targetDistance <= enemyAttackAction.attackDisMax && enemyManager.targetDistance >= enemyAttackAction.attackDisMin &&
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
        enemyAnimator.animator.SetFloat("horizontal", 0, 0.1f, Time.deltaTime);
        enemyAnimator.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
        enemyAnimator.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyManager.isPreformingAction = true;
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }   
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : EnemyState
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        float targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (enemyManager.isPreformingAction)
        {
            enemyAnimator.animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
        }

        if (enemyManager.currentRecoveryTime <= 0 && targetDistance <= enemyStatus.attackRangeMax)
        {
            return attackState;
        }
        else if (targetDistance > enemyStatus.attackRangeMax)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
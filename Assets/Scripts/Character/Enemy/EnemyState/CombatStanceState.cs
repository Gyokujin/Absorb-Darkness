using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : EnemyState
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        enemyManager.targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.targetDistance <= enemyStatus.attackRangeMax)
        {
            return attackState;
        }
        else if (enemyManager.targetDistance > enemyStatus.attackRangeMax)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : EnemyState
{
    [Header("States")]
    public AttackState attackState;
    public PursueTargetState pursueTargetState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        float targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (targetDistance <= enemyStatus.attackRangeMax)
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
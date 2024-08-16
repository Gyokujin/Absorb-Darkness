using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : EnemyState
{
    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        float targetDistance = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (targetDistance <= enemyStatus.attackRangeMax)
        {
            return enemyManager.attackState;
        }
        else if (targetDistance > enemyStatus.attackRangeMax)
        {
            return enemyManager.pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
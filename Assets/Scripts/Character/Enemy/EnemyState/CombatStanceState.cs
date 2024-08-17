using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : EnemyState
{
    public override EnemyState Tick()
    {
        float targetDistance = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);

        if (targetDistance <= enemy.enemyStatus.attackRangeMax)
            return enemy.attackState;
        else if (targetDistance > enemy.enemyStatus.attackRangeMax)
            return enemy.pursueTargetState;
        else
            return this;
    }
}
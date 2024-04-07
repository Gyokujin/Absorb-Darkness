using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : EnemyState
{
    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        return this;
    }
}
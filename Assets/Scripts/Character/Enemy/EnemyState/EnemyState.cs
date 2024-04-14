using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    private EnemyManager enemyManager;

    void Awake()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    public abstract EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator);
}
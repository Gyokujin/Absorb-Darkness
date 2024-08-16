using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyManager enemyManager;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    public abstract EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator);
}
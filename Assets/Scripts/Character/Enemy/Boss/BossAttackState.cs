using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : MonoBehaviour
{
    [Header("Boss Patern")]
    [SerializeField]
    private EnemyAttackAction[] phase1Pattern;
    [SerializeField]
    private EnemyAttackAction[] phase2Pattern;
    [SerializeField]
    private EnemyAttackAction[] phase3Pattern;

    [Header("Component")]
    private AttackState attackState;

    void Awake()
    {
        attackState = GetComponentInChildren<AttackState>();
    }

    void Start()
    {
        ControlPattern(phase1Pattern);
    }

    void ControlPattern(EnemyAttackAction[] patterns)
    {
        for (int i = 0; i < attackState.enemyAttacks.Length; i++)
        {
            attackState.enemyAttacks[i] = patterns[i];
        }
    }
}
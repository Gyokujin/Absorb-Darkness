using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
    public enum EnemyState
    {
        Idle, Move, Attack
    }

    [Header("State")]
    public EnemyState state;
    public bool isPreformingAction;

    [Header("Component")]
    private EnemyMove enemyMove;

    void Awake()
    {
        state = EnemyState.Idle;
        enemyMove = GetComponent<EnemyMove>();
    }

    void FixedUpdate()
    {
        HandleCurrentAction();
    }

    void HandleCurrentAction()
    {
        if (enemyMove.currentTarget == null)
        {
            enemyMove.HandleDetection();
        }
        else
        {
            enemyMove.HandleMoveTarget();
        }
    }
}
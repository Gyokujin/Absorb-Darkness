using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
    [Header("State")]
    public bool isPreformingAction;

    [Header("Detection")]
    public float detectionRadius = 20;
    public float detectionAngleMax = 50;
    public float detectionAngleMin = -50;

    [Header("Component")]
    private EnemyMove enemyMove;

    void Awake()
    {
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
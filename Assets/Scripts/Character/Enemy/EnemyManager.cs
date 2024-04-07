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
    public EnemyState state = EnemyState.Idle;
    public bool isPreformingAction;

    [Header("Attack")]
    [SerializeField]
    private EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public float currentRecoveryTime = 0;

    [Header("Component")]
    private EnemyMove enemyMove;
    private EnemyStats enemyStats;
    private EnemyAnimator enemyAnimator;

    void Awake()
    {
        enemyMove = GetComponent<EnemyMove>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
    }

    void Update()
    {
        HandleRecoveryTimer();
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
        else if (enemyMove.targetDistance > enemyMove.stopDistance)
        {
            enemyMove.HandleMoveTarget();
        }
        else if (enemyMove.targetDistance <= enemyMove.stopDistance)
        {

        }
    }

    void AttackTarget()
    {
        if (isPreformingAction)
            return;

        if (currentAttack == null)
        {
            GetNewAttack();
        }
        else
        {
            isPreformingAction = true;
            enemyAnimator.PlayTargetAnimation(currentAttack.actionAnimation, true);
        }
    }

    void GetNewAttack()
    {
        Vector3 targetDir = enemyMove.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDir, transform.forward);
        enemyMove.targetDistance = Vector3.Distance(enemyMove.currentTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyMove.targetDistance <= enemyAttackAction.attackDisMax && enemyMove.targetDistance >= enemyAttackAction.attackDisMin
                && viewableAngle <= enemyAttackAction.attackAngleMax && viewableAngle >= enemyAttackAction.attackAngleMin)
            {
                maxScore += enemyAttackAction.attackScore;
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int tempScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (enemyMove.targetDistance <= enemyAttackAction.attackDisMax && enemyMove.targetDistance >= enemyAttackAction.attackDisMin
                && viewableAngle <= enemyAttackAction.attackAngleMax && viewableAngle >= enemyAttackAction.attackAngleMin)
            {
                if (currentAttack != null)
                    return;

                tempScore += enemyAttackAction.attackScore;

                if (tempScore > randomValue)
                {
                    currentAttack = enemyAttackAction;
                }
            }
        }
    }

    void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }
        else if (isPreformingAction)
        {
            isPreformingAction = false;
        }
    }
}
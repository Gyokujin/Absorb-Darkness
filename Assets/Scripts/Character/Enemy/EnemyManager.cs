using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
    [Header("State")]
    public bool isPreformingAction;

    [Header("Attack")]
    public float currentRecoveryTime = 0;

    [Header("Component")]
    public CharacterStatus currentTarget;
    public EnemyState curState;
    private EnemyMove enemyMove;
    private EnemyAnimator enemyAnimator;
    EnemyStatus enemyStatus;

    void Awake()
    {
        enemyMove = GetComponent<EnemyMove>();
        enemyStatus = GetComponent<EnemyStatus>();
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
    }

    void Update()
    {
        HandleRecoveryTimer();
    }

    void FixedUpdate()
    {
        HandleStateMachine();
    }

    void HandleStateMachine()
    {
        if (curState != null)
        {
            EnemyState nextState = curState.Tick(this, enemyStatus, enemyAnimator);

            if (nextState != null)
            {
                SwitchNextState(nextState);
            }
        }
    }

    void SwitchNextState(EnemyState state)
    {
        curState = state;
    }

    void SelectAttackAction()
    {
        //if (isPreformingAction)
        //    return;

        //if (currentAttack == null)
        //{
        //    GetNewAttack();
        //}
        //else
        //{
        //    Attack();
        //}
    }

    void GetNewAttack()
    {
        //Vector3 targetDir = enemyMove.currentTarget.transform.position - transform.position;
        //float viewableAngle = Vector3.Angle(targetDir, transform.forward);
        //enemyMove.targetDistance = Vector3.Distance(enemyMove.currentTarget.transform.position, transform.position);

        //int maxScore = 0;

        //for (int i = 0; i < enemyAttacks.Length; i++)
        //{
        //    EnemyAttackAction enemyAttackAction = enemyAttacks[i];

        //    if (enemyMove.targetDistance <= enemyAttackAction.attackDisMax && enemyMove.targetDistance >= enemyAttackAction.attackDisMin &&
        //        viewableAngle <= enemyAttackAction.attackAngleMax && viewableAngle >= enemyAttackAction.attackAngleMin)
        //    {
        //        maxScore += enemyAttackAction.attackScore;
        //    }
        //}

        //int randomValue = Random.Range(0, maxScore);
        //int tempScore = 0;

        //for (int i = 0; i < enemyAttacks.Length; i++)
        //{
        //    EnemyAttackAction enemyAttackAction = enemyAttacks[i];

        //    if (enemyMove.targetDistance <= enemyAttackAction.attackDisMax && enemyMove.targetDistance >= enemyAttackAction.attackDisMin &&
        //        viewableAngle <= enemyAttackAction.attackAngleMax && viewableAngle >= enemyAttackAction.attackAngleMin)
        //    {
        //        if (currentAttack != null)
        //            return;

        //        tempScore += enemyAttackAction.attackScore;

        //        if (tempScore > randomValue)
        //        {
        //            currentAttack = enemyAttackAction;
        //        }
        //    }
        //}
    }

    void Attack()
    {
        //isPreformingAction = true;
        //currentRecoveryTime = currentAttack.recoveryTime;
        //enemyAnimator.PlayTargetAnimation(currentAttack.actionAnimation, true);
        //currentAttack = null;
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
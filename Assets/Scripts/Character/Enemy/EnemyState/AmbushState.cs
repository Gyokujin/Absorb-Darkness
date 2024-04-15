using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : EnemyState
{
    [Header("Ambush")]
    private bool isSleeping = true;
    [SerializeField]
    private string sleepAnimation;
    [SerializeField]
    private string wakeAnimation;
    [SerializeField]
    private float detectionRadius = 2;

    [Header("States")]
    [SerializeField]
    private PursueTargetState pursueTargetState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        if (isSleeping && !enemyManager.isInteracting)
        {
            enemyAnimator.PlayTargetAnimation(sleepAnimation, true);
        }

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, enemyManager.detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatus characterStatus = colliders[i].transform.GetComponent<CharacterStatus>();
            
            if (characterStatus != null)
            {
                Vector3 targetDir = characterStatus.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetDir, enemyManager.transform.forward);
                
                if (viewableAngle > enemyStatus.detectionAngleMin && viewableAngle < enemyStatus.detectionAngleMax)
                {
                    enemyManager.currentTarget = characterStatus;
                    isSleeping = false;
                    enemyAnimator.PlayTargetAnimation(wakeAnimation, true);
                }
            }
        }

        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
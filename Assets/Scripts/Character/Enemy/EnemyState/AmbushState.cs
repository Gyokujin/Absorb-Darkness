using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : EnemyState
{
    public bool isIdle;
    public string idleAnimation;
    public string wakeAnimation;
    public float detectionRadius = 2;
    public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        if (isIdle && !enemyManager.isPreformingAction)
        {
            enemyAnimator.PlayTargetAnimation(idleAnimation, true);
        }

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatus characterStatus = colliders[i].transform.GetComponent<CharacterStatus>();
            
            if (characterStatus != null)
            {
                Vector3 targetDir = characterStatus.transform.position - enemyManager.transform.position;
                enemyStatus.viewableAngle = Vector3.Angle(targetDir, enemyManager.transform.forward);
                
                if (enemyStatus.viewableAngle > enemyStatus.detectionAngleMin && enemyStatus.viewableAngle < enemyStatus.detectionAngleMax)
                {
                    enemyManager.currentTarget = characterStatus;
                    isIdle = false;
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
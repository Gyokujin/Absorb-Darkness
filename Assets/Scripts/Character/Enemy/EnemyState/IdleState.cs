using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    [Header("States")]
    public PursueTargetState pursueTargetState;

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        if (enemyManager.onDamage)
            return this;

        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyStatus.detectionRadius, enemyManager.detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStatus characterStatus = colliders[i].transform.GetComponent<CharacterStatus>();

            if (characterStatus != null)
            {
                Vector3 targetDirection = Vector3.Normalize(characterStatus.transform.position - transform.position);
                bool angleAble = Vector3.Angle(targetDirection, transform.forward) > enemyStatus.detectionAngleMin && 
                    Vector3.Angle(targetDirection, transform.forward) < enemyStatus.detectionAngleMax;

                bool showAble = false;
                RaycastHit hit;

                if (Physics.Raycast(enemyManager.lockOnTransform.position, targetDirection, out hit, enemyStatus.detectionRadius))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        showAble = true;
                    }
                }

                if (angleAble && showAble) // && showAble
                {
                    enemyManager.currentTarget = characterStatus;
                }
            }
        }

        if (enemyManager.currentTarget != null)
        {
            EnemyAudio enemyAudio = enemyManager.enemyAudio;
            enemyAudio.PlaySFX(enemyAudio.enemyClips[(int)EnemyAudio.EnemySound.Detect]);
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
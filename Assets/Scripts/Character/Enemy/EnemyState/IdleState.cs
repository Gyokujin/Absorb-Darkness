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
                Vector3 targetDirection = characterStatus.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyStatus.detectionAngleMin && viewableAngle < enemyStatus.detectionAngleMax)
                {
                    enemyManager.currentTarget = characterStatus;
                }
            }
        }

        if (enemyManager.currentTarget != null)
        {
            CharacterAudio characterAudio = enemyManager.characterAudio;
            characterAudio.PlaySFX(characterAudio.audioClips[(int)CharacterSound.Detect]);
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : EnemyState
{
    [Header("Ambush")]
    private bool isSleeping = true;
    [SerializeField]
    private float detectionRadius = 2;

    public override EnemyState Tick()
    {
        if (isSleeping && !enemy.isInteracting)
            enemy.enemyAnimator.PlayTargetAnimation(enemy.characterAnimatorData.SleepAnimation, true);

        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, detectionRadius, enemy.detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetComponent<CharacterStatus>() != null)
            {
                CharacterStatus characterStatus = colliders[i].transform.GetComponent<CharacterStatus>();
                Vector3 targetDir = characterStatus.transform.position - enemy.transform.position;
                float viewableAngle = Vector3.Angle(targetDir, enemy.transform.forward);

                if (viewableAngle > enemy.enemyStatus.detectionAngleMin && viewableAngle < enemy.enemyStatus.detectionAngleMax)
                {
                    enemy.currentTarget = characterStatus;
                    isSleeping = false;
                    enemy.enemyAnimator.PlayTargetAnimation(enemy.characterAnimatorData.WakeAnimation, true);
                }
            }
        }

        if (enemy.currentTarget != null)
            return enemy.pursueTargetState;
        else
            return this;
    }
}
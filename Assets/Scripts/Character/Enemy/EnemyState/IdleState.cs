using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    public override EnemyState Tick()
    {
        if (enemy.onDamage)
            return this;

        Collider[] colliders = Physics.OverlapSphere(transform.position, enemy.enemyStatus.detectionRadius, enemy.detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<CharacterStatus>() != null)
            {
                CharacterStatus characterStatus = colliders[i].transform.GetComponent<CharacterStatus>();
                Vector3 targetDirection = Vector3.Normalize(characterStatus.transform.position - transform.position);
                bool angleAble = Vector3.Angle(targetDirection, transform.forward) > enemy.enemyStatus.detectionAngleMin && Vector3.Angle(targetDirection, transform.forward) < enemy.enemyStatus.detectionAngleMax;
                bool showAble = false;

                if (Physics.Raycast(enemy.lockOnTransform.position, targetDirection, out RaycastHit hit, enemy.enemyStatus.detectionRadius))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer(enemy.layerData.PlayerLayer))
                        showAble = true;
                }

                if (angleAble && showAble)
                    enemy.currentTarget = characterStatus;
            }
        }

        if (enemy.currentTarget != null)
        {
            EnemyAudio enemyAudio = enemy.enemyAudio;
            enemyAudio.PlaySFX(enemyAudio.enemyClips[(int)EnemyAudio.EnemySound.Detect]);
            return enemy.pursueTargetState;
        }
        else
            return this;
    }
}
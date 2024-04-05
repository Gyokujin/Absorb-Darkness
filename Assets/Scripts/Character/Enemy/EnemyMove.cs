using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyMove : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask detectionLayer;
    public CharacterStats currentTarget;

    [Header("Component")]
    private EnemyManager enemyManager;

    void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    void OnDrawGizmosSelected()
    {
        Color gizmoColor = new Color(255, 0, 0, 0.3f);
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, enemyManager.detectionRadius);
    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyManager.detectionAngleMin && viewableAngle < enemyManager.detectionAngleMax)
                {
                    currentTarget = characterStats;
                }
            }
        }
    }
}
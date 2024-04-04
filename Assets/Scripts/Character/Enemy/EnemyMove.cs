using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public LayerMask detectionLayer;

    private EnemyManager enemyManager;

    void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    public void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {

        }
    }
}
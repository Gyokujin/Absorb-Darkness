using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("Component")]
    private EnemyManager enemyManager;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    void Start()
    {
        Physics.IgnoreCollision(enemyManager.collider, enemyManager.blockerCollider, true);
    }
}
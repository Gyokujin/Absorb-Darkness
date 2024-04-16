using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyMove : MonoBehaviour
{
    [Header("Component")]
    private EnemyManager enemyManager;

    //[SerializeField]
    //private Collider enemyCollider;
    //[SerializeField]
    //private Collider enemyBlockerCollider;
    //private EnemyAnimator enemyAnimator;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        enemyManager = GetComponent<EnemyManager>();
        // enemyAnimator = GetComponentInChildren<EnemyAnimator>();
    }

    void Start()
    {
        Physics.IgnoreCollision(enemyManager.collider, enemyManager.blockerCollider, true);
    }
}
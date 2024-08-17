using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyManager enemy;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        enemy = GetComponentInParent<EnemyManager>();
    }

    public abstract EnemyState Tick();
}
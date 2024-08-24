using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyData;

public class MeteorExplosion : MonoBehaviour
{
    [Header("Data")]
    private SorceressData sorceressData;

    [Header("Component")]
    private new Collider collider;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        collider = GetComponent<Collider>();
    }

    void OnEnable()
    {
        collider.enabled = true;
        Invoke(nameof(OffCollider), sorceressData.ExplosionRetentionTime);
    }

    void OffCollider()
    {
        collider.enabled = false;
    }
}
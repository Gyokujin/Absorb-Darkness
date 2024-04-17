using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorceress : MonoBehaviour
{
    [Header("Meteor")]
    [SerializeField]
    private Transform meteorTransform;
    [SerializeField]
    private float meteorFallSpeed = 1.2f;

    [Header("Compnent")]
    private EnemyManager enemyManager;
    private Meteor meteor;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    public void SpawnMeteor()
    {
        meteor = PoolManager.instance.GetSpell(0).GetComponent<Meteor>();
        meteor.transform.position = meteorTransform.position;

        Quaternion rotation = Quaternion.LookRotation(enemyManager.currentTarget.transform.position);
        meteor.gameObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    public void FallMeteor()
    {
        Vector3 attackDir = Vector3.Normalize(enemyManager.currentTarget.transform.position - meteor.transform.position);
        meteor.Falling(attackDir, meteorFallSpeed);
    }
}
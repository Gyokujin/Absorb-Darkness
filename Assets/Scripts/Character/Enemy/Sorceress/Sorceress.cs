using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorceress : MonoBehaviour
{
    [Header("Meteor")]
    [SerializeField]
    private Meteor meteor;
    [SerializeField]
    private GameObject meteorObject;
    [SerializeField]
    private Transform MeteorTransform;

    [Header("Compnent")]
    private EnemyManager enemyManager;

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
        meteor = Instantiate(meteorObject, MeteorTransform.position, Quaternion.identity).GetComponent<Meteor>();
    }

    public void FallMeteor()
    {
        Vector3 attackDir = Vector3.Normalize(enemyManager.currentTarget.transform.position - meteor.transform.position);
        meteor.Falling(attackDir);
    }
}
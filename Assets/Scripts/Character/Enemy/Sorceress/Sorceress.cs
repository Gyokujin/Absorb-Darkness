using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorceress : MonoBehaviour
{
    [Header("Spell")]
    private Meteor meteor;
    private LightningImpact lightningImpact;

    [Header("Meteor")]
    [SerializeField]
    private Transform meteorTransform;
    [SerializeField]
    private float meteorFallSpeed = 1.2f;

    [Header("LightningImpact")]
    [SerializeField]
    private Transform lightningImpactTransform;
    [SerializeField]
    private float lightningImpactSpeed = 12;

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
        meteor = PoolManager.instance.GetSpell(0).GetComponent<Meteor>();
        meteor.transform.position = meteorTransform.position;
        Quaternion rotation = Quaternion.LookRotation(enemyManager.currentTarget.transform.position);
        meteor.gameObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    public void FallMeteor()
    {
        Vector3 fallDir = Vector3.Normalize(enemyManager.currentTarget.transform.position - meteor.transform.position);
        meteor.Falling(fallDir, meteorFallSpeed);
    }

    public void SpawnLightning()
    {
        lightningImpact = PoolManager.instance.GetSpell(2).GetComponent<LightningImpact>();
        lightningImpact.transform.position = lightningImpactTransform.position;
        Quaternion spellRotation = Quaternion.LookRotation(enemyManager.currentTarget.transform.position - transform.position);
        lightningImpact.transform.rotation = spellRotation;
    }

    public void ShootLightning()
    {
        Vector3 shootDir = Vector3.Normalize(enemyManager.currentTarget.transform.position - lightningImpact.transform.position);
        lightningImpact.Shoot(shootDir, lightningImpactSpeed);
    }
}
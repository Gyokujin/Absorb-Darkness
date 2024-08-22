using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyData;

public class Sorceress : EnemyManager
{
    [Header("Data")]
    private SorceressData sorceressData;

    [Header("Spell")]
    private LightningImpact lightningImpact;
    private Meteor[] spawnMeteors;
    private int summonCount;

    [Header("Spell Transform")]
    [SerializeField]
    private Transform lightningImpactTransform;
    [SerializeField]
    private Transform poisonMistTransform;
    [SerializeField]
    private Transform[] summonTransforms;
    [SerializeField]
    private Transform[] meteorTransform;

    [Header("Coroutine")]
    private WaitForSeconds summonWait;
    private WaitForSeconds meteorFallWait;

    void Awake()
    {
        base.Init();
        Init();
    }

    protected override void Init()
    {
        spawnMeteors = new Meteor[meteorTransform.Length];
        summonWait = new WaitForSeconds(sorceressData.SummonDelay);
        meteorFallWait = new WaitForSeconds(sorceressData.MeteorFallDelay);
    }

    public void SpawnLightning()
    {
        lightningImpact = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.LightningImpact).GetComponent<LightningImpact>();
        lightningImpact.transform.position = lightningImpactTransform.position;
        Quaternion spellRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
        lightningImpact.transform.SetPositionAndRotation(lightningImpactTransform.position, spellRotation);
    }

    public void ShootLightning()
    {
        Vector3 shootDir = Vector3.Normalize(currentTarget.transform.position - lightningImpactTransform.position + Vector3.up * sorceressData.LightningImpactOffsetY);
        float angle = Vector3.Angle(transform.forward, shootDir);

        if (angle > sorceressData.LightningImpactRotateLimit) // 만약 범위 밖으로 벗어난다면 정면으로 쏘게한다.
            shootDir = transform.forward;

        lightningImpact.Shoot(shootDir, sorceressData.LightningImpactSpeed);
    }

    public void PoisonMist()
    {
        GameObject poisonMist = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.PosionMist);
        poisonMist.transform.position = poisonMistTransform.position;
    }

    public IEnumerator SummonBat()
    {
        summonCount++;
        Vector3 summonPos = summonTransforms[summonCount % summonTransforms.Length].position;
        GameObject summonSFX = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.Summon);
        summonSFX.transform.position = summonPos;

        yield return summonWait;
        GameObject summonBat = PoolManager.instance.GetEnemy((int)PoolManager.Enemy.Bat);
        summonPos.y += sorceressData.SummonOffsetY; // 몬스터 오브젝트의 Y좌표는 0
        summonBat.transform.SetPositionAndRotation(summonPos, transform.rotation);
    }

    public void SpawnMeteors()
    {
        for (int i = 0; i < spawnMeteors.Length; i++)
        {
            spawnMeteors[i] = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.Meteor).GetComponent<Meteor>();
            Quaternion rotation = Quaternion.LookRotation(currentTarget.transform.position);
            spawnMeteors[i].transform.SetPositionAndRotation(meteorTransform[i].position, rotation);
        }
    }

    public IEnumerator FallMeteors()
    {
        foreach (Meteor meteor in spawnMeteors)
        {
            Vector3 fallDir = Vector3.Normalize(currentTarget.transform.position - meteor.transform.position);
            meteor.Falling(fallDir, sorceressData.MeteorFallSpeed);
            yield return meteorFallWait;
        }
    }
}
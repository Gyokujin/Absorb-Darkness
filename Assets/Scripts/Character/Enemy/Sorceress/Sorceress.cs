using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorceress : MonoBehaviour
{
    [Header("Spell")]
    private LightningImpact lightningImpact;
    private Meteor[] meteors;

    [Header("LightningImpact")]
    [SerializeField]
    private Transform lightningImpactTransform;
    [SerializeField]
    private float lightningImpactOffsetY = 0.5f;
    [SerializeField]
    private float lightningImpactSpeed = 12;
    [SerializeField]
    private float lightningImpactRotateLimit = 60;

    [Header("PoisonMist")]
    [SerializeField]
    private Transform poisonMistTransform;

    [Header("Summon")]
    [SerializeField]
    private Transform[] summonTransforms;
    private int summonCount;
    [SerializeField]
    private float summonDelay = 1f;
    private WaitForSeconds summonWait;

    [Header("Meteor")]
    [SerializeField]
    private Transform[] meteorTransform;
    [SerializeField]
    private float meteorFallSpeed = 1.2f;
    [SerializeField]
    private float meteorFallDelay = 0.5f;
    private WaitForSeconds meteorFallWait;

    [Header("Compnent")]
    private EnemyManager enemyManager;
    private CharacterAudio characterAudio;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        enemyManager = GetComponent<EnemyManager>();
        characterAudio = GetComponent<CharacterAudio>();
    }

    void Start()
    {
        meteors = new Meteor[meteorTransform.Length];
        meteorFallWait = new WaitForSeconds(meteorFallDelay);
        summonWait = new WaitForSeconds(summonDelay);
    }

    public void SpawnLightning()
    {
        lightningImpact = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.LightningImpact).GetComponent<LightningImpact>();
        lightningImpact.transform.position = lightningImpactTransform.position;
        Quaternion spellRotation = Quaternion.LookRotation(enemyManager.currentTarget.transform.position - transform.position);
        lightningImpact.transform.rotation = spellRotation;
        characterAudio.PlaySFX(characterAudio.audioClips[4]);
    }

    public void ShootLightning()
    {
        Vector3 shootDir = Vector3.Normalize(enemyManager.currentTarget.transform.position - lightningImpactTransform.position + Vector3.up * lightningImpactOffsetY);
        float angle = Vector3.Angle(transform.forward, shootDir);

        if (angle > lightningImpactRotateLimit) // 만약 범위 밖으로 벗어난다면 정면으로 쏘게한다.
        {
            shootDir = transform.forward;
        }

        lightningImpact.Shoot(shootDir, lightningImpactSpeed);
        characterAudio.PlaySFX(characterAudio.audioClips[5]);
    }

    public void PoisonMist()
    {
        GameObject poisonMist = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.PosionMist);
        poisonMist.transform.position = poisonMistTransform.position;
    }

    public IEnumerator SummonBat()
    {
        summonCount++;
        Vector3 summonPos = summonTransforms[summonCount % 2].position;
        GameObject summonSFX = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.Summon);
        summonSFX.transform.position = summonPos;

        yield return summonWait;
        GameObject summonBat = PoolManager.instance.GetEnemy((int)PoolManager.Enemy.Bat);
        summonBat.transform.position = summonPos;
        summonBat.transform.rotation = transform.rotation;
    }

    public void SpawnMeteors()
    {
        for (int i = 0; i < meteors.Length; i++)
        {
            meteors[i] = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.Meteor).GetComponent<Meteor>();
            meteors[i].transform.position = meteorTransform[i].position;
            Quaternion rotation = Quaternion.LookRotation(enemyManager.currentTarget.transform.position);
            meteors[i].gameObject.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }
    }

    public IEnumerator FallMeteors()
    {
        foreach (Meteor meteor in meteors)
        {
            Vector3 fallDir = Vector3.Normalize(enemyManager.currentTarget.transform.position - meteor.transform.position);
            meteor.Falling(fallDir, meteorFallSpeed);
            yield return meteorFallWait;
        }
    }
}
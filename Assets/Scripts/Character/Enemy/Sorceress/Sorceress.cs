using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorceress : MonoBehaviour
{
    [Header("Spell")]
    [SerializeField]
    private Meteor[] meteors;
    private LightningImpact lightningImpact;

    [Header("Meteor")]
    [SerializeField]
    private Transform[] meteorTransform;
    [SerializeField]
    private float meteorFallSpeed = 1.2f;
    [SerializeField]
    private float meteorFallDelay = 0.5f;

    [Header("LightningImpact")]
    [SerializeField]
    private Transform lightningImpactTransform;
    [SerializeField]
    private float lightningImpactOffsetY = 0.5f;
    [SerializeField]
    private float lightningImpactSpeed = 12;

    [Header("PoisonMist")]
    [SerializeField]
    private Transform poisonMistTransform;

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

    void Start()
    {
        meteors = new Meteor[meteorTransform.Length];
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
            yield return new WaitForSeconds(meteorFallDelay);
        }
    }

    public void SpawnLightning()
    {
        lightningImpact = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.LightningImpact).GetComponent<LightningImpact>();
        lightningImpact.transform.position = lightningImpactTransform.position;
        Quaternion spellRotation = Quaternion.LookRotation(enemyManager.currentTarget.transform.position - transform.position);
        lightningImpact.transform.rotation = spellRotation;
    }

    public void ShootLightning()
    {
        Vector3 shootDir = Vector3.Normalize(enemyManager.currentTarget.transform.position - lightningImpact.transform.position + new Vector3(0, lightningImpactOffsetY, 0));
        lightningImpact.Shoot(shootDir, lightningImpactSpeed);
    }

    public void PoisonMist()
    {
        GameObject poisonMist = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.PosionMist);
        poisonMist.transform.position = poisonMistTransform.position;
    }
}
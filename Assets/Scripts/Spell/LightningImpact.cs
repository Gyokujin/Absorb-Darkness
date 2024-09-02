using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyData;
using SystemData;

public class LightningImpact : MonoBehaviour
{
    [Header("Data")]
    private SorceressData sorceressData;
    private GameObjectData gameObjectData;

    [Header("Shooting")]
    private float curRetentionTime;
    [SerializeField]
    private GameObject lightningEffect;
    private LayerMask targetLayer;
    private LayerMask environmentLayer;

    [Header("Component")]
    private new Collider collider;
    private new Rigidbody rigidbody;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        targetLayer = LayerMask.NameToLayer(gameObjectData.PlayerLayer);
        environmentLayer = LayerMask.NameToLayer(gameObjectData.EnvironmentLayer);
    }

    void OnEnable()
    {
        collider.enabled = false;
        curRetentionTime = sorceressData.LightningImpactRetentionTime;
    }

    void Update()
    {
        if (curRetentionTime > 0)
            curRetentionTime -= Time.deltaTime;
        else
            Return();
    }

    public void Shoot(Vector3 shootDir, float speed)
    {
        collider.enabled = true;
        curRetentionTime = sorceressData.LightningImpactRetentionTime;
        rigidbody.velocity = shootDir * speed;
        lightningEffect.SetActive(true);
    }

    IEnumerator ElectricShock()
    {
        rigidbody.velocity = Vector3.zero;
        GameObject electricShock = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.ElectricShock);
        electricShock.transform.position = transform.position;

        yield return null;
        Return();
    }

    void Return()
    {
        rigidbody.velocity = Vector3.zero;
        lightningEffect.SetActive(false);
        PoolManager.instance.Return(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetLayer)
            StartCoroutine(ElectricShock());
        else if (other.gameObject.layer == environmentLayer)
            Return();
    }
}
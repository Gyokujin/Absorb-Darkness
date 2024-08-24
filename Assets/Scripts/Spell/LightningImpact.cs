using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyData;
using SystemData;

public class LightningImpact : MonoBehaviour
{
    [Header("Data")]
    private SorceressData sorceressData;
    private LayerData layerData;

    [Header("Shooting")]
    private bool onShooting;
    private float curRetentionTime;
    [SerializeField]
    private GameObject lightningEffect;
    private LayerMask targetLayer;
    private LayerMask groundLayer;

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
        targetLayer = LayerMask.NameToLayer(layerData.PlayerLayer);
        groundLayer = LayerMask.NameToLayer(layerData.GroundLayer);
    }

    void OnEnable()
    {
        onShooting = false;
        collider.enabled = false;
    }

    void Update()
    {
        if (onShooting)
        {
            if (curRetentionTime > 0)
                curRetentionTime -= Time.deltaTime;
            else
                Return();
        }
    }

    public void Shoot(Vector3 shootDir, float speed)
    {
        onShooting = true;
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
        else if (other.gameObject.layer == groundLayer)
            Return();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyData;
using SystemData;

public class Meteor : MonoBehaviour
{
    [Header("Data")]
    private SorceressData sorceressData;
    private GameObjectData gameObjectData;

    [Header("Falling")]
    [SerializeField]
    private GameObject fireEffect;
    [SerializeField]
    private GameObject smokeEffect;
    private float curFallTime;
    private LayerMask groundLayer;

    [Header("Component")]
    private new Rigidbody rigidbody;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        groundLayer = LayerMask.NameToLayer(gameObjectData.GroundLayer);
    }

    void OnEnable()
    {
        curFallTime = sorceressData.MeteorRetentionTime;
    }

    void Update()
    {
        if (curFallTime > 0)
            curFallTime -= Time.deltaTime;
        else
            Return();
    }

    public void Falling(Vector3 fallDir, float speed)
    {
        rigidbody.velocity = fallDir * speed;
        fireEffect.SetActive(true);
        smokeEffect.SetActive(true);
    }

    IEnumerator Explosion()
    {
        GameObject explosion = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.MeteorExplosion);
        explosion.transform.position = transform.position;
        fireEffect.SetActive(false);
        smokeEffect.SetActive(false);

        yield return null;
        Return();
    }

    void Return()
    {
        rigidbody.velocity = Vector3.zero;
        PoolManager.instance.Return(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == groundLayer)
            StartCoroutine(Explosion());
    }
}
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
    private bool onFalling;
    private float curFallTime;
    [SerializeField]
    private GameObject fireEffect;
    [SerializeField]
    private GameObject smokeEffect;
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
        onFalling = false;
    }

    void Update()
    {
        if (onFalling)
        {
            if (curFallTime > 0)
                curFallTime -= Time.deltaTime;
            else
                Return();
        }
    }

    public void Falling(Vector3 fallDir, float speed)
    {
        onFalling = true;
        curFallTime = sorceressData.MeteorRetentionTime;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("Falling")]
    [SerializeField]
    private float fallTime = 20; // 메테오 풀링 버그방지용
    private float curFallTime = 10;
    [SerializeField]
    private GameObject fireEffect;
    [SerializeField]
    private GameObject smokeEffect;

    [Header("Attack")]
    [SerializeField]
    private int groundLayer = 6;

    [Header("Component")]
    private new Rigidbody rigidbody;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (curFallTime > 0)
        {
            curFallTime -= Time.deltaTime;
        }
        else
        {
            Return();
        }
    }

    public void Falling(Vector3 fallDir, float speed)
    {
        curFallTime = fallTime;
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
        {
            StartCoroutine("Explosion");
        }
    }
}
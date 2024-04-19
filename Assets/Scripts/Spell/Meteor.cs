using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("Falling")]
    [SerializeField]
    private float fallTime = 20; // 메테오 풀링 버그방지용
    private bool onFalling;
    private float curFallTime;

    [Header("Component")]
    private new Rigidbody rigidbody;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (onFalling)
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
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Falling(Vector3 fallDir, float speed)
    {
        onFalling = true;
        curFallTime = fallTime;
        rigidbody.velocity = fallDir * speed;
    }

    IEnumerator Explosion()
    {
        GameObject explosion = PoolManager.instance.GetEnemySpell((int)PoolManager.EnemySpell.MeteorExplosion);
        explosion.transform.position = transform.position;
        yield return null;
        Return();
    }

    void Return()
    {
        onFalling = false;
        rigidbody.velocity = Vector3.zero;
        PoolManager.instance.Return(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.layer == 6)
        {
            StartCoroutine("Explosion");
        }
    }
}
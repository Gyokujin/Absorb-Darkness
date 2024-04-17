using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private new Rigidbody rigidbody;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Falling(Vector3 shootDir, float speed)
    {
        rigidbody.velocity = shootDir * speed;
    }

    IEnumerator Explosion()
    {
        GameObject explosion = PoolManager.instance.GetSpell(1);
        explosion.transform.position = transform.position;
        rigidbody.velocity = Vector3.zero;

        yield return null;
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
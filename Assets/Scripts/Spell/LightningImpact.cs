using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningImpact : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField]
    private float shootTime = 20; // 메테오 풀링 버그방지용
    private float curShootTime;
    private bool onShooting;

    [Header("Component")]
    [SerializeField]
    private GameObject lightningEffect;
    private new Rigidbody rigidbody;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (onShooting)
        {
            if (curShootTime > 0)
            {
                curShootTime -= Time.deltaTime;
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

    public void Shoot(Vector3 shootDir, float speed)
    {
        onShooting = true;
        curShootTime = shootTime;
        lightningEffect.SetActive(true);
        rigidbody.velocity = shootDir * speed;
    }

    IEnumerator ElectricShock()
    {
        GameObject electricShock = PoolManager.instance.GetSpell(3);
        electricShock.transform.position = transform.position;
        rigidbody.velocity = Vector3.zero;

        yield return null;
        Return();
    }

    void Return()
    {
        onShooting = false;
        rigidbody.velocity = Vector3.zero;
        lightningEffect.SetActive(false);
        PoolManager.instance.Return(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            StartCoroutine("ElectricShock");
        }
        else if (other.gameObject.layer == 6)
        {
            Return();
        }
    }
}
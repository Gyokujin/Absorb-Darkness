using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningImpact : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField]
    private float shootTime = 20;
    private float curShootTime;
    [SerializeField]
    private bool onShooting;

    [Header("Attack")]
    private int playerLayer = 3;
    private int groundLayer = 6;

    [Header("Component")]
    [SerializeField]
    private GameObject lightningEffect;
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
    }

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
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

    public void Shoot(Vector3 shootDir, float speed)
    {
        onShooting = true;
        collider.enabled = true;
        curShootTime = shootTime;
        lightningEffect.SetActive(true);
        rigidbody.velocity = shootDir * speed;
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
        if (other.gameObject.layer == playerLayer)
        {
            StartCoroutine(ElectricShock());
        }
        else if (other.gameObject.layer == groundLayer)
        {
            Return();
        }
    }
}
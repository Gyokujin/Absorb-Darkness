using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningImpact : MonoBehaviour
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

    public void Shoot(Vector3 shootDir, float speed)
    {
        rigidbody.velocity = shootDir * speed;
    }
}
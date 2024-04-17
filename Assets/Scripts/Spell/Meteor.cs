using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    private float fallSpeed = 1.2f;
    private new Rigidbody rigidbody;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Falling(Vector3 shootDir)
    {
        rigidbody.velocity = shootDir * fallSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.layer == 6)
        {

        }
    }
}
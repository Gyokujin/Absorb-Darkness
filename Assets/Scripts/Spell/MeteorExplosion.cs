using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorExplosion : MonoBehaviour
{
    private new Collider collider;

    void Awake()
    {
        collider = GetComponent<Collider>();
    }

    void OnEnable()
    {
        collider.enabled = true;
        Invoke("OffCollider", 0.05f);
    }

    void OffCollider()
    {
        collider.enabled = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class StayDamageCollider : MonoBehaviour
{
    [Header("Data")]
    private GameObjectData gameObjectData;

    [Header("Attack")]
    [SerializeField]
    private int damage = 1;
    private LayerMask targetLayer;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        targetLayer = LayerMask.NameToLayer(gameObjectData.PlayerLayer);
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == targetLayer && collision.GetComponent<PlayerStatus>() != null)
            collision.gameObject.GetComponent<PlayerStatus>().TakeDamage(damage, false);
    }
}
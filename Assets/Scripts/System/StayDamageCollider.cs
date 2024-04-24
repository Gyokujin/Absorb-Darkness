using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayDamageCollider : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    private int targetLayer;

    void Start()
    {
        targetLayer = LayerMask.NameToLayer("Player");
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == targetLayer)
        {
            PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();

            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damage, false);
            }
        }
    }
}
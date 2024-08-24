using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayDamageCollider : DamageCollider
{
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == targetLayer && collision.GetComponent<PlayerStatus>() != null)
            collision.gameObject.GetComponent<PlayerStatus>().TakeDamage(damage, false);
    }
}
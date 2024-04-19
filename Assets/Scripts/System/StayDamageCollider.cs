using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayDamageCollider : MonoBehaviour
{
    [SerializeField]
    private int damage = 3;

    void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();

            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damage, false);
            }
        }
    }
}
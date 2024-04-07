using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damage = 25;

    void OnCollisionEnter(Collision collision)
    {
        PlayerStatus playerStatus = collision.gameObject.GetComponent<PlayerStatus>();

        if (playerStatus != null)
        {
            playerStatus.TakeDamage(damage);
        }
    }
}
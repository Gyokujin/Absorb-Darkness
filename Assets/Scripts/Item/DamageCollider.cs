using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageCollider : MonoBehaviour
{
    [Header("Weapon Info")]
    [SerializeField]
    private int weaponDamage = 25;
    private Collider damageCollider;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void AbleDamageCollider(bool able)
    {
        damageCollider.enabled = able;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();

            if (playerStatus != null)
            {
                playerStatus.TakeDamage(weaponDamage);
            }
        }

        if (collision.tag == "Enemy")
        {
            EnemyStatus enemyStatus = collision.GetComponent<EnemyStatus>();

            if (enemyStatus != null)
            {
                enemyStatus.TakeDamage(weaponDamage, GetComponentInParent<CharacterStatus>());
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageCollider : MonoBehaviour
{
    public enum AttackType
    {
        PlayerWeapon, PlayerSpell, EnemyWeapon, EnemySpell
    }

    [SerializeField]
    private AttackType attackType;

    [Header("Weapon Info")]
    private Collider damageCollider;
    [SerializeField]
    private int damage = 10;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;

        if (attackType == AttackType.PlayerWeapon || attackType == AttackType.EnemyWeapon)
        {
            damageCollider.enabled = false;
        }
    }

    public void AbleDamageCollider(bool able)
    {
        damageCollider.enabled = able;
    }

    void OnTriggerEnter(Collider collision)
    {
        switch (attackType)
        {
            case AttackType.PlayerWeapon:
            case AttackType.PlayerSpell:
                if (collision.tag == "Enemy")
                {
                    EnemyStatus enemyStatus = collision.GetComponent<EnemyStatus>();

                    if (enemyStatus != null)
                    {
                        enemyStatus.TakeDamage(damage, GetComponentInParent<CharacterStatus>());
                    }
                }

                break;

            case AttackType.EnemyWeapon:
            case AttackType.EnemySpell:
                if (collision.tag == "Player")
                {
                    PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();

                    if (playerStatus != null)
                    {
                        playerStatus.TakeDamage(damage);
                    }
                }

                break;
        }
    }
}
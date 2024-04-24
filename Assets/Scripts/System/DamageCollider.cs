using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageCollider : MonoBehaviour
{
    public enum AttackType
    {
        PlayerWeapon, PlayerSpell, EnemyUnarmed, EnemyWeapon, EnemySpell
    }

    [Header("Attack")]
    [SerializeField]
    private AttackType attackType;
    [SerializeField]
    private int targetLayer;

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

        if (attackType == AttackType.EnemyUnarmed || attackType == AttackType.PlayerWeapon || attackType == AttackType.EnemyWeapon)
        {
            damageCollider.enabled = false;
        }
    }

    void Start()
    {
        switch (attackType)
        {
            case AttackType.PlayerWeapon:
            case AttackType.PlayerSpell:
                targetLayer = LayerMask.NameToLayer("Enemy");
                break;

            case AttackType.EnemyUnarmed:
            case AttackType.EnemyWeapon:
            case AttackType.EnemySpell:
                targetLayer = LayerMask.NameToLayer("Player");
                break;
        }
    }

    public void OpenDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void CloseDamageCollider()
    {
        damageCollider.enabled = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == targetLayer)
        {
            switch (attackType)
            {
                case AttackType.PlayerWeapon:
                case AttackType.PlayerSpell:
                    EnemyStatus enemyStatus = collision.GetComponent<EnemyStatus>();

                    if (enemyStatus != null)
                    {
                        enemyStatus.TakeDamage(damage, GetComponentInParent<CharacterStatus>());
                    }

                    break;

                case AttackType.EnemyUnarmed:
                case AttackType.EnemyWeapon:
                case AttackType.EnemySpell:
                    PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();

                    if (playerStatus != null)
                    {
                        playerStatus.TakeDamage(damage, true);
                    }

                    break;
            }
        }
    }
}
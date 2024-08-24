using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

[RequireComponent(typeof(Collider))]
public class DamageCollider : MonoBehaviour
{
    public enum AttackType
    {
        PlayerWeapon, PlayerSpell, EnemyUnarmed, EnemyWeapon, EnemySpell
    }

    [SerializeField]
    private AttackType attackType;

    [Header("Data")]
    private GameObjectData gameObjectData;

    [Header("Attack")]
    [SerializeField]
    private int damage = 10;
    private Collider damageCollider;
    [SerializeField]
    private TrailRenderer trailRenderer;
    private LayerMask targetLayer;

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
            damageCollider.enabled = false;
    }

    void Start()
    {
        switch (attackType)
        {
            case AttackType.PlayerWeapon:
            case AttackType.PlayerSpell:
                targetLayer = LayerMask.NameToLayer(gameObjectData.EnemyLayer);
                break;

            case AttackType.EnemyUnarmed:
            case AttackType.EnemyWeapon:
            case AttackType.EnemySpell:
                targetLayer = LayerMask.NameToLayer(gameObjectData.PlayerLayer);
                break;
        }
    }

    public void OpenDamageCollider()
    {
        damageCollider.enabled = true;

        if (trailRenderer != null)
            trailRenderer.enabled = true;
    }

    public void CloseDamageCollider()
    {
        damageCollider.enabled = false;

        if (trailRenderer != null)
            trailRenderer.enabled = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == targetLayer)
        {
            switch (attackType)
            {
                case AttackType.PlayerWeapon:
                case AttackType.PlayerSpell:
                    collision.GetComponent<EnemyStatus>().TakeDamage(damage, GetComponentInParent<CharacterStatus>());
                    PlayerAudio playerAudio = gameObject.GetComponentInParent<PlayerAudio>();
                    playerAudio.PlaySFX(playerAudio.playerClips[(int)PlayerAudio.PlayerSound.Attack2]);

                    break;

                case AttackType.EnemyUnarmed:
                case AttackType.EnemyWeapon:
                case AttackType.EnemySpell:
                    collision.GetComponent<PlayerStatus>().TakeDamage(damage, true);
                    break;
            }
        }
    }
}
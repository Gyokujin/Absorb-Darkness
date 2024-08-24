using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class DamageCollider : MonoBehaviour
{
    public enum AttackType
    {
        PlayerWeapon, PlayerSpell, EnemyUnarmed, EnemyWeapon, EnemySpell
    }

    [SerializeField]
    protected AttackType attackType;

    [Header("Data")]
    private GameObjectData gameObjectData;

    [Header("Attack")]
    [SerializeField]
    protected int damage = 10;
    [SerializeField]
    private bool hasCrash;
    protected LayerMask targetLayer;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
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
                    collision.GetComponent<PlayerStatus>().TakeDamage(damage, hasCrash);
                    break;
            }
        }
    }
}
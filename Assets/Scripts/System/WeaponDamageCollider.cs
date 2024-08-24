using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageCollider : DamageCollider
{
    [SerializeField]
    private TrailRenderer trailRenderer;
    private new Collider collider;

    void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        collider = GetComponent<Collider>();
    }

    public void OpenDamageCollider()
    {
        collider.enabled = true;

        if (attackType == AttackType.PlayerWeapon)
            trailRenderer.enabled = true;
    }

    public void CloseDamageCollider()
    {
        collider.enabled = false;

        if (attackType == AttackType.PlayerWeapon)
            trailRenderer.enabled = false;
    }
}
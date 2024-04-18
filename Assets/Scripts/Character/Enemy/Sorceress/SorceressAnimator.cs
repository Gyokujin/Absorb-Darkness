using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorceressAnimator : MonoBehaviour
{
    private Sorceress sorceress;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        sorceress = GetComponentInParent<Sorceress>();
    }

    public void SpawnMeteorAnimation()
    {
        sorceress.SpawnMeteor();
    }

    public void FallMeteorAnimation()
    {
        sorceress.FallMeteor();
    }

    public void SpawnLightningAnimation()
    {
        sorceress.SpawnLightning();
    }

    public void ShootLightningAnimation()
    {
        sorceress.ShootLightning();
    }
}
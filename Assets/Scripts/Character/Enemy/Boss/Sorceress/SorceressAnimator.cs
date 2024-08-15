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

    public void SpawnLightningAnimation()
    {
        sorceress.SpawnLightning();
    }

    public void ShootLightningAnimation()
    {
        sorceress.ShootLightning();
    }

    public void PoisonMistAnimation()
    {
        sorceress.PoisonMist();
    }

    public void SummonBatAnimation()
    {
        StartCoroutine(sorceress.SummonBat());
    }

    public void SpawnMeteorAnimation()
    {
        sorceress.SpawnMeteors();
    }

    public void FallMeteorAnimation()
    {
        StartCoroutine(sorceress.FallMeteors());
    }
}
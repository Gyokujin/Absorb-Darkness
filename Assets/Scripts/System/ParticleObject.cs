using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        PoolManager.instance.Return(transform.parent.gameObject);
    }
}
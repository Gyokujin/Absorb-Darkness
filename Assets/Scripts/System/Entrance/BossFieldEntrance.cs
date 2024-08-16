using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BossFieldEntrance : Entrance
{
    public FogWallEntrance targetGate;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer && other.GetComponent<PlayerManager>() != null)
        {
            targetGate.CloseGate();
            GameManager.instance.ReadFieldInfo(fieldInfo, this);
        }
    }
}
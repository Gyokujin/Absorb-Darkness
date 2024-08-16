using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BossFieldEntrance : Entrance
{
    [SerializeField]
    private FogWallEntrance targetGate;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer && other.GetComponent<PlayerManager>() != null)
        {
            targetGate.Close();
            GameManager.instance.ReadFieldInfo(fieldInfo);
        }
    }
}
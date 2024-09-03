using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFieldEntrance : Entrance
{
    public FogWallEntrance targetGate;
    private bool isAct;

    void OnTriggerEnter(Collider other)
    {
        if (!isAct && other.gameObject.layer == playerLayer && other.GetComponent<PlayerManager>() != null)
        {
            isAct = true;
            targetGate.CloseGate();
            GameManager.instance.ReadFieldInfo(fieldInfo, this);
        }
    }
}
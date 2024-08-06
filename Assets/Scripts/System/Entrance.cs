using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntranceType
{
    NextStage, BossRoom
}

public class Entrance : MonoBehaviour
{
    [SerializeField]
    private EntranceType entranceType;
    [SerializeField]
    private FogWallEntrance targetGate;
    private int playerLayer;
    [SerializeField]
    private BossInfo bossInfo;

    void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    void BossEntrance()
    {
        GameManager.instance.EntranceBossRoom();

        if (bossInfo != null)
        {
            UIManager.instance.bossStageUI.OpenBossStageUI(bossInfo.bossName);
        }

        targetGate.Close();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer && other.GetComponent<PlayerManager>() != null)
        {
            switch (entranceType)
            {
                case EntranceType.NextStage:
                    break;

                case EntranceType.BossRoom:
                    BossEntrance();
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
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
    private GateEntrance targetGate;
    private int playerLayer;

    void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
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
                    GameManager.instance.EntranceBossRoom();
                    targetGate.Close();
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntranceType
{
    NextStage, BossRoom
}

public class Entrance : MonoBehaviour
{
    private int playerLayer;
    [SerializeField]
    private EntranceType entranceType;

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
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
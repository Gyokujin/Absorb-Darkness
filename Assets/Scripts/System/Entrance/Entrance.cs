using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class Entrance : MonoBehaviour
{
    [Header("Data")]
    private GameObjectData gameObjectData;

    [SerializeField]
    protected FieldInfo fieldInfo;
    protected int playerLayer;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerLayer = LayerMask.NameToLayer(gameObjectData.PlayerLayer);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer && other.GetComponent<PlayerManager>() != null)
            GameManager.instance.ReadFieldInfo(fieldInfo, this);
    }
}
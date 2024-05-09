using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType
{
    Item, Message, Gate
}

public class Interactable : MonoBehaviour
{
    [Header("Interact")]
    public InteractType interactType;
    public string interactableText;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        switch (interactType)
        {
            case InteractType.Item:
                interactableText = "�������� ȹ���Ѵ� E";
                break;

            case InteractType.Message:
                interactableText = "�޽����� Ȯ���Ѵ� E";
                break;

            case InteractType.Gate:
                interactableText = "�Ȱ� ������ ���� E";
                break;
        }
    }

    public virtual void Interact(PlayerManager playerManager, PlayerAction playerInteract)
    {
        playerManager.playerMove.rigidbody.velocity = Vector3.zero;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractType
    {
        Item, Message, Gate
    }

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

    public virtual void Interact(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        playerManager.playerMove.rigidbody.velocity = Vector3.zero;
    }
}
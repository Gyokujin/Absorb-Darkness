using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType
{
    Item, Message
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
        }
    }

    public virtual void Interact(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        playerManager.playerMove.rigidbody.velocity = Vector3.zero;
    }
}
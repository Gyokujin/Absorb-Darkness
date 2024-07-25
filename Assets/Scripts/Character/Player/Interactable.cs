using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemDatas;

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
        InteractData interactData = new InteractData();
        
        switch (interactType)
        {
            case InteractType.Item:
                interactableText = interactData.itemInteractText;
                break;

            case InteractType.Message:
                interactableText = interactData.messageInteractText;
                break;

            case InteractType.Gate:
                interactableText = interactData.gateInteractText;
                break;
        }
    }

    public virtual void Interact(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        playerManager.playerMove.rigidbody.velocity = Vector3.zero;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class Interactable : MonoBehaviour
{
    public enum InteractType
    {
        Item, Message, Gate
    }

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

    public virtual void Interact(PlayerManager player, PlayerBehavior playerBehavior)
    {
        player.playerMove.rigidbody.velocity = Vector3.zero;
    }
}
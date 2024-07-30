using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class Interactable : MonoBehaviour
{
    public enum InteractType
    {
        Item, Message, LockDoor, FogWall
    }

    public InteractType interactType;
    [HideInInspector]
    public string interactableText;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        InteractData interactData = new();
        
        switch (interactType)
        {
            case InteractType.Item:
                interactableText = interactData.ItemInteractText;
                break;

            case InteractType.Message:
                interactableText = interactData.MessageInteractText;
                break;

            case InteractType.LockDoor:
                interactableText = interactData.LockDoorInteractText;
                break;

            case InteractType.FogWall:
                interactableText = interactData.FogWallInteractText;
                break;
        }
    }

    public virtual void Interact(PlayerManager player, PlayerBehavior playerBehavior)
    {
        player.playerMove.rigidbody.velocity = Vector3.zero;
    }
}
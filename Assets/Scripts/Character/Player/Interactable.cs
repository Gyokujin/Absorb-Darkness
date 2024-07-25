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
                interactableText = "아이템을 획득한다 E";
                break;

            case InteractType.Message:
                interactableText = "메시지를 확인한다 E";
                break;

            case InteractType.Gate:
                interactableText = "안개 속으로 들어간다 E";
                break;
        }
    }

    public virtual void Interact(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        playerManager.playerMove.rigidbody.velocity = Vector3.zero;
    }
}
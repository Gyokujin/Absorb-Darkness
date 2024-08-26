using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractType
    {
        DropItem, RuneLetter, LockDoor, FogWall
    }

    public InteractType interactType;

    public string interactMesssage;

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        switch (interactType)
        {
            case InteractType.DropItem:
                interactMesssage = UIManager.instance.messageUIData.DropItemInteractText;
                break;

            case InteractType.RuneLetter:
                interactMesssage = UIManager.instance.messageUIData.RuneLetterInteractText;
                break;

            case InteractType.LockDoor:
                interactMesssage = UIManager.instance.messageUIData.LockDoorInteractText;
                break;

            case InteractType.FogWall:
                interactMesssage = UIManager.instance.messageUIData.FogWallInteractText;
                break;
        }
    }

    public virtual void Interact(PlayerManager player)
    {
        player.playerMove.rigidbody.velocity = Vector3.zero;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageShow : Interactable
{
    [Header("Message")]
    [SerializeField]
    private GameMessage gameMessage;

    public override void Interact(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        base.Interact(playerManager, playerInteract);
        ShowMessage(playerManager, playerInteract);
    }

    void ShowMessage(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        UIManager.instance.OpenMessagePopUpUI(gameMessage.message);
    }
}
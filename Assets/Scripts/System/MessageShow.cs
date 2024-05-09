using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageShow : Interactable
{
    [Header("Message")]
    [SerializeField]
    private GameMessage gameMessage;

    public override void Interact(PlayerManager playerManager, PlayerAction playerInteract)
    {
        base.Interact(playerManager, playerInteract);
        ShowMessage(playerManager, playerInteract);
    }

    void ShowMessage(PlayerManager playerManager, PlayerAction playerInteract)
    {
        UIManager.instance.OpenMessagePopUpUI(gameMessage.message);
    }
}
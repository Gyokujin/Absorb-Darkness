using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageShow : Interactable
{
    [Header("Message")]
    [SerializeField]
    private GameMessage gameMessage;

    public override void Interact(PlayerManager playerManager, PlayerBehavior playerBehavior)
    {
        base.Interact(playerManager, playerBehavior);
        ShowMessage();
    }

    void ShowMessage()
    {
        UIManager.instance.OpenMessagePopUpUI(true, gameMessage.message);
    }
}
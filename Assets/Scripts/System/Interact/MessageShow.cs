using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageShow : Interactable
{
    [Header("Message")]
    [SerializeField]
    private GameMessage gameMessage;

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        ShowMessage();
    }

    void ShowMessage()
    {
        UIManager.instance.OpenMessageUI(MessageUI.MessageType.GameMessage);
        UIManager.instance.messageUI.UpdateGameMessage(gameMessage.message);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Interact1]);
    }
}
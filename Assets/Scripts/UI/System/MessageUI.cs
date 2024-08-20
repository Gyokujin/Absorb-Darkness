using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    public enum MessageType
    {
        InteractMessage, GameMessage, ItemPopup, GameSystem
    }

    [Header("Interact Message")]
    [SerializeField]
    private GameObject interactMessagePopup;
    [SerializeField]
    private Text interactMessageText;

    [Header("Game Message")]
    [SerializeField]
    private GameObject gameMessageUI;
    [SerializeField]
    private Text gameMessageText;

    [Header("Item Popup")]
    [SerializeField]
    private GameObject itemPopUpUI;
    [SerializeField]
    private Text itemText;
    [SerializeField]
    private RawImage itemImage;

    [Header("Game System")]
    [SerializeField]
    private GameObject gameSystemUI;
    [SerializeField]
    private Text gameSystemText;

    public void OpenInteractMessage()
    {
        interactMessagePopup.SetActive(true);
    }

    public void UpdateInteractMessage(string message)
    {
        interactMessageText.text = message;
    }

    public void CloseInteractMessage()
    {
        if (interactMessagePopup.activeSelf)
            interactMessagePopup.SetActive(false);
    }

    public void OpenGameMessage()
    {
        gameMessageUI.SetActive(true);
    }

    public void UpdateGameMessage(string message)
    {
        gameMessageText.text = message;
    }

    public void OpenGameSystem()
    {
        gameSystemUI.SetActive(true);
    }

    public void UpdateGameSystem(string message)
    {
        gameSystemText.text = message;
    }

    public void OpenItemPopup()
    {
        itemPopUpUI.SetActive(true);
    }

    public void UpdateItemPopup(Item item)
    {
        itemText.text = item.itemName;
        itemImage.texture = item.itemIcon.texture;
    }

    public void CloseAllMessage()
    {
        interactMessagePopup.SetActive(false);
        gameMessageUI.SetActive(false);
        itemPopUpUI.SetActive(false);
        gameSystemUI.SetActive(false);
    }
}
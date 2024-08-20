using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIData;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("Data")]
    public MessageUIData messageUIData;

    [Header("UI")]
    public HudUI hudUI;
    public SelectUI selectUI;
    public InventoryUI inventoryUI;
    public EquipmentUI equipmentUI;
    public GameSystemUI gameSystemUI;
    public MessageUI messageUI;
    public QuickSlotUI quickSlotUI;
    public StageUI stageUI;

    [HideInInspector]
    public PlayerInventory playerInventory;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    void Init()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        quickSlotUI.UpdateUsingItemUI(playerInventory.curUsingItem, playerInventory.curUsingItem.itemCount);
    }

    public void OpenGameUI()
    {
        hudUI.gameObject.SetActive(false);
        selectUI.gameObject.SetActive(true);
        GameManager.instance.LockCamera(false);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.GameUI]);
    }

    public void CloseGameUI()
    {
        CloseAllUI();
        hudUI.gameObject.SetActive(true);
        selectUI.gameObject.SetActive(false);
        GameManager.instance.LockCamera(true);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Interact2]);
    }

    public void CloseAllUI()
    {
        inventoryUI.gameObject.SetActive(false);
        equipmentUI.gameObject.SetActive(false);
        // gameSystemUI.gameObject.SetActive(false);
    }

    public void OpenMessageUI(MessageUI.MessageType messageType)
    {
        switch (messageType)
        {
            case MessageUI.MessageType.InteractMessage:
                messageUI.OpenInteractMessage();
                break;

            case MessageUI.MessageType.GameMessage:
                messageUI.OpenGameMessage();
                break;

            case MessageUI.MessageType.ItemPopup:
                messageUI.OpenItemPopup();
                break;

            case MessageUI.MessageType.GameSystem:
                messageUI.OpenGameSystem();
                break;
        }
    }

    public void CloseMessageUI()
    {
        messageUI.CloseAllMessage();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Interact2]);
    }

    public void OpenStageUI(FieldInfo fieldInfo)
    {
        stageUI.OpenStageInfo(fieldInfo);
    }
}
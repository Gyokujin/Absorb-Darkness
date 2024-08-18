using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField]
    private GameObject hudUI;
    public SelectUI selectUI;
    public InventoryUI inventoryUI;
    public EquipmentUI equipmentUI;
    public GameSystemUI gameSystemUI;
    public InteractableUI interactableUI;

    [HideInInspector]
    public PlayerInventory playerInventory;

    [SerializeField]
    private GameObject interactPopUpUI;
    [SerializeField]
    private Text interactMessage;

    [Header("PopUp Item")]
    public GameObject itemPopUpUI;
    [SerializeField]
    private Text itemPopUpName;
    [SerializeField]
    private RawImage itemPopUpIcon;

    [Header("Game Message")]
    [SerializeField]
    private GameObject messageTopUI;
    [SerializeField]
    private Text messageTopText;
    [SerializeField]
    private GameObject messageBottomUI;
    [SerializeField]
    private Text messageBottomText;

    [Header("Component")]
    public QuickSlotUI quickSlotUI;
    public StageUI stageUI;

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
        hudUI.SetActive(false);
        selectUI.gameObject.SetActive(true);
        selectUI.OpenSelectUI();
        GameManager.instance.LockCamera(false);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.GameUI]);
    }

    public void CloseGameUI()
    {
        hudUI.SetActive(true);
        selectUI.CloseSelectUI();
        selectUI.gameObject.SetActive(false);
        inventoryUI.gameObject.SetActive(false);
        equipmentUI.gameObject.SetActive(false);
        // gameSystemUI.gameObject.SetActive(false);

        GameManager.instance.LockCamera(true);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Interact2]);
    }

    public void OpenStageUI(FieldInfo fieldInfo)
    {
        stageUI.OpenStageInfo(fieldInfo);
    }

    public void OpenInventoryUI()
    {
        //inventoryui
        //AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    //public void InventoryUIUpdate()
    //{
    //    for (int i = 0; i < equipmentSlots.Length; i++)
    //    {
    //        if (i < playerInventory.equipmentWeapons.Count)
    //        {
    //            if (equipmentSlots.Length < playerInventory.equipmentWeapons.Count)
    //            {
    //                Instantiate(inventorySlotPrefab, inventorySlotsParent);
    //                equipmentSlots = inventorySlotsParent.GetComponentsInChildren<InventorySlot>();
    //            }

    //            equipmentSlots[i].AddItem(playerInventory.equipmentWeapons[i]);
    //        }
    //        else
    //        {
    //            equipmentSlots[i].ClearInventorySlot();
    //        }
    //    }
    //}

    public void OpenEquipmentUI()
    {
        CloseAllInventoryUI();
        // equipmentWindow.SetActive(true);
        // equipmentWindowUI.OpenEquipmentsUI();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void CloseAllInventoryUI()
    {
        //ResetAllSelectedSlots();
        //InventoryUI.instance.CloseInventory();
        //equipmentWindow.SetActive(false);
    }



    public void OpenInteractUI(string message)
    {
        interactMessage.text = message;
        interactPopUpUI.SetActive(true);
    }

    public void CloseInteractUI()
    {
        interactPopUpUI.SetActive(false);
    }

    public void OpenItemPopUpUI(string itemName, Texture itemIcon)
    {
        itemPopUpName.text = itemName;
        itemPopUpIcon.texture = itemIcon;
        itemPopUpUI.SetActive(true);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.PickUp]);
    }

    public void CloseItemPopUpUI()
    {
        itemPopUpUI.SetActive(false);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Interact2]);
    }

    public void OpenMessagePopUpUI(bool onTop, string message)
    {
        if (onTop)
        {
            messageTopUI.SetActive(true);
            messageTopText.text = message;
        }
        else
        {
            messageBottomUI.SetActive(true);
            messageBottomText.text = message;
        }

        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Interact1]);
    }

    public void CloseMessagePopUpUI()
    {
        messageTopUI.SetActive(false);
        messageBottomUI.SetActive(false);
    }
}
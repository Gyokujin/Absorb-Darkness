using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("Game System")]
    [SerializeField]
    private GameObject hudWindow;
    [SerializeField]
    private GameObject selectWindow;

    [Header("Inventory")]
    [SerializeField]
    private GameObject inventorySlotPrefab;
    [SerializeField]
    private Transform inventorySlotsParent;
    public PlayerInventory playerInventory;

    [Header("Equipment")]
    public GameObject equipmentWindow;
    [SerializeField]
    private EquipmentWindowUI equipmentWindowUI;
    private InventorySlot[] equipmentSlots;

    [Header("Equipment Window Slot Selected")]
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;

    [Header("Interact")]
    public InteractableUI interactableUI;
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
    public QuickSlotsUI quickSlotsUI;
    public BossStageUI bossStageUI;

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
    }

    void Start()
    {
        equipmentSlots = inventorySlotsParent.GetComponentsInChildren<InventorySlot>();
        InventoryUIUpdate();
    }

    public void OpenGameSystemUI()
    {
        selectWindow.SetActive(true);
        hudWindow.SetActive(false);
        GameManager.instance.LockCamera(false);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.GameSystem]);
    }

    public void CloseGameSystemUI()
    {
        selectWindow.SetActive(false);
        hudWindow.SetActive(true);
        CloseAllInventoryUI();
        ResetAllSelectedSlots();
        GameManager.instance.LockCamera(true);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Interact2]);
    }

    public void OpenInventoryUI()
    {
        InventoryManager.instance.OpenWeaponInventory();
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Click]);
    }

    public void InventoryUIUpdate()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (i < playerInventory.weaponsInventory.Count)
            {
                if (equipmentSlots.Length < playerInventory.weaponsInventory.Count)
                {
                    Instantiate(inventorySlotPrefab, inventorySlotsParent);
                    equipmentSlots = inventorySlotsParent.GetComponentsInChildren<InventorySlot>();
                }

                equipmentSlots[i].AddItem(playerInventory.weaponsInventory[i]);
            }
            else
            {
                equipmentSlots[i].ClearInventorySlot();
            }
        }
    }

    public void OpenEquipmentUI()
    {
        CloseAllInventoryUI();
        equipmentWindow.SetActive(true);
        equipmentWindowUI.OpenEquipmentsUI();
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Click]);
    }

    public void CloseAllInventoryUI()
    {
        ResetAllSelectedSlots();
        InventoryManager.instance.CloseInventory();
        equipmentWindow.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
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
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.PickUp]);
    }

    public void CloseItemPopUpUI()
    {
        itemPopUpUI.SetActive(false);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Interact2]);
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

        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Interact1]);
    }

    public void CloseMessagePopUpUI()
    {
        messageTopUI.SetActive(false);
        messageBottomUI.SetActive(false);
    }
}
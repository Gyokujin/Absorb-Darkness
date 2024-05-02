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
    private GameObject weaponInventoryWindow;
    public PlayerInventory playerInventory;

    [Header("Equipment")]
    [SerializeField]
    private GameObject equipmentScreenWindow;
    [SerializeField]
    private GameObject weaponInventorySlotPrefab;
    [SerializeField]
    private Transform weaponInventorySlotsParent;
    private WeaponInventorySlot[] weaponInventorySlots;

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
    public GameObject messagePopUp;
    [SerializeField]
    private Text messageText;

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
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        InventoryUIUpdate();
    }

    public void InventoryUIUpdate()
    {
        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if (i < playerInventory.weaponsInventory.Count)
            {
                if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }

                weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySlot();
            }
        }
    }

    public void OpenGameSystemUI()
    {
        selectWindow.SetActive(true);
        hudWindow.SetActive(false);
        GameManager.instance.LockCamera(false);
    }

    public void CloseGameSystemUI()
    {
        selectWindow.SetActive(false);
        hudWindow.SetActive(true);
        CloseAllInventoryUI();
        ResetAllSelectedSlots();
        GameManager.instance.LockCamera(true);
    }

    public void CloseAllInventoryUI()
    {
        ResetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        equipmentScreenWindow.SetActive(false);
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
    }

    public void CloseItemPopUpUI()
    {
        itemPopUpUI.SetActive(false);
    }

    public void OpenMessagePopUpUI(string message)
    {
        messagePopUp.SetActive(true);
        messageText.text = message;
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)SystemSound.Interact1]);
    }

    public void CloseMessagePopUpUI()
    {
        messagePopUp.SetActive(false);
    }
}
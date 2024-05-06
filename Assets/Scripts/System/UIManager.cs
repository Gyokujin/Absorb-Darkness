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
    public GameObject inventoryWindow;
    [SerializeField]
    private GameObject inventorySlotPrefab;
    [SerializeField]
    private Transform inventorySlotsParent;
    public PlayerInventory playerInventory;

    [Header("Equipment")]
    public GameObject equipmentWindow;
    [SerializeField]
    private EquipmentWindowUI equipmentWindowUI;

    private WeaponInventorySlot[] equipmentSlots;

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

    [Header("Boss Info")]
    [SerializeField]
    private GameObject bossInfoUI;
    [SerializeField]
    private Text bossNameText;
    [SerializeField]
    private Slider bossHPSlider;

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
        equipmentSlots = inventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        InventoryUIUpdate();
    }

    public void OpenGameSystemUI()
    {
        selectWindow.SetActive(true);
        hudWindow.SetActive(false);
        GameManager.instance.LockCamera(false);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)SystemSound.GameSystem]);
    }

    public void CloseGameSystemUI()
    {
        selectWindow.SetActive(false);
        hudWindow.SetActive(true);
        CloseAllInventoryUI();
        ResetAllSelectedSlots();
        GameManager.instance.LockCamera(true);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)SystemSound.Interact2]);
    }

    public void OpenInventoryUI()
    {
        CloseAllInventoryUI();
        inventoryWindow.SetActive(true);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)SystemSound.Click]);
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
                    equipmentSlots = inventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
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
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)SystemSound.Click]);
    }

    public void CloseAllInventoryUI()
    {
        ResetAllSelectedSlots();
        inventoryWindow.SetActive(false);
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

    public void OpenBossInfo(string name)
    {
        bossNameText.text = name;
        bossHPSlider.value = 1;
        bossInfoUI.SetActive(true);
    }

    public void BossHPUIModify(float curHP, float maxHP)
    {
        bossHPSlider.value = curHP / maxHP;
    }

    public void CloseBossInfo()
    {
        bossInfoUI.SetActive(false);
    }
}
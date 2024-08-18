using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    private InitItemData initItemData;

    [Header("Inventory")]
    public List<WeaponItem> weaponItems;
    public List<UsingItem> usingItems;
    public List<InteractItem> interactItems;

    [Header("UI")]
    [SerializeField]
    private GameObject inventoryButtons;
    public GameObject slotObject;

    [Header("Component")]
    [SerializeField]
    private WeaponInventory weaponInventory;
    [SerializeField]
    private UsingItemInventory usingItemInventory;
    [SerializeField]
    private InteractItemInventory interactItemInventory;

    private PlayerInventory playerInventory;
    [SerializeField]
    private QuickSlotsUI quickSlotsUI;

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
        initItemData = new InitItemData();
        playerInventory = FindObjectOfType<PlayerInventory>();

        // 최초 아이템 초기화
        playerInventory.curUsingItem = usingItems[0];
        playerInventory.curUsingItem.itemCount = initItemData.InitEstusCount;
        quickSlotsUI.UpdateUsingItemUI(playerInventory.curUsingItem, playerInventory.curUsingItem.itemCount);
    }

    public void ItemLoot(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.WeaponItem:
                WeaponItem weaponItem = item as WeaponItem;
                GetWeaponItem(weaponItem);
                break;

            case Item.ItemType.UsingItem:
                UsingItem usingItem = item as UsingItem;
                GetUsingItem(usingItem);
                break;

            case Item.ItemType.InteractItem:
                InteractItem interactItem = item as InteractItem;
                GetInteractItem(interactItem);
                break;
        }

        UIManager.instance.OpenItemPopUpUI(item.itemName, item.itemIcon.texture);
    }

    void GetWeaponItem(WeaponItem weaponItem)
    {
        weaponItems.Add(weaponItem);
    }

    void GetUsingItem(UsingItem usingItem)
    {
        usingItems.Add(usingItem);
    }

    void GetInteractItem(InteractItem interactItem)
    {
        interactItems.Add(interactItem);
    }

    public void OpenWeaponInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        weaponInventory.gameObject.SetActive(true);
        weaponInventory.WeaponItemUpdate();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void OpenUsingItemInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        usingItemInventory.gameObject.SetActive(true);
        usingItemInventory.UsingItemUpdate();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void OpenInteractItemInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        interactItemInventory.gameObject.SetActive(true);
        interactItemInventory.InteractItemUpdate();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void CloseInventory()
    {
        inventoryButtons.SetActive(false);
        weaponInventory.gameObject.SetActive(false);
        usingItemInventory.gameObject.SetActive(false);
        interactItemInventory.gameObject.SetActive(false);
    }
}
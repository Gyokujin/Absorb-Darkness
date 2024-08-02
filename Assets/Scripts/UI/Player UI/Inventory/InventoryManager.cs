using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    private InitItemData initItemData;

    [Header("Inventory")]
    private WeaponInventory weaponInventory;
    private UsingItemInventory usingItemInventory;
    private InteractItemInventory interactItemInventory;

    [Header("UI")]
    [SerializeField]
    private GameObject inventoryButtons;
    public GameObject slotObject;

    [Header("Component")]
    private PlayerInventory playerInventory;
    private PlayerWeaponSlotManager playerWeaponSlotManager;
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
        weaponInventory = GetComponentInChildren<WeaponInventory>();
        usingItemInventory = GetComponentInChildren<UsingItemInventory>();
        interactItemInventory = GetComponentInChildren<InteractItemInventory>();

        playerInventory = FindObjectOfType<PlayerInventory>();
        playerWeaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();

        // 최초 아이템 초기화
        playerInventory.curUsingItem = usingItemInventory.usingItems[0];
        playerInventory.curUsingItem.itemCount = initItemData.InitEstusCount;
        quickSlotsUI.UpdateUsingItemUI(playerInventory.curUsingItem, playerInventory.curUsingItem.itemCount);

        CloseInventory();
        inventoryButtons.SetActive(false);
    }

    public void GetWeaponItem(WeaponItem weaponItem)
    {
        weaponInventory.weaponItems.Add(weaponItem);
    }

    public void GetUsingItem(UsingItem usingItem)
    {
        usingItemInventory.usingItems.Add(usingItem);
    }

    public void GetInteractItem(InteractItem interactItem)
    {
        interactItemInventory.interactItems.Add(interactItem);
    }

    public void OpenWeaponInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        weaponInventory.gameObject.SetActive(true);
        weaponInventory.WeaponItemUpdate();
    }

    public void OpenUsingItemInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        usingItemInventory.gameObject.SetActive(true);
        usingItemInventory.UsingItemUpdate();
    }

    public void OpenInteractItemInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        interactItemInventory.gameObject.SetActive(true);
    }

    void CloseInventory()
    {
        weaponInventory.gameObject.SetActive(false);
        usingItemInventory.gameObject.SetActive(false);
        interactItemInventory.gameObject.SetActive(false);
    }
}
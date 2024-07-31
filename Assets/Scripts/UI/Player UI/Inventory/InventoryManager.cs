using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    private InitItemData initItemData;

    [Header("Inventory")]
    public WeaponInventory weaponInventory;
    public UsingItemInventory usingItemInventory;
    public InteractItemInventory interactItemInventory;
    [SerializeField]
    private GameObject[] inventoryButtons;

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

        playerInventory = FindObjectOfType<PlayerInventory>();
        playerWeaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();

        // 최초 아이템 초기화
        playerInventory.curUsingItem = usingItemInventory.usingItems[0];
        playerInventory.curUsingItem.itemCount = initItemData.InitEstusCount;
        quickSlotsUI.UpdateUsingItemUI(playerInventory.curUsingItem, playerInventory.curUsingItem.itemCount);

        CloseInventory();
        ShowButtonControl(false);
    }

    //void OnEnable()
    //{
    //    OpenWeaponInventory();
    //}

    public void OpenWeaponInventory()
    {
        CloseInventory();
        ShowButtonControl(true);
        weaponInventory.gameObject.SetActive(true);
    }

    public void OpenUsingItemInventory()
    {
        CloseInventory();
        ShowButtonControl(true);
        usingItemInventory.gameObject.SetActive(true);
    }

    public void OpenInteractItemInventory()
    {
        CloseInventory();
        ShowButtonControl(true);
        interactItemInventory.gameObject.SetActive(true);
    }

    void CloseInventory()
    {
        weaponInventory.gameObject.SetActive(false);
        usingItemInventory.gameObject.SetActive(false);
        interactItemInventory.gameObject.SetActive(false);
    }

    void ShowButtonControl(bool onShow)
    {
        foreach (GameObject button in inventoryButtons)
        {
            button.SetActive(onShow);
        }
    }
}
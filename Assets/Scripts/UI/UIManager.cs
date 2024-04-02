using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("Game System")]
    public GameObject hudWindow;
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
    public EquipmentWindowUI equipmentWindowUI;

    [Header("Equipment Window Slot Selected")]
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;

    // [Header("Game Setting")]

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
        UpdateUI();
    }

    public void UpdateUI()
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Window")]
    public GameObject hudWindow;
    [SerializeField]
    private GameObject selectWindow;
    [SerializeField]
    private GameObject weaponInventoryWindow;

    [Header("Weapon Inventory")]
    [SerializeField]
    private GameObject weaponInventorySlotPrefab;
    [SerializeField]
    private Transform weaponInventorySlotsParent;

    [Header("Component")]
    public PlayerInventory playerInventory;
    private EquipmentWindowUI equipmentWindowUI;
    private WeaponInventorySlot[] weaponInventorySlots;

    void Awake()
    {
        equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
        // equipmentWindowUI = GetComponentInChildren<EquipmentWindowUI>();
    }

    void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
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

    public void ControlSelectWindow(bool onOpen)
    {
        selectWindow.SetActive(onOpen);
    }

    public void CloseAllInventoryWindow()
    {
        weaponInventoryWindow.SetActive(false);
    }
}
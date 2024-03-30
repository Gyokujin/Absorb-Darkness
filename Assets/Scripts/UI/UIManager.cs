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

    [Header("Weapon Inventory")]
    [SerializeField]
    private GameObject weaponInventorySlotPrefab;
    [SerializeField]
    private Transform weaponInventorySlotsParent;
    private WeaponInventorySlot[] weaponInventorySlots;
    [SerializeField]
    private EquipmentWindowUI equipmentWindowUI;

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

        equipmentWindowUI = GetComponentInChildren<EquipmentWindowUI>();
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        equipmentWindowUI.gameObject.SetActive(false);
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
        weaponInventoryWindow.gameObject.SetActive(false);
        equipmentWindowUI.gameObject.SetActive(false);
        GameManager.instance.LockCamera(true);
    }
}
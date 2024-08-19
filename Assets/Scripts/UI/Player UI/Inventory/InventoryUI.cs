using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField]
    private GameObject weaponInventory;
    public List<WeaponSlot> weaponSlots = new();
    [SerializeField]
    private Transform weaponSlotTransform;

    [Header("Using Item")]
    [SerializeField]
    private GameObject usingItemInventory;
    public List<ItemSlot> usingItemSlots = new();
    [SerializeField]
    private Transform usingItemSlotTransform;

    [Header("Interact Item")]
    [SerializeField]
    private GameObject interactItemInventory;
    public List<ItemSlot> interactItemSlots = new();
    [SerializeField]
    private Transform interactItemSlotTransform;

    [Header("Slot")]
    [SerializeField]
    private WeaponSlot weaponSlot;
    [SerializeField]
    private UsingItemSlot usingItemSlot;
    [SerializeField]
    private ItemSlot interactItemSlot;

    [Header("UI")]
    [SerializeField]
    private GameObject inventoryButtons;

    public void OpenWeaponInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        weaponInventory.SetActive(true);
        UpdateWeaponInventory();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void UpdateWeaponInventory()
    {
        for (int i = 0; i < UIManager.instance.playerInventory.weaponItems.Count; i++)
        {
            if (i >= weaponSlots.Count)
            {
                WeaponSlot createSlot = Instantiate(weaponSlot, weaponSlotTransform).GetComponent<WeaponSlot>();
                weaponSlots.Add(createSlot);
            }

            WeaponItem item = UIManager.instance.playerInventory.weaponItems[i];
            weaponSlots[i].ItemSlotUpdate(true, item.itemIcon, item.itemCount, item.itemName, item.itemInfo, i);
        }
    }

    public void OpenUsingItemInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        usingItemInventory.SetActive(true);
        UpdateUsingItemInventory();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void UpdateUsingItemInventory()
    {
        for (int i = 0; i < UIManager.instance.playerInventory.usingItems.Count; i++)
        {
            if (i >= usingItemSlots.Count)
            {
                UsingItemSlot createSlot = Instantiate(usingItemSlot, usingItemSlotTransform).GetComponent<UsingItemSlot>();
                usingItemSlots.Add(createSlot);
            }

            UsingItem item = UIManager.instance.playerInventory.usingItems[i];
            usingItemSlots[i].ItemSlotUpdate(false, item.itemIcon, item.itemCount, item.itemName, item.itemInfo, i);
        }
    }

    public void OpenInteractItemInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        interactItemInventory.SetActive(true);
        UpdateInteractItemInventory();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void UpdateInteractItemInventory()
    {
        for (int i = 0; i < UIManager.instance.playerInventory.interactItems.Count; i++)
        {
            if (i >= interactItemSlots.Count)
            {
                ItemSlot createSlot = Instantiate(interactItemSlot, interactItemSlotTransform).GetComponent<ItemSlot>();
                interactItemSlots.Add(createSlot);
            }

            InteractItem item = UIManager.instance.playerInventory.interactItems[i];
            interactItemSlots[i].ItemSlotUpdate(false, item.itemIcon, item.itemCount, item.itemName, item.itemInfo, i);
        }
    }

    public void CloseInventory()
    {
        inventoryButtons.SetActive(false);
        weaponInventory.SetActive(false);
        usingItemInventory.SetActive(false);
        interactItemInventory.SetActive(false);
    }
}
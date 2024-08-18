using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField]
    private GameObject weaponInventory;
    public List<ItemSlot> weaponSlots = new();
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

    [Header("UI")]
    [SerializeField]
    private GameObject inventoryButtons;
    [SerializeField]
    private GameObject slotObject;

    public void OpenWeaponInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        weaponInventory.gameObject.SetActive(true);
        UpdateWeaponInventory();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void UpdateWeaponInventory()
    {
        for (int i = 0; i < UIManager.instance.playerInventory.weaponItems.Count; i++)
        {
            if (i >= weaponSlots.Count)
            {
                ItemSlot createSlot = Instantiate(slotObject, weaponSlotTransform).GetComponent<ItemSlot>();
                weaponSlots.Add(createSlot);
            }

            Item item = UIManager.instance.playerInventory.weaponItems[i];
            weaponSlots[i].ItemSlotUpdate(true, item.itemIcon, item.itemCount, item.itemName, item.itemInfo);
        }
    }

    public void OpenUsingItemInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        usingItemInventory.gameObject.SetActive(true);
        UpdateUsingItemInventory();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void UpdateUsingItemInventory()
    {
        for (int i = 0; i < UIManager.instance.playerInventory.usingItems.Count; i++)
        {
            if (i >= usingItemSlots.Count)
            {
                ItemSlot createSlot = Instantiate(slotObject, usingItemSlotTransform).GetComponent<ItemSlot>();
                usingItemSlots.Add(createSlot);
            }

            Item item = UIManager.instance.playerInventory.usingItems[i];
            usingItemSlots[i].ItemSlotUpdate(false, item.itemIcon, item.itemCount, item.itemName, item.itemInfo);
        }
    }

    public void OpenInteractItemInventory()
    {
        CloseInventory();
        inventoryButtons.SetActive(true);
        interactItemInventory.gameObject.SetActive(true);
        UpdateInteractItemInventory();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void UpdateInteractItemInventory()
    {
        for (int i = 0; i < UIManager.instance.playerInventory.interactItems.Count; i++)
        {
            if (i >= interactItemSlots.Count)
            {
                ItemSlot createSlot = Instantiate(slotObject, interactItemSlotTransform).GetComponent<ItemSlot>();
                interactItemSlots.Add(createSlot);
            }

            Item item = UIManager.instance.playerInventory.interactItems[i];
            interactItemSlots[i].ItemSlotUpdate(false, item.itemIcon, item.itemCount, item.itemName, item.itemInfo);
        }
    }

    public void CloseInventory()
    {
        inventoryButtons.SetActive(false);
        weaponInventory.gameObject.SetActive(false);
        usingItemInventory.gameObject.SetActive(false);
        interactItemInventory.gameObject.SetActive(false);
    }
}
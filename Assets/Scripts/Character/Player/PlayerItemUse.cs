using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    public GameObject curUsingItem;
    private GameObject leftHandWeapon;
    private ItemSlotManager itemSlotManager;
    private PlayerInventory playerInventory;

    void Awake()
    {
        itemSlotManager = GetComponentInChildren<ItemSlotManager>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    public void UseItem(PlayerAnimator playerAnimator, UsingItem item)
    {
        if (itemSlotManager.leftHandSlot.currentWeaponModel != null)
        {
            leftHandWeapon = itemSlotManager.leftHandSlot.currentWeaponModel.gameObject;
            leftHandWeapon.SetActive(false);
        }

        switch (item.itemType)
        {
            case UsingItem.UsingItemType.EstusFlask:
                if (playerInventory.estusCount > 0)
                {
                    UseEstus(item);
                }
                else
                {
                    return;
                }
                break;
        }

        curUsingItem.transform.parent = itemSlotManager.leftHandSlot.parentOverride;
        curUsingItem.transform.position = itemSlotManager.leftHandSlot.parentOverride.transform.position;
        curUsingItem.transform.localRotation = Quaternion.identity;
        playerAnimator.PlayTargetAnimation(item.usingAnimation, true);
    }

    void UseEstus(UsingItem item)
    {
        curUsingItem = PoolManager.instance.GetItem((int)PoolManager.Item.EstusFlask);
        playerInventory.estusCount--;
        UIManager.instance.quickSlotsUI.UpdateUsingItemUI(item, playerInventory.estusCount);
    }

    public void EndItemUse()
    {
        PoolManager.instance.Return(curUsingItem);
        curUsingItem = null;

        if (leftHandWeapon != null)
        {
            leftHandWeapon.SetActive(true);
            leftHandWeapon = null;
        }
    }
}
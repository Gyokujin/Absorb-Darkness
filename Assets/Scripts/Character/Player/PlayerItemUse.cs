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
        switch (item.itemType)
        {
            case UsingItem.UsingItemType.EstusFlask:
                if (playerInventory.estusCount <= 0)
                    return;

                curUsingItem = PoolManager.instance.GetItem((int)PoolManager.Item.EstusFlask);
                playerInventory.estusCount--;
                break;
        }

        curUsingItem = item.GameObject();
        leftHandWeapon = itemSlotManager.leftHandSlot.currentWeaponModel.gameObject;
        leftHandWeapon.SetActive(false);

        curUsingItem.transform.parent = itemSlotManager.leftHandSlot.parentOverride;
        curUsingItem.transform.position = itemSlotManager.leftHandSlot.parentOverride.transform.position;
        curUsingItem.transform.localRotation = Quaternion.identity;
        UIManager.instance.quickSlotsUI.UpdateUsingItemUI(item);
        playerAnimator.PlayTargetAnimation(item.usingAnimation, true);
    }

    public void EndItemUse()
    {
        PoolManager.instance.Return(curUsingItem);
        curUsingItem = null;
        leftHandWeapon.SetActive(true);
        leftHandWeapon = null;
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    public GameObject curUsingItem;
    private GameObject leftHandWeapon;
    private ItemSlotManager itemSlotManager;

    void Awake()
    {
        itemSlotManager = GetComponentInChildren<ItemSlotManager>();
    }

    public void UseItem(PlayerAnimator playerAnimator, UsingItem item)
    {
        if (item.itemCount <= 0)
        {
            return;
        }

        curUsingItem = item.GameObject();
        leftHandWeapon = itemSlotManager.leftHandSlot.currentWeaponModel.gameObject;
        leftHandWeapon.SetActive(false);
        
        switch (item.itemType)
        {
            case UsingItem.UsingItemType.EstusFlask:
                curUsingItem = PoolManager.instance.GetItem((int)PoolManager.Item.EstusFlask);
                break;
        }

        curUsingItem.transform.parent = itemSlotManager.leftHandSlot.parentOverride;
        curUsingItem.transform.position = itemSlotManager.leftHandSlot.parentOverride.transform.position;
        curUsingItem.transform.localRotation = Quaternion.identity;
        item.itemCount--;
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
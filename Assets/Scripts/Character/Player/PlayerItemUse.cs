using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    private GameObject leftHandWeapon;
    private ItemSlotManager itemSlotManager;

    void Awake()
    {
        itemSlotManager = GetComponentInChildren<ItemSlotManager>();
    }

    public void UseItem(PlayerAnimator playerAnimator, UsingItem item)
    {
        leftHandWeapon = itemSlotManager.leftHandSlot.currentWeaponModel.gameObject;
        leftHandWeapon.SetActive(false);
        GameObject targetItem = null;

        switch (item.itemType)
        {
            case UsingItem.UsingItemType.EstusFlask:
                targetItem = PoolManager.instance.GetItem((int)PoolManager.Item.EstusFlask);
                break;
        }

        targetItem.transform.position = itemSlotManager.leftHandSlot.parentOverride.transform.position;
        playerAnimator.PlayTargetAnimation(item.usingAnimation, true);
    }

    public void ReturnWeapon()
    {
        leftHandWeapon.SetActive(true);
        leftHandWeapon = null;
    }
}
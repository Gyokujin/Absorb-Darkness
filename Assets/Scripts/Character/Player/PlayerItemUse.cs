using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    private PlayerManager player;

    public GameObject curUsingItem;
    private GameObject leftHandWeapon;

    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void UseItem(PlayerAnimator playerAnimator, UsingItem item)
    {
        if (player.playerItemSlotManager.leftHandSlot.currentWeaponModel != null)
        {
            leftHandWeapon = player.playerItemSlotManager.leftHandSlot.currentWeaponModel.gameObject;
            leftHandWeapon.SetActive(false);
        }

        switch (item.itemType)
        {
            case UsingItem.UsingItemType.EstusFlask:
                if (player.playerInventory.estusCount > 0)
                {
                    UseEstus(item);
                }
                else
                {
                    return;
                }
                break;
        }

        curUsingItem.transform.parent = player.playerItemSlotManager.leftHandSlot.parentOverride;
        curUsingItem.transform.position = player.playerItemSlotManager.leftHandSlot.parentOverride.transform.position;
        curUsingItem.transform.localRotation = Quaternion.identity;
        playerAnimator.PlayTargetAnimation(item.usingAnimation, true);
    }

    void UseEstus(UsingItem item)
    {
        curUsingItem = PoolManager.instance.GetItem((int)PoolManager.Item.EstusFlask);
        player.playerInventory.estusCount--;
        UIManager.instance.quickSlotsUI.UpdateUsingItemUI(item, player.playerInventory.estusCount);
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
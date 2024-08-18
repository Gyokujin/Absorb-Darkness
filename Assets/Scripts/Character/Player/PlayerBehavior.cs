using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private PlayerManager player;

    [Header("Interact")]
    private Interactable interactableObj;

    [Header("Item Use")]
    public GameObject curUsingItem;
    private GameObject leftHandWeapon;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
    }

    public void CheckInteractableObject()
    {
        if (Physics.SphereCast(transform.position, player.interactData.InteractCheckRadius, transform.forward, out RaycastHit hit, player.interactData.InteractCheckDis, PlayerCamera.instance.targetLayer))
        {
            if (hit.collider.CompareTag(player.interactData.InteractObjTag))
                interactableObj = hit.collider.GetComponent<Interactable>();
        }
        else
            interactableObj = null;


        if (interactableObj != null)
            UIManager.instance.OpenInteractUI(interactableObj.interactableText);
        else
            UIManager.instance.CloseInteractUI();
    }

    public void BehaviourAction()
    {
        if (player.playerBehavior.interactableObj != null)
            interactableObj.Interact(player);
        else
        {
            UIManager.instance.CloseItemPopUpUI();
            UIManager.instance.CloseMessagePopUpUI();
        }
    }

    public void ItemLoot(Item item)
    {
        player.playerInventory.AddToInventory(item);
        UIManager.instance.OpenItemPopUpUI(item.itemName, item.itemIcon.texture);
    }

    public void UseItem(UsingItem item)
    {
        if (player.playerInventory.leftHandSlot.currentWeaponModel != null)
        {
            leftHandWeapon = player.playerInventory.leftHandSlot.currentWeaponModel;
            leftHandWeapon.SetActive(false);
        }

        switch (item.usingItemType)
        {
            case UsingItem.UsingItemType.EstusFlask:
                if (player.playerInventory.curUsingItem.itemCount <= 0)
                    return;

                curUsingItem = PoolManager.instance.GetItem((int)PoolManager.Item.EstusFlask);
                player.playerInventory.curUsingItem.itemCount--;
                break;
        }

        curUsingItem.transform.parent = player.playerInventory.leftHandSlot.parentOverride;
        curUsingItem.transform.position = player.playerInventory.leftHandSlot.parentOverride.transform.position;
        curUsingItem.transform.localRotation = Quaternion.identity;
        player.playerAnimator.animator.SetBool(player.characterAnimatorData.IsItemUseParameter, true);
        player.playerAnimator.PlayTargetAnimation(item.usingAnimation, true);
        UIManager.instance.quickSlotUI.UpdateUsingItemUI(item, player.playerInventory.curUsingItem.itemCount);
    }

    public void EndItemUse()
    {
        player.playerAnimator.animator.SetBool(player.characterAnimatorData.IsItemUseParameter, false);
        PoolManager.instance.Return(curUsingItem);
        curUsingItem = null;

        if (leftHandWeapon != null)
        {
            leftHandWeapon.SetActive(true);
            leftHandWeapon = null;
        }
    }
}
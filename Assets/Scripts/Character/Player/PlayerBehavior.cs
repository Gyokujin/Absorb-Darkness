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
        if (Physics.SphereCast(transform.position, player.interactData.InteractCheckRadius, transform.forward, out RaycastHit hit, player.interactData.InteractCheckDis, PlayerCamera.instance.targetLayers))
        {
            if (hit.collider.CompareTag(player.interactData.InteractObjTag))
                interactableObj = hit.collider.GetComponent<Interactable>();
        }
        else
            interactableObj = null;

        if (interactableObj != null)
        {
            UIManager.instance.OpenMessageUI(MessageUI.MessageType.InteractMessage);
            UIManager.instance.messageUI.UpdateInteractMessage(interactableObj.interactMesssage);
        }
        else
            UIManager.instance.messageUI.CloseInteractMessage();
    }

    public void BehaviourAction()
    {
        if (player.playerBehavior.interactableObj != null)
        {
            interactableObj.Interact(player);
            player.playerAudio.StopFootstepSFX();
            player.playerAudio.StopSprintSFX();
        }
        else
            UIManager.instance.CloseMessageUI();
    }

    public void ItemLoot(Item item)
    {
        player.playerInventory.AddToInventory(item);
        UIManager.instance.OpenMessageUI(MessageUI.MessageType.ItemPopup);
        UIManager.instance.messageUI.UpdateItemPopup(item);
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
            case UsingItem.UsingItemType.EtherFlask:
                if (player.playerInventory.curUsingItem.itemCount <= 0)
                    return;

                curUsingItem = PoolManager.instance.GetItem((int)PoolManager.Item.EtherFlask);
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
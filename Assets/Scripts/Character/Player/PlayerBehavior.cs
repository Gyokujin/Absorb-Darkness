using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using SystemData;

public class PlayerBehavior : MonoBehaviour
{
    private PlayerManager player;
    private PlayerAnimatorData animatorData;
    private InteractData interactData;

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
        animatorData = new PlayerAnimatorData();
        interactData = new InteractData();
    }

    public void CheckInteractableObject(PlayerManager playerManager)
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, interactData.InteractCheckRadius, transform.forward, out hit, interactData.InteractCheckDis, PlayerCamera.instance.targetLayer))
        {
            if (hit.collider.tag == interactData.InteractObjTag && hit.collider.GetComponent<Interactable>() != null)
            {
                interactableObj = hit.collider.GetComponent<Interactable>();
                UIManager.instance.OpenInteractUI(interactableObj.interactableText);

                if (player.playerInput.interactInput)
                {
                    interactableObj.Interact(playerManager, this);

                    if (interactableObj.interactType == Interactable.InteractType.Item)
                    {
                        UIManager.instance.InventoryUIUpdate();
                    }
                }
            }
        }
        else
        {
            if (UIManager.instance.itemPopUpUI != null)
            {
                UIManager.instance.CloseInteractUI();

                if (interactableObj != null && player.playerInput.interactInput)
                {
                    UIManager.instance.CloseItemPopUpUI();
                    UIManager.instance.CloseMessagePopUpUI();

                    switch (interactableObj.interactType)
                    {
                        case Interactable.InteractType.Item:
                            break;

                        case Interactable.InteractType.Message:
                            break;
                    }

                    AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Interact2]);
                    interactableObj = null;
                }
            }
        }
    }

    public void UseItem(PlayerAnimator playerAnimator, UsingItem item)
    {
        if (player.playerItemSlotManager.leftHandSlot.currentWeaponModel != null)
        {
            leftHandWeapon = player.playerItemSlotManager.leftHandSlot.currentWeaponModel.gameObject;
            leftHandWeapon.SetActive(false);
        }

        switch (item.usingItemType)
        {
            case UsingItem.UsingItemType.EstusFlask:
                if (player.playerInventory.curUsingItem.itemCount <= 0)
                    return;

                //if (InventoryManager.instance.usingItemInventory.usingItems[0].itemCount <= 0)
                //    return;

                curUsingItem = PoolManager.instance.GetItem((int)PoolManager.Item.EstusFlask);
                player.playerInventory.curUsingItem.itemCount--;
                UIManager.instance.quickSlotsUI.UpdateUsingItemUI(item, player.playerInventory.curUsingItem.itemCount);
                break;
        }

        curUsingItem.transform.parent = player.playerItemSlotManager.leftHandSlot.parentOverride;
        curUsingItem.transform.position = player.playerItemSlotManager.leftHandSlot.parentOverride.transform.position;
        curUsingItem.transform.localRotation = Quaternion.identity;
        player.playerAnimator.animator.SetBool(animatorData.IsItemUseParameter, true);
        playerAnimator.PlayTargetAnimation(item.usingAnimation, true);
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

        player.playerAnimator.animator.SetBool(animatorData.IsItemUseParameter, false);
    }
}
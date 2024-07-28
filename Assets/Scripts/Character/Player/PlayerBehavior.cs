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
    private Interactable itemInteractableObj;

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

        if (Physics.SphereCast(transform.position, interactData.interactCheckRadius, transform.forward, out hit, interactData.interactCheckDis, PlayerCamera.instance.targetLayer))
        {
            if (hit.collider.tag == interactData.interactObjTag && hit.collider.GetComponent<Interactable>() != null)
            {
                itemInteractableObj = hit.collider.GetComponent<Interactable>();
                UIManager.instance.OpenInteractUI(itemInteractableObj.interactableText);

                if (player.playerInput.interactInput)
                {
                    itemInteractableObj.Interact(playerManager, this);

                    if (itemInteractableObj.interactType == Interactable.InteractType.Item)
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

                if (itemInteractableObj != null && player.playerInput.interactInput)
                {
                    UIManager.instance.CloseItemPopUpUI();
                    UIManager.instance.CloseMessagePopUpUI();

                    switch (itemInteractableObj.interactType)
                    {
                        case Interactable.InteractType.Item:
                            break;

                        case Interactable.InteractType.Message:
                            break;
                    }

                    AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Interact2]);
                    itemInteractableObj = null;
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

        switch (item.itemType)
        {
            case UsingItem.UsingItemType.EstusFlask:
                if (player.playerInventory.estusCount <= 0)
                    return;

                curUsingItem = PoolManager.instance.GetItem((int)PoolManager.Item.EstusFlask);
                player.playerInventory.estusCount--;
                UIManager.instance.quickSlotsUI.UpdateUsingItemUI(item, player.playerInventory.estusCount);
                break;
        }

        curUsingItem.transform.parent = player.playerItemSlotManager.leftHandSlot.parentOverride;
        curUsingItem.transform.position = player.playerItemSlotManager.leftHandSlot.parentOverride.transform.position;
        curUsingItem.transform.localRotation = Quaternion.identity;
        player.playerAnimator.animator.SetBool(animatorData.isItemUseParameter, true);
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

        player.playerAnimator.animator.SetBool(animatorData.isItemUseParameter, false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using SystemData;

public class PlayerBehavior : MonoBehaviour
{
    private PlayerManager player;

    [Header("Data")]
    private PlayerAnimatorData animatorData;
    private InteractData interactData;

    [Header("Interact")]
    [SerializeField]
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

    public void CheckInteractableObject(PlayerManager player)
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, interactData.InteractCheckRadius, transform.forward, out hit, interactData.InteractCheckDis, PlayerCamera.instance.targetLayer))
        {
            if (hit.collider.tag == interactData.InteractObjTag)
            {
                interactableObj = hit.collider.GetComponent<Interactable>();
            }
        }
        else
        {
            interactableObj = null;
        }


        if (interactableObj != null)
        {
            UIManager.instance.OpenInteractUI(interactableObj.interactableText);
        }
        else
        {
            UIManager.instance.CloseInteractUI();
        }
    }

    public void BehaviourAction()
    {
        if (player.playerBehavior.interactableObj != null)
        {
            interactableObj.Interact(player, this);

            switch (interactableObj.interactType)
            {
                case Interactable.InteractType.Item:
                case Interactable.InteractType.Message:
                case Interactable.InteractType.FogWall:
                case Interactable.InteractType.LockDoor:
                    break;
            }
        }
        else
        {
            UIManager.instance.CloseItemPopUpUI();
            UIManager.instance.CloseMessagePopUpUI();
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
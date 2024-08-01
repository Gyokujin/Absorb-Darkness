using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingItemPickUp : Interactable
{
    [SerializeField]
    private UsingItem dropUsingItem;

    public override void Interact(PlayerManager player, PlayerBehavior playerBehavior)
    {
        base.Interact(player, playerBehavior);
        PickUpUsingItem(player, playerBehavior);
    }

    void PickUpUsingItem(PlayerManager player, PlayerBehavior playerBehavior)
    {
        player.playerAnimator.PlayTargetAnimation("PickUp", true);
        InventoryManager.instance.GetUsingItem(dropUsingItem);
        UIManager.instance.OpenItemPopUpUI(dropUsingItem.itemName, dropUsingItem.itemIcon.texture);

        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.PickUp]);
        gameObject.SetActive(false);
    }
}
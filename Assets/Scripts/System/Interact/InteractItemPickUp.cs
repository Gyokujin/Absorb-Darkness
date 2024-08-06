using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItemPickUp : Interactable
{
    [SerializeField]
    private InteractItem dropInteractItem;

    public override void Interact(PlayerManager player, PlayerBehavior playerBehavior)
    {
        base.Interact(player, playerBehavior);
        PickUpInteractItem(player, playerBehavior);
    }

    void PickUpInteractItem(PlayerManager player, PlayerBehavior playerBehavior)
    {
        player.playerAnimator.PlayTargetAnimation("PickUp", true);
        InventoryManager.instance.GetInteractItem(dropInteractItem);
        UIManager.instance.OpenItemPopUpUI(dropInteractItem.itemName, dropInteractItem.itemIcon.texture);

        // AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.PickUp]);
        gameObject.SetActive(false);
    }
}
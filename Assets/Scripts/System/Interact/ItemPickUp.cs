using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    [SerializeField]
    private Item dropItem;

    public override void Interact(PlayerManager player, PlayerBehavior playerBehavior)
    {
        base.Interact(player, playerBehavior);
        PickUpItem(player, playerBehavior);
    }

    void PickUpItem(PlayerManager player, PlayerBehavior playerBehavior)
    {
        player.playerAnimator.PlayTargetAnimation("PickUp", true);
        InventoryManager.instance.ItemLoot(dropItem);
        gameObject.SetActive(false);
    }
}
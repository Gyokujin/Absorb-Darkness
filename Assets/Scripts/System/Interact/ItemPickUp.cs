using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    [SerializeField]
    private Item dropItem;

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        PickUpItem(player);
    }

    void PickUpItem(PlayerManager player)
    {
        player.playerAnimator.PlayTargetAnimation("PickUp", true);
        player.playerBehavior.ItemLoot(dropItem);
        gameObject.SetActive(false);
    }
}
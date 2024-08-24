using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

public class ItemPickup : Interactable
{
    [Header("Data")]
    private PlayerAnimatorData playerAnimatorData;

    [SerializeField]
    private Item dropItem;

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        PickupItem(player);
    }

    void PickupItem(PlayerManager player)
    {
        player.playerAnimator.PlayTargetAnimation(playerAnimatorData.PickupAnimation, true);
        player.playerBehavior.ItemLoot(dropItem);
        gameObject.SetActive(false);
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Pickup]);
    }
}